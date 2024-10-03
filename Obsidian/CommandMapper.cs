using Obsidian.Commands;
using Obsidian.Commands.Implementation;
using Obsidian.Commands.Interface;
namespace Obsidian;
public sealed class CommandMapper
{
    private readonly CommandFactory _commandFactory;
    public CommandMapper(string baseDirectory)
    {
        // Crea un'istanza di ObsidianConductor come implementazione di ICharacterRepository
        ICharacterRepository repository = new CharacterFileRepository(ObsidianConductor.CreateInstance(baseDirectory));

        // Inizializza la CommandFactory passando il repository
        _commandFactory = new CommandFactory(repository);
    }

    /// <summary>
    /// Parses and handles the user input command.
    /// </summary>
    /// <param name="command">The command string input by the user.</param>
    public async Task ExecuteCommand(string command)
    {
        try
        {
            var parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0)
            {
                ObsidianConsoleUtilities.PrintMessage("No command provided.");
                return;
            }

            var action = parts[0].ToLower();
            var entityType = parts.Length > 1 ? parts[1] : null;

            if (action == "help")
            {
                PrintCommandSummary();
                return;
            }

            if (entityType == null)
            {
                ObsidianConsoleUtilities.PrintMessage("Invalid command format. Type 'help' for a list of commands.");
                return;
            }

            try
            {
                var commandInstance = _commandFactory.CreateCommand(action, entityType);
                await commandInstance.ExecuteAsync(parts);
                ObsidianConsoleUtilities.PrintDelimiter();
            }
            catch (InvalidOperationException ex)
            {
                ObsidianConsoleUtilities.PrintError($"Error: {ex.Message}\n");
            }
        }
        catch (Exception ex)
        {
            ObsidianConsoleUtilities.PrintError($"Error: {ex.Message}\n");
        }
    }

    

    /// <summary>
    /// Prints the command summary in yellow.
    /// </summary>
    private static void PrintCommandSummary()
    {
        ObsidianConsoleUtilities.PrintDelimiter();
        ObsidianConsoleUtilities.PrintMessage("\t\t\t<COMMAND SUMMARY>\n");
        ObsidianConsoleUtilities.PrintMessage("\tmk ch -n <name> -a <alias> -app <aspetto> -d <datanascita (format YYYY/MM/DD)>\n\t\tCreate a new character\n");
        ObsidianConsoleUtilities.PrintMessage("\tdel ch -n <name>\n\t\tDelete a character by name\n");
        ObsidianConsoleUtilities.PrintMessage("\tadd rec -n <name> -t <text> -d <date (format YYYY/MM/DD)>\n\t\tAdd a new record to a character\n");
        ObsidianConsoleUtilities.PrintMessage("\tedit rec -n <name> -id <id> -t <text>\n\t\tEdit an existing record by ID\n");
        ObsidianConsoleUtilities.PrintMessage("\tdel rec -n <name> -id <id>\n\t\tDelete a record by ID for a character\n");
        ObsidianConsoleUtilities.PrintMessage("\tread ch -n <name>\n\t\tRead and display the entire content of a character file\n");
        ObsidianConsoleUtilities.PrintMessage("\thelp\n\t\tShow this command summary");
        ObsidianConsoleUtilities.PrintDelimiter();
    }
}
