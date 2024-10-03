using Data.Models;
using Obsidian.Commands.Interface;

namespace Obsidian.Commands.Implementation;
public class CharacterFileRepository(ObsidianConductor conductor) : ICharacterRepository
{
    private readonly ObsidianConductor _conductor = conductor;
    public async Task<Character> GetCharacterAsync(string name) => await _conductor.GetCharacterFromFileAsync(name);
    public async Task CreateCharacterAsync(Character character) => await _conductor.CreateCharacterFileAsync(character.Nome, character);
    public async Task UpdateCharacterAsync(Character character) => await _conductor.UpdateCharacterFileAsync(character.Nome, character);
    public async Task DeleteCharacterAsync(string name) => await _conductor.DeleteCharacterFileAsync(name);
    public async Task AddRecordAsync(string name, CharacterRecord record) => await _conductor.AddRecordToCharacterAsync(name, record);
    public async Task DeleteRecordByIdAsync(string name, int recordId) => await _conductor.DeleteCharacterRecordByIdAsync(name, recordId);
    public async Task EditRecordAsync(string name, int recordId, string newText) => await _conductor.EditCharacterRecordAsync(name, recordId, newText);
}