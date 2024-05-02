using RTS.Editor;
using System;

namespace RTS.Debug
{
    public static class DebugConsole
    {
        public static void PrintLastClickedNode()
        {
            if (MapEditorCommand.LastClickedNode == null) Console.WriteLine("Clicked node: NULL");
            else Console.WriteLine($"Clicked node: {MapEditorCommand.LastClickedNode} {MapEditorCommand.LastClickedNode.GlobalPosition}, path {MapEditorCommand.LastClickedNode.GetPath()}");
        }
    }
}
