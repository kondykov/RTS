using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.editor
{
    public class MapEditorCommand
    {
        public const string SUFFIX = "Tile_";
        public int GridSize = 20;
        public static Node3D FieldSelector = null;
    }
}
