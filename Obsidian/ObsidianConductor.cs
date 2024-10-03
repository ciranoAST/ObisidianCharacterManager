using Data.Models;
using ObsidianExceptions;
using System.Text;
namespace Obsidian;
public sealed class ObsidianConductor
{
    private readonly string _baseDirectory;
    private ObsidianConductor(string baseDirectory)
    {
        _baseDirectory = baseDirectory;
        if (!Directory.Exists(_baseDirectory))
            Directory.CreateDirectory(_baseDirectory);
    }
    public static ObsidianConductor CreateInstance(string baseDirectory) => new(baseDirectory);
    /// <summary>
    /// Creates a new file for the character with the provided character data asynchronously.
    /// </summary>
    /// <param name="characterName">The name of the character.</param>
    /// <param name="character">The character data to write to the file.</param>
    /// <exception cref="InvalidOperationException">Thrown if the file already exists.</exception>
    public async Task CreateCharacterFileAsync(string characterName, Character character)
    {
        var filePath = GetCharacterFilePath(characterName);
        if (File.Exists(filePath))
            throw new ObsidianConductorException($"Character file '{characterName}' already exists.", nameof(CreateCharacterFileAsync));

        await File.WriteAllTextAsync(filePath, GenerateCharacterMarkdown(character));
    }

    /// <summary>
    /// Deletes a character's file in a case-insensitive manner asynchronously.
    /// </summary>
    /// <returns>Returns true if the file is deleted, else false</returns>
    /// <param name="characterName">The name of the character whose file is to be deleted.</param>
    /// <exception cref="FileNotFoundException">Thrown if the character file does not exist.</exception>
    public async Task<bool> DeleteCharacterFileAsync(string characterName)
    {
        var directoryInfo = new DirectoryInfo(_baseDirectory);
        var file = directoryInfo.GetFiles().FirstOrDefault(f => string.Equals(f.Name, $"{characterName}.md", StringComparison.OrdinalIgnoreCase));

        if (file != null)
        {
            await Task.Run(() => File.Delete(file.FullName));
            return true;
        }
        throw new ObsidianConductorException($"Character file '{characterName}' not found in directory '{_baseDirectory}'.", nameof(DeleteCharacterFileAsync));
    }

    /// <summary>
    /// Adds a new record (note) to a character's file and updates the file asynchronously.
    /// The ID of the new record is automatically generated based on the number of existing records.
    /// </summary>
    /// <param name="characterName">The name of the character to add the record to.</param>
    /// <param name="newRecord">The new record to add (without an ID, which will be generated automatically).</param>
    public async Task AddRecordToCharacterAsync(string characterName, CharacterRecord newRecord)
    {
        var character = await GetCharacterFromFileAsync(characterName);
        newRecord.Id = GetNextRecordId(character);
        character.Records.Add(newRecord);
        await UpdateCharacterFileAsync(characterName, character);
    }

    /// <summary>
    /// Deletes a record from a character's file by its ID asynchronously.
    /// </summary>
    /// <param name="characterName">The name of the character whose record is to be deleted.</param>
    /// <param name="recordId">The ID of the record to delete.</param>
    /// <exception cref="InvalidOperationException">Thrown if the record is not found.</exception>
    public async Task DeleteCharacterRecordByIdAsync(string characterName, int recordId)
    {
        var character = await GetCharacterFromFileAsync(characterName);
        var record = character.Records.FirstOrDefault(r => r.Id == recordId) ?? throw new ObsidianConductorException($"Record with ID {recordId} not found for character '{characterName}'.", nameof(DeleteCharacterRecordByIdAsync));
        character.Records.Remove(record);        
        await UpdateCharacterFileAsync(characterName, character);
    }

    /// <summary>
    /// Edits a specific record of a character by its ID asynchronously.
    /// </summary>
    /// <param name="characterName">The name of the character whose record is to be edited.</param>
    /// <param name="recordId">The ID of the record to edit.</param>
    /// <param name="newText">The new text to update in the record.</param>
    /// <exception cref="InvalidOperationException">Thrown if the record is not found.</exception>
    public async Task EditCharacterRecordAsync(string characterName, int recordId, string newText)
    {
        var character = await GetCharacterFromFileAsync(characterName);
        var record = character.Records.FirstOrDefault(r => r.Id == recordId) ?? throw new ObsidianConductorException($"Record with ID {recordId} not found for character '{characterName}'.", nameof(EditCharacterRecordAsync));
        record.Testo = newText;
        await UpdateCharacterFileAsync(characterName, character);
    }

    /// <summary>
    /// Gets a specific record from a character file by its ID asynchronously.
    /// </summary>
    /// <param name="characterName">The name of the character whose record is to be retrieved.</param>
    /// <param name="recordId">The ID of the record to retrieve.</param>
    /// <returns>Returns the record with the specified ID.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the record is not found.</exception>
    public async Task<CharacterRecord> GetRecordByIdAsync(string characterName, int recordId)
    {
        var character = await GetCharacterFromFileAsync(characterName);
        var record = character.Records.FirstOrDefault(r => r.Id == recordId);
        return record ?? throw new ObsidianConductorException($"Record with ID {recordId} not found for character '{characterName}'.", nameof(GetRecordByIdAsync));
    }

    /// <summary>
    /// Gets all records from a character file asynchronously.
    /// </summary>
    /// <param name="characterName">The name of the character whose records are to be retrieved.</param>
    /// <returns>Returns a list of all records associated with the character.</returns>
    public async Task<List<CharacterRecord>> GetAllCharacterRecordsAsync(string characterName)
        => (await GetCharacterFromFileAsync(characterName)).Records;

    /// <summary>
    /// Updates the character file with new data asynchronously.
    /// </summary>
    /// <param name="characterName">The name of the character whose file is to be updated.</param>
    /// <param name="character">The updated character data.</param>
    public async Task UpdateCharacterFileAsync(string characterName, Character character)
        => await File.WriteAllTextAsync(GetCharacterFilePath(characterName), GenerateCharacterMarkdown(character));

    /// <summary>
    /// Reads and parses the character's data from their file asynchronously.
    /// </summary>
    /// <param name="characterName">The name of the character whose file is to be read.</param>
    /// <returns>Returns a Character object parsed from the file.</returns>
    public async Task<Character> GetCharacterFromFileAsync(string characterName)
        => ParseCharacterFromMarkdown(await ReadCharacterFileContentAsync(characterName));

    /// <summary>
    /// Reads the entire content of the character file asynchronously.
    /// </summary>
    /// <param name="characterName">The name of the character whose file to read.</param>
    /// <returns>The full text of the character file.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the character file does not exist.</exception>
    public async Task<string> ReadFullFileAsync(string characterName)
        => await ReadCharacterFileContentAsync(characterName);
    #region Utility
    /// <summary>
    /// Reads the content of a character's file asynchronously.
    /// </summary>
    /// <param name="characterName">The name of the character whose file is to be read.</param>
    /// <returns>Returns the full content of the character file.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the character file does not exist.</exception>
    private async Task<string> ReadCharacterFileContentAsync(string characterName)
    {
        var filePath = GetCharacterFilePath(characterName);
        if (File.Exists(filePath))
            return await File.ReadAllTextAsync(filePath);

        throw new ObsidianConductorException($"Character file '{characterName}' not found in directory '{_baseDirectory}'.", nameof(ReadCharacterFileContentAsync));
    }
    /// <summary>
    /// Helper method to get the next available record ID.
    /// </summary>
    /// <param name="character">The character whose records are being checked.</param>
    /// <returns>The next available record ID.</returns>
    private static int GetNextRecordId(Character character)
        => character.Records?.Count > 0 ? character.Records.Max(r => r.Id) + 1 : 1;
    /// <summary>
    /// Generates the Markdown content from a Character object.
    /// </summary>
    /// <param name="character">The character whose data will be converted to Markdown format.</param>
    /// <returns>Returns a Markdown-formatted string for the character.</returns>
    private static string GenerateCharacterMarkdown(Character character)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"# {character.Nome} ({character.Alias})");
        sb.AppendLine();
        sb.AppendLine($"**Data di Nascita:** {character.DataNascita?.ToShortDateString()}");
        sb.AppendLine($"**Aspetto:** {character.Aspetto}");
        sb.AppendLine();
        sb.AppendLine("## Appunti:");

        foreach (var record in character.Records)
        {
            sb.AppendLine($"### Appunto {record.Id} - {record.Data?.ToShortDateString()}:");
            sb.AppendLine($"{record.Testo}");
            sb.AppendLine();
        }

        return sb.ToString();
    }
    /// <summary>
    /// Helper method to get the full file path for a given character.
    /// </summary>
    /// <param name="characterName">The name of the character.</param>
    /// <returns>The full file path for the character file.</returns>
    private string GetCharacterFilePath(string characterName)
        => Path.Combine(_baseDirectory, $"{characterName}.md");
    /// <summary>
    /// Parses the Markdown content and converts it into a Character object, with error handling.
    /// </summary>
    /// <param name="fileContent">The Markdown content of the file.</param>
    /// <returns>Returns a Character object parsed from the file content. If parsing fails, returns null.</returns>
    private static Character ParseCharacterFromMarkdown(string fileContent)
    {
        if (string.IsNullOrWhiteSpace(fileContent))
            throw new ObsidianConductorException("fileContent empty or null", nameof(ParseCharacterFromMarkdown));

        return ObsidianWizard.ParseCharacterFromMarkdown(fileContent)
            ?? throw new ObsidianConductorException("Failed to parse the character from the Markdown content.", nameof(ParseCharacterFromMarkdown));
    }
    #endregion
}