using Obsidian.Commands.Interface;
using ObsidianExceptions;

namespace Obsidian.Commands.Implementation;
public sealed class ReadCharacterCommand(ICharacterRepository repository) : BaseCommand, ICommand
{
    private readonly ICharacterRepository _repository = repository;

    public async Task ExecuteAsync(string[] args)
    {
        string name = null;

        for (var i = 0; i < args.Length; i++)
        {
            if (args[i] == "-n")
                name = GatherValuesUntilNextFlag(args, ref i);
        }

        if (string.IsNullOrEmpty(name))
            throw new ObsidianCharacterException("Name is required");

        try
        {
            var character = await _repository.GetCharacterAsync(name);
            if (character != null)
            {
                ObsidianConsoleUtilities.PrintMessage($"Content of '{name}.md':\n");
                ObsidianConsoleUtilities.PrintMessage(character.ToMarkdown());  // Assuming you have a ToMarkdown method
            }
        }
        catch (Exception ex)
        {
            throw new ObsidianCharacterException(ex);
        }
    }
}
