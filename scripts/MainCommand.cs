using Godot;

namespace RTS;

public static class MainCommand
{
    public static Node3D RootNode;

    public static string PathToMainMenu = "res://prefabs/menus/MainMenu.tscn";
    public static string PathToWorldsPage = "res://prefabs/menus/WorldPageMenu.tscn";
    public static string PathToWorld = "res://prefabs/World.tscn";
    public static string PathToSettings = null;

    public static string SceneToLoad { get; set; }
}