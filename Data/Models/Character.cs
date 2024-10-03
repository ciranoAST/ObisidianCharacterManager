namespace Data.Models;
public sealed class Character
{
    public string Nome { get; set; }
    public string Alias { get; set; }
    public DateTime? DataNascita { get; set; }
    public string Aspetto { get; set; }
    public List<CharacterRecord> Records { get; set; } = [];
}