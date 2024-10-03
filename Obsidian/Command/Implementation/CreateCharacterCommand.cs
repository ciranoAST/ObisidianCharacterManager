using Obsidian.Commands.Interface;
using ObsidianExceptions;
namespace Obsidian.Commands.Implementation;
public sealed class CreateCharacterCommand(ICharacterRepository repository) : BaseCommand, ICommand
{
    private readonly ICharacterRepository _repository = repository;
    public async Task ExecuteAsync(string[] args)
    {
        // Codice per gestire i parametri e creare il personaggio
        string name = null;
        string alias = null;
        string aspetto = null;
        DateTime? dataNascita = null;

        for (var i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "-n":
                    name = GatherValuesUntilNextFlag(args, ref i);
                    break;
                case "-a":
                    alias = GatherValuesUntilNextFlag(args, ref i);
                    break;
                case "-app":
                    aspetto = GatherValuesUntilNextFlag(args, ref i);
                    break;
                case "-d":
                    var dateString = GatherValuesUntilNextFlag(args, ref i);
                    if (DateTime.TryParseExact(dateString, "yyyy/MM/dd", null, System.Globalization.DateTimeStyles.None, out var parsedDate))
                        dataNascita = parsedDate;
                    else
                        throw new ObsidianCharacterException("Invalid date format. Please use YYYY/MM/DD.");
                    break;
            }
        }

        if (string.IsNullOrEmpty(name))
            throw new ObsidianCharacterException("Name is required");

        await _repository.CreateCharacterAsync(new() { Nome = name, Alias = alias, Aspetto = aspetto, DataNascita = dataNascita, Records = [] });
        ObsidianConsoleUtilities.PrintMessage($"Character '{name}' created successfully.");
    }
}