namespace Obsidian.Commands;
public class BaseCommand
{
    #region Utility
    protected static string GatherValuesUntilNextFlag(string[] parts, ref int i)
    {
        var values = new List<string>();
        while (i + 1 < parts.Length && !parts[i + 1].StartsWith('-'))
            values.Add(parts[++i]);
        return string.Join(" ", values);
    }
    #endregion
}