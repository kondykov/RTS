using Godot;

namespace RTS.UI;

public partial class UIHandler : Control
{
    #region MainMenu

    public void ShowMainMenu() => GetTree().ChangeSceneToFile(MainCommand.PathToMainMenu);

    #endregion
}