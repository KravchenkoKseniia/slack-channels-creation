// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SlackNet;
using File = System.IO.File;
using dotenv.net;

DotEnv.Load(options: new DotEnvOptions(envFilePaths: new[] { "D:\\rider_projects\\SlackAPI\\.env" }));

var apiToken = Environment.GetEnvironmentVariable("SLACK_API_TOKEN");
var appToken = Environment.GetEnvironmentVariable("SLACK_APP_TOKEN");
    
if (string.IsNullOrEmpty(apiToken) || string.IsNullOrEmpty(appToken))
{
    Console.Error.WriteLine("SLACK_API_TOKEN and SLACK_APP_TOKEN must be set");
    return;
} 
    
Console.WriteLine("SLACK_API_TOKEN and SLACK_APP_TOKEN are set");
    

// Initialize the Slack API client
var api = new SlackServiceBuilder()
        .UseApiToken(apiToken)
        .GetApiClient();

// Socket Mode client for event handling
var socketModeClient = new SlackServiceBuilder()
        .UseAppLevelToken(appToken)
        .GetSocketModeClient();

// Get users emails
var usersId = new List<string>();
string[] emails;

try
{
    emails =  File.ReadAllLines("D:\\rider_projects\\SlackAPI\\test_mails.txt");
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Error reading emails: {ex.Message}");
    return;
}
    
try
{
    // Call the conversations.create API method
    var result = await api.Conversations.Create("test-users-private", true);

    foreach (var email in emails)
    {
        try
        {
            var user = await api.Users.LookupByEmail(email);
            usersId.Add(user.Id);
            Console.WriteLine($"Found user for {email}: {user.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error looking up email {email}: {ex.Message}");
        }
    }

    // Add users to the channel
    await api.Conversations.Invite(result.Id, usersId);

    // Log the result
    Console.WriteLine($"Channel created: {result.Id}");
}
catch (Exception ex) 
{ 
    Console.Error.WriteLine($"Error creating conversation: {ex.Message}");
}

await socketModeClient.Connect();
Console.WriteLine("Chat was created successfully");



