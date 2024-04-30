using Godot;
using RTS.Camera;
using RTS.editor;
using RTS.scripts.dbg;
using System;

namespace RTS.Editor
{    public partial class MapEditorCommandHandler : Node
    {
        public MapEditorCommandHandler() { }
        public void GenerateSimpleField()
        {

            PackedScene tiles = GD.Load<PackedScene>("res://prefabs/tiles/GrassTile_Simple.tscn");
            var field = MainCommand.RootNode.GetNode("Field");
            for (int i = 0; i < MainCommand.GridSize * 2; i += 2)
            {
                for (int j = 0; j < MainCommand.GridSize * 2; j += 2)
                {
                    Cell cell = new Cell();
                    cell.Node = tiles.Instantiate<Node3D>();
                    cell.Node.Name = $"{MainCommand.SUFFIX}{i}-{j}";
                    cell.Node.Position = new Vector3(i, 0, j);
                    Cell.AppendCell(cell);

                    field.AddChild(cell.Node);
                }
            }
        }
        public void RemoveSelector()
        {
            if (MapEditorCommand.FieldSelector != null)
            {
                MapEditorCommand.FieldSelector = null;
                MainCommand.RootNode.RemoveChild(MainCommand.RootNode.GetNode<Node3D>("3DCursor"));
            }
        }
        public void AddSelector(Vector3 raycastResult)
        {
            PackedScene tiles = GD.Load<PackedScene>("res://prefabs/ui/Selector.tscn");
            Node3D node = tiles.Instantiate<Node3D>();
            var vec = MainCommandHandler.Vector3FloatToInt(raycastResult);
            if ((int)vec.X % 2 == 0) vec.X += 1;
            if ((int)vec.Z % 2 == 0) vec.Z += 1;
            vec.Y = 1;
            node.Position = vec;
            node.Name = "3DCursor";
            if (MainCommand.RootNode.GetNodeOrNull("3DCursor") != null) RemoveSelector();
            MapEditorCommand.FieldSelector = node;
            MainCommand.RootNode.AddChild(node);
        }
        public void EditorHandler(WorldCamera _camera)
        {
            if (Input.IsActionJustPressed("ESC"))
                if (GetNodeOrNull("3DCursor") != null)
                    RemoveSelector();
            if (Input.IsActionJustPressed("mouse_left_click"))
            {
                var raycastResult = _camera.GetRaycast();
                if (raycastResult.X >= 0 && raycastResult.X < MainCommand.GridSize * 2 && raycastResult.Z >= 0 && raycastResult.Z < MainCommand.GridSize * 2)
                    AddSelector(raycastResult);
            }
            if (Input.IsActionJustPressed("F5") && MapEditorCommand.FieldSelector != null)
            {
                Node3D replaceNode;
                Node field = MainCommand.RootNode.GetNode("Field");
                var nodeForRemoving = field.GetNode($"{MainCommand.SUFFIX}{MapEditorCommand.FieldSelector.Position.X - 1}-{MapEditorCommand.FieldSelector.Position.Z - 1}");
                field.RemoveChild(nodeForRemoving);
                replaceNode = GD.Load<PackedScene>("res://prefabs/tiles/SandTile_Simple.tscn").Instantiate<Node3D>();
                replaceNode.Name = nodeForRemoving.Name;
                replaceNode.Position = new Vector3(MapEditorCommand.FieldSelector.Position.X - 1, 0, MapEditorCommand.FieldSelector.Position.Z - 1);
                field.AddChild(replaceNode);
            }
        }
    }
}
