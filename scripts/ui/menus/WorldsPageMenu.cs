using Godot;

namespace RTS.UI.Menu;

public partial class WorldsPageMenu : Control
{
    public void BackToMainMenu() => GetTree().ChangeSceneToFile(MainCommand.PathToMainMenu);
    public void LoadFirstWorld() => GetTree().ChangeSceneToFile(MainCommand.PathToWorld);
}