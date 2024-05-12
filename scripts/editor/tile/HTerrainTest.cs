using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using GodotPlugins;

namespace RTS.scripts.editor.tile
{
    public class HTerrainTest
    { 
        //GDScript gdScript = GD.Load<GDScript>("res://addons/zylann.hterrain/hterrain.gd");
        GDScript gdScript = GD.Load<GDScript>("res://script/TestHTerrain/HTerrain.gd");
        Node _HTerrain = MainCommand.RootNode.GetNode("RootHTerrain/HTerrain");
        void _Init()
        {
            var asd = (GodotObject)gdScript.New(_HTerrain);
            asd.Call("test");
        }
    }
}
