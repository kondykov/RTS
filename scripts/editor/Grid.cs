using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace RTS.Editor
{
    public class Grid
    {
        private static Node3D _gridMap;
        private List<Node3D> _nodes;


        public void AddTile(Node3D tile)
        {
            tile.SetScript("res://scripts/editor/tile/StaticNodeHandler.cs");
            _nodes.Add(tile);
        }
        /// <summary>
        /// Global position. Return a tile from field.
        /// </summary>
        /// <param name="position">Position int</param>
        /// <returns></returns>
        public Node3D GetTile(Vector3 position)
        {
            return null;
        }
    }
}
