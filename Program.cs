namespace SlackAPI;
using System;

public static class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Hello World!");
        Console.WriteLine("Enter the name of the channel you want to create: ");
        var channelName = Console.ReadLine();

        if (string.IsNullOrEmpty(channelName))
        {
            Console.Error.WriteLine("Channel name cannot be empty");
            return;
        }
        
        Console.WriteLine("Is the channel private? (y/n): ");
        var isPrivateInput = Console.ReadLine();
        bool isPrivate = isPrivateInput != null && isPrivateInput.Equals("y", StringComparison.OrdinalIgnoreCase);

        try 
        {
            await SlackChannelCreator.CreateChannel(channelName, isPrivate);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error creating channel: {ex.Message}");
        }
    }
}