using Obsidian.Commands.Interface;
using ObsidianExceptions;
namespace Obsidian.Commands.Implementation;
public sealed class DeleteRecordCommand(ICharacterRepository repository) : BaseCommand, ICommand
{
    private readonly ICharacterRepository _repository = repository;
    public async Task ExecuteAsync(string[] args)
    {
        string name = null;
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
            }
        }

        if (string.IsNullOrEmpty(name) || id == 0)
            throw new ObsidianRecordException("Name and Id are required");

        try
        {
            await _repository.DeleteRecordByIdAsync(name, id);
            ObsidianConsoleUtilities.PrintMessage($"Record {id} successfully deleted for character '{name}'.");
        }
        catch (Exception ex)
        {
            throw new ObsidianRecordException(ex);
        }
    }
}