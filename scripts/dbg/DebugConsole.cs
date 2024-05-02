using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTS.Loaders;

namespace RTS.scripts.dbg
{
    internal class DebugConsole
    {
        public void Print()
        {
            
        }
        public static void TestLoader()
        {
            Loaders.Preloader.GetTiles("res://prefabs/tiles");
        }
    }
}
