using Data.Models;
namespace Obsidian.Commands.Interface;
public interface ICharacterRepository
{
    Task<Character> GetCharacterAsync(string name);
    Task CreateCharacterAsync(Character character);
    Task UpdateCharacterAsync(Character character);
    Task DeleteCharacterAsync(string name);
    Task AddRecordAsync(string name, CharacterRecord record);
    Task DeleteRecordByIdAsync(string name, int recordId);
    Task EditRecordAsync(string name, int recordId, string newText);
}