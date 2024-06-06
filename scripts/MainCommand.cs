using Godot;
using RTS.Camera;

namespace RTS
{
    public enum ControlModes
    {
        None, TileEditor, ObjectEditor
    }
    public static class MainCommand
    {
        static public ControlModes Mode = ControlModes.None;
        static public Node3D RootNode;
    }
}
