using System;
using Godot;

namespace RTS.UI.Menu;

public partial class PauseMenu : Control
{
    public void Continue()
    {
        GetTree().Paused = false;
        Visible = false;
    }

    public void CreateNewGame()
    {
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile(MainCommand.PathToWorld);
    }

    public void ShowStats() => Console.WriteLine("ShowStatistics");

    public void Exit()
    {
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile(MainCommand.PathToMainMenu);
    }

    public override void _Ready() => Visible = false;

    public override void _Process(double delta)
    {
        if (!Visible) MouseModeHandler.CaptureMouse();
        if (Input.IsActionJustPressed("ESC"))
        {
            Visible = !Visible;
            if (Visible)
            {
                MouseModeHandler.VisibleMouse();
                GetTree().Paused = true;
            }
            else
            {
                MouseModeHandler.CaptureMouse();
                GetTree().Paused = false;
            }
        }
    }
}