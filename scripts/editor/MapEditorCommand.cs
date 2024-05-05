using Godot;

namespace RTS.Editor
{
    public class MapEditorCommand
    {
        public const string SUFFIX = "Tile_";
        public int GridSize = 100;
        public static Node3D FieldSelector = null;
        public static Node3D LastClickedNode = null;
        public static Vector3 LastClickedPosition = new Vector3(0, 0, 0);
    }
}
