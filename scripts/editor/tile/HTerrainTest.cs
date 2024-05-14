using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using GodotPlugins;

namespace RTS.Editor
{
    public class HTerrainTest
    { 
        //GDScript gdScript = GD.Load<GDScript>("res://addons/zylann.hterrain/hterrain.gd");
        GDScript gdScript = GD.Load<GDScript>("res://scripts/TestHTerrains.gd");
        Node _HTerrain = MainCommand.RootNode.GetNode("Field/HTerrain");
        static Node _hTerrain;
        public void _Init()
        {
            _hTerrain = MainCommand.RootNode.GetNode("Field/HTerrain");
            Console.WriteLine(_hTerrain.Name);
            var asd = (GodotObject)gdScript.New(_HTerrain);
            asd.Call("_ready");
        }
    }
}
