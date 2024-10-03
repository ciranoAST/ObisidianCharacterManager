using ObsidianCharacterManager.Manager;

var obsidianPath = DirectoryManager.SetupDirectory(CharacterConfigurationManager.LoadConfiguration());
var commandManager = new CommandManager(obsidianPath);
await commandManager.Run();