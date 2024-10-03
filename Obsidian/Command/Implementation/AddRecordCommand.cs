using Obsidian.Commands.Interface;
using ObsidianExceptions;
namespace Obsidian.Commands.Implementation;
public sealed class AddRecordCommand(ICharacterRepository repository) : BaseCommand, ICommand
{
    private readonly ICharacterRepository _repository = repository;
    public async Task ExecuteAsync(string[] args)
    {
        string name = null;
        string text = null;
        DateTime? date = null;

        for (var i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "-n":
                    name = GatherValuesUntilNextFlag(args, ref i);
                    break;
                case "-t":
                    text = GatherValuesUntilNextFlag(args, ref i);
                    break;
                case "-d":
                    if (DateTime.TryParse(GatherValuesUntilNextFlag(args, ref i), out var parsedDate))
                        date = parsedDate;
                    break;
            }
        }

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(text))
            throw new ObsidianRecordException("Name and Text are required");

        try
        {
            await _repository.AddRecordAsync(name, new() { Testo = text, Data = date });
            ObsidianConsoleUtilities.PrintMessage($"Record added successfully to character '{name}'.");
        }
        catch (Exception ex)
        {
            throw new ObsidianRecordException(ex);
        }
    }
}