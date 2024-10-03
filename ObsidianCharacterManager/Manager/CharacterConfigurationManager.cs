using Microsoft.Extensions.Configuration;
namespace ObsidianCharacterManager.Manager;
public static class CharacterConfigurationManager
{
    /// <summary>
    /// Loads the configuration settings from the 'appsettings.json' file.
    /// The configuration is set to automatically reload on changes and will not throw an error if the file is missing.
    /// </summary>
    /// <returns>An <see cref="IConfiguration"/> object containing the configuration values.</returns>
    public static IConfiguration LoadConfiguration() => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Set base directory to current working directory
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // Load 'appsettings.json' file if it exists
            .Build();
}
