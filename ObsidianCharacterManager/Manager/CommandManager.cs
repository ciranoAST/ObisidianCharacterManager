using Obsidian;
namespace ObsidianCharacterManager.Manager;

/// <summary>
/// Initializes a new instance of the <see cref="CommandManager"/> class.
/// This class is responsible for managing and executing user commands via the CommandMapper.
/// </summary>
/// <param name="obsidianPath">The path where Obsidian files are stored. Passed to the CommandMapper for command execution.</param>
public class CommandManager(string obsidianPath)
{
    private readonly CommandMapper _commandMapper = new CommandMapper(obsidianPath);

    /// <summary>
    /// Starts the command manager, displays the help message, and continuously listens for user input.
    /// Executes user commands in a loop until the application is terminated.
    /// </summary>
    public async Task Run()
    {
        await _commandMapper.ExecuteCommand("help");
        while (true)
        {
            string userInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(userInput))
            {
                if (userInput.Trim().ToLower().Equals("exit"))
                {
                    Console.WriteLine("Exiting the application...");
                    break;
                }

                await _commandMapper.ExecuteCommand(userInput);
            }
        }
    }
}
