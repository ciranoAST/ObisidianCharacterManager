namespace Obsidian.Commands.Interface;
public interface ICommand
{
    Task ExecuteAsync(string[] args);
}