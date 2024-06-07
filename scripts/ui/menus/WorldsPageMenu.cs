using Godot;

namespace RTS.UI.Menu;

public partial class WorldsPageMenu : Control
{
    public void ShowWorldsPage() => GetTree().ChangeSceneToFile(MainCommand.PathToWorldsPage);
}