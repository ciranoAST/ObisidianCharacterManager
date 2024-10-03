using Obsidian.Commands.Implementation;
using Obsidian.Commands.Interface;

namespace Obsidian.Commands;
public class CommandFactory(ICharacterRepository repository)
{
    private readonly ICharacterRepository _repository = repository;
    public ICommand CreateCommand(string action, string entityType)
    =>
        action switch
        {
            "mk" when entityType == "ch" => new CreateCharacterCommand(_repository),
            "add" when entityType == "rec" => new AddRecordCommand(_repository),
            "edit" when entityType == "rec" => new EditRecordCommand(_repository),
            "del" when entityType == "ch" => new DeleteCharacterCommand(_repository),
            "del" when entityType == "rec" => new DeleteRecordCommand(_repository),
            "read" when entityType == "ch" => new ReadCharacterCommand(_repository),
            _ => throw new InvalidOperationException("Invalid command.")
        };
}
