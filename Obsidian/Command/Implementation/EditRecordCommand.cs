using Obsidian.Commands.Interface;
using ObsidianExceptions;
namespace Obsidian.Commands.Implementation;
public sealed class EditRecordCommand(ICharacterRepository repository) : BaseCommand, ICommand
{
    private readonly ICharacterRepository _repository = repository;
    public async Task ExecuteAsync(string[] args)
    {
        string name = null;
        string text = null;
        int id = 0;

        for (var i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "-n":
                    name = GatherValuesUntilNextFlag(args, ref i);
                    break;
                case "-id":
                    _ = int.TryParse(GatherValuesUntilNextFlag(args, ref i), out id);
                    break;
                case "-t":
                    text = GatherValuesUntilNextFlag(args, ref i);
                    break;
            }
        }

        if (string.IsNullOrEmpty(name) || id == 0 || string.IsNullOrEmpty(text))
            throw new ObsidianRecordException("Name, Id and Text are required");

        try
        {
            await _repository.EditRecordAsync(name, id, text);
            ObsidianConsoleUtilities.PrintMessage($"Record {id} successfully edited for character '{name}'.");
        }
        catch (Exception ex)
        {
            throw new ObsidianRecordException(ex);
        }
    }
}