using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Loaders
{
    public static class Preloader
    {
        private static int _id = 0;
        private static Dictionary<int, PackedScene> _tiles = new Dictionary<int, PackedScene>();
        private static List<Node3D> _objects;

        private static bool IsPackedScene(object packedScene) => packedScene.GetType() == typeof(PackedScene) ? true : false;

        private static PackedScene Load(string path)
        {
            try { return GD.Load<PackedScene>(path); }
            catch { return null; }
        }
        public static Dictionary<int, PackedScene> Tiles { get => _tiles; set => _tiles = value; }

        public static Dictionary<int, PackedScene> GetTiles(string folderName)
        {
            if(DirAccess.DirExistsAbsolute(folderName))
            {
                foreach (var item in DirAccess.GetFilesAt(folderName))
                {
                    Console.WriteLine($"{folderName}/{item}");
                    if(Load($"{folderName}/{item}") != null)
                        Tiles.Add(_id++, Load($"{folderName}/{item}"));
                }
                return Tiles;
            }
            return null;
        }
        public static PackedScene GetTile(int id) => _tiles[id];
    }
}
