using Godot;

namespace RTS.UI.Menu;

public partial class WorldsPageMenu : Control
{
    private void BackToMainMenu()
    {
        MainCommand.SceneToLoad = MainCommand.PathToMainMenu;
        GetTree().ChangeSceneToFile(LoadingScreen.Loader);
        
        //GetTree().ChangeSceneToFile(MainCommand.PathToMainMenu);
    }

    private void LoadFirstWorld()
    {
        MainCommand.SceneToLoad = MainCommand.PathToWorld;
        GetTree().ChangeSceneToFile(LoadingScreen.Loader);
    }
}