using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace RTS
{
    public enum ControlModes
    {
        None, TileEditor, ObjectEditor
    }
    public static class MainCommand
    {
        public const string SUFFIX = "Tile_";
        static public int GridSize = 5;
        static public ControlModes Mode = ControlModes.None;
        static public Node3D RootNode;
        static public Node3D Field;
        static public Node3D AsNode3D(Node node) => (Node3D)node;
    }
}
