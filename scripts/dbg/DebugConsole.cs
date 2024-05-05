using RTS.Editor;
using RTS.Camera;
using Godot;
using System;

namespace RTS.Debug
{
    public static class DebugConsole
    {
        public static void Print()
        {
            if (Input.IsActionJustPressed("mouse_left_click")) Console.WriteLine(MainCommand.CurrentCamera.GetRaycast());
        }
    }
}
