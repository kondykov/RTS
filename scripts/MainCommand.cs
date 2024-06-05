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
        public const string SUFFIX = "Tile_";
        static public int GridSize = 10;
        static public ControlModes Mode = ControlModes.None;
        static public Node3D RootNode;
        static public Camera3D PerspectiveCamera;
        static public WorldCamera CurrentCamera; 
        static public Node3D Field;
        static public Node3D AsNode3D(Node node) => (Node3D)node;
    }
}
