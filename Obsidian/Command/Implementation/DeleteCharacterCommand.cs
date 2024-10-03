using Obsidian.Commands.Interface;
using ObsidianExceptions;
namespace Obsidian.Commands.Implementation;
public sealed class DeleteCharacterCommand(ICharacterRepository repository) : BaseCommand, ICommand
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
            await _repository.DeleteCharacterAsync(name);
            Console.WriteLine($"Character '{name}' deleted successfully.");
        }
        catch (Exception ex)
        {
            throw new ObsidianCharacterException(ex);
        }
    }
}