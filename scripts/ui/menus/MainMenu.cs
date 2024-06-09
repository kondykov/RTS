using System;
using Godot;

namespace RTS.UI.Menu;

public partial class MainMenu : Node
{
    public void ShowWorldsPage() => GetTree().ChangeSceneToFile(MainCommand.PathToWorldsPage);
    public void ShowStats() => Console.WriteLine("sdfhg");
    public void Exit() => GetTree().Quit();
}