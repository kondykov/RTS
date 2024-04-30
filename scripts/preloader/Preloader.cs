using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Loaders
{
    public partial class Preloader
    {
        private Node3D _loadedNode;
        private static List<PackedScene> _tiles = new List<PackedScene>();
        private static List<Node3D> _objects;

        public static List<PackedScene> Tiles { get => _tiles; set => _tiles = value; }

        public static List<string> GetFileNamesFromFolder(string folderName)
        {
            if(DirAccess.DirExistsAbsolute(folderName))
            {
                Console.Clear();
                foreach (var item in DirAccess.GetFilesAt(folderName))
                {
                    Console.WriteLine($"{folderName}/{item}");
                    //Tiles.Add(GD.Load<PackedScene>($"{folderName}/{item}"));
                    var tmp = GD.Load<PackedScene>($"res://prefabs/tiles/GrassTile_Simple.tscn");
                    Console.WriteLine(tmp);
                    Tiles.Add(tmp);
                }
            }
            return null;
        }
    }
}
