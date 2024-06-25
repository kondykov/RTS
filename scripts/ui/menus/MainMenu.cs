using System;
using Godot;
using RTS.Terrain;

namespace RTS.UI.Menu;

public partial class MainMenu : Node
{
    private void ShowWorldsPage()
    {
        GetTree().ChangeSceneToFile(MainCommand.PathToWorldsPage);
    }

    private void ShowStats()
    {
        Console.WriteLine("sdfhg");
    }

    private void Exit()
    {
        GetTree().Quit();
    }
}