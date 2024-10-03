using Microsoft.Extensions.Configuration;
using System.Text.Json;
namespace ObsidianCharacterManager.Manager;
public static class DirectoryManager
{
    // Crea e riutilizza un'istanza di JsonSerializerOptions per ottimizzare le prestazioni
    private static readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    /// <summary>
    /// Configures the directory to store character files. It prompts the user for a custom directory or uses the default one.
    /// If a custom directory is provided, it updates the path in the appsettings.json file.
    /// </summary>
    /// <param name="config">An instance of <see cref="IConfiguration"/> used to load existing settings from appsettings.json.</param>
    /// <returns>Returns the path of the directory where character files will be stored.</returns>
    public static string SetupDirectory(IConfiguration config)
    {
        Console.WriteLine("====================================");
        Console.WriteLine(" Welcome to Obsidian MD Character Manager ");
        Console.WriteLine("====================================\n");

        string currentDirectory = Directory.GetCurrentDirectory();
        Console.WriteLine($"The program is running in: {currentDirectory}");

        string defaultPath = config["ObsidianPath"] ?? Path.Combine(currentDirectory, "ObsidianCharacterFiles");

        Console.WriteLine("If you want to specify a custom folder for managing Obsidian files, enter the full path now. Press Enter to use the default location.");
        string inputPath = Console.ReadLine();
        string obsidianPath = string.IsNullOrWhiteSpace(inputPath) ? defaultPath : inputPath;

        // Crea la directory se non esiste
        EnsureDirectoryExists(obsidianPath);

        if (!string.IsNullOrWhiteSpace(inputPath))
            UpdateAppSettings(obsidianPath);

        return obsidianPath;
    }

    /// <summary>
    /// Ensures that the directory exists. If it doesn't exist, the method creates it.
    /// </summary>
    /// <param name="path">The directory path to check or create.</param>
    private static void EnsureDirectoryExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Console.WriteLine($"Created directory: {path}");
        }
        else
            Console.WriteLine($"Using existing directory: {path}");
    }

    /// <summary>
    /// Updates the "ObsidianPath" in the appsettings.json file with the provided path.
    /// This ensures that the custom path provided by the user is saved for future executions.
    /// </summary>
    /// <param name="obsidianPath">The new directory path to save in the appsettings.json file.</param>
    private static void UpdateAppSettings(string obsidianPath)
    {
        string appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

        var jsonString = File.ReadAllText(appSettingsPath);
        var appSettings = JsonSerializer.Deserialize<AppSettings>(jsonString);

        appSettings.ObsidianPath = obsidianPath;

        jsonString = JsonSerializer.Serialize(appSettings, _jsonOptions);
        File.WriteAllText(appSettingsPath, jsonString);

        Console.WriteLine($"Updated appsettings.json with new path: {obsidianPath}");
    }
}

/// <summary>
/// Represents the settings that are loaded from or saved to the appsettings.json file.
/// </summary>
public sealed record AppSettings
{
    public string ObsidianPath { get; set; }
}