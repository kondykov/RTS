using System.Collections.Generic;

namespace RTS.ui;

public enum MenuEnum
{
    MainMenu,
    WorldsPage,
    Statistics,
}

public class Menu
{
    private Dictionary<MenuEnum, string> _menuEnum = new()
    {
        { MenuEnum.MainMenu, "res://prefabs/menus/MainMenu.tscn" },
        { MenuEnum.WorldsPage, "res://prefabs/menus/MainMenu.tscn" },
        { MenuEnum.MainMenu, "res://prefabs/menus/MainMenu.tscn" },
    };
}