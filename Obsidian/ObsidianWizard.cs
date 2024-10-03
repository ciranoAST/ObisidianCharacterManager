using Data.Models;
using ObsidianExceptions;
using System.Text;
using System.Text.RegularExpressions;
namespace Obsidian;
public static partial class ObsidianWizard
{
    /// <summary>
    /// Parses the Markdown content and converts it into a Character object.
    /// </summary>
    /// <param name="fileContent">The Markdown content of the file.</param>
    /// <returns>Returns a Character object parsed from the file content.</returns>
    public static Character ParseCharacterFromMarkdown(string fileContent)
    {
        if (string.IsNullOrWhiteSpace(fileContent))
            throw new ObsidianWizardException("File content cannot be null or empty.", nameof(ParseCharacterFromMarkdown), fileContent);

        var character = new Character();
        _ = new StringBuilder();

        // Nome e Alias
        var matchNameAlias = NameAliasRegex().Match(fileContent);
        if (matchNameAlias.Success)
        {
            character.Nome = matchNameAlias.Groups["nome"].Value.Trim();
            character.Alias = matchNameAlias.Groups["alias"].Value.Trim();
        }
        else
            throw new ObsidianWizardException("Name and alias not found in the provided content.", nameof(ParseCharacterFromMarkdown), fileContent);

        // Data di Nascita
        var matchBirthDate = BirthDateRegex().Match(fileContent);
        if (matchBirthDate.Success && DateTime.TryParse(matchBirthDate.Groups["dataNascita"].Value, out var birthDate))
        {
            character.DataNascita = birthDate;
        }

        // Aspetto
        var matchAppearance = AppearanceRegex().Match(fileContent);
        if (matchAppearance.Success)
        {
            character.Aspetto = matchAppearance.Groups["aspetto"].Value.Trim();
        }

        // Appunti (Records)
        var matchesRecords = RecordRegex().Matches(fileContent);
        foreach (Match match in matchesRecords)
        {
            var record = new CharacterRecord
            {
                Id = int.Parse(match.Groups["id"].Value.Trim()),
                Testo = match.Groups["testo"].Value.Trim()
            };

            if (DateTime.TryParse(match.Groups["data"].Value, out var recordDate))
            {
                record.Data = recordDate;
            }

            character.Records.Add(record);
        }

        return character;
    }
    /// <summary>
    /// Converts a <see cref="Character"/> object into a Markdown-formatted string.
    /// </summary>
    /// <param name="character">The <see cref="Character"/> object to convert to Markdown.</param>
    /// <returns>A Markdown-formatted string representing the character's details, including name, alias, date of birth, appearance, and records.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="character"/> is null.</exception>
    public static string ToMarkdown(this Character character)
    {
        ArgumentNullException.ThrowIfNull(character);

        var sb = new StringBuilder();
        // Name and Alias
        sb.AppendLine($"# {character.Nome} ({character.Alias})");
        sb.AppendLine();

        // Date of Birth
        if (character.DataNascita.HasValue)
            sb.AppendLine($"**Data di Nascita:** {character.DataNascita.Value:yyyy/MM/dd}");
        else
            sb.AppendLine("**Data di Nascita:** N/A");

        // Appearance
        sb.AppendLine($"**Aspetto:** {character.Aspetto ?? "N/A"}");
        sb.AppendLine();
        // Notes/Records
        sb.AppendLine("## Appunti:");
        if (character.Records?.Count > 0)
        {
            foreach (var record in character.Records)
            {
                sb.AppendLine($"### Appunto {record.Id} - {(record.Data.HasValue ? record.Data.Value.ToString("yyyy/MM/dd") : "N/A")}:");
                sb.AppendLine($"{record.Testo}");
                sb.AppendLine();
            }
        }
        else
            sb.AppendLine("No records available.");

        return sb.ToString();
    }
    #region Utility
    // Nome e Alias regex (espressione generata)
    [GeneratedRegex(@"#\s*(?<nome>.+)\s+\((?<alias>.+)\)")]
    private static partial Regex NameAliasRegex();

    // Data di Nascita regex (espressione generata)
    [GeneratedRegex(@"\*\*Data di Nascita:\*\*\s*(?<dataNascita>\d{2}/\d{2}/\d{4})")]
    private static partial Regex BirthDateRegex();

    // Aspetto regex (espressione generata)
    [GeneratedRegex(@"\*\*Aspetto:\*\*\s*(?<aspetto>.+)")]
    private static partial Regex AppearanceRegex();

    // Appunti regex (espressione generata)
    [GeneratedRegex(@"### Appunto\s*(?<id>\d+)(?:\s*-\s*(?<data>\d{2}/\d{2}/\d{4})?)?\s*:\s*(?<testo>[\s\S]+?)(?=(###|$))")]
    private static partial Regex RecordRegex();
    #endregion
}