using Godot;
using Godot.Collections;
using RTS.Camera;
using RTS.Editor;
using RTS.Loaders;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace RTS
{
    public partial class MainCommandHandler : Node3D
    {
        private Vector3 _mousePosition;
        private Vector3 _selectorPosition = new Vector3(-1, -1, -1);
        private MapEditorCommandHandler _mapEditor = new MapEditorCommandHandler();
        private WorldCamera _camera = new WorldCamera();
        public void ClearSelector() => MainCommand.Mode = ControlModes.None;
        public void SelectTileEditor() => MainCommand.Mode = ControlModes.TileEditor;
        public void SelectObjectEditor() => MainCommand.Mode = ControlModes.ObjectEditor;

        public override void _Ready()
        {
            MainCommand.RootNode = GetTree().Root.GetChild<Node3D>(0);
            _mapEditor.GenerateSimpleField();
            Preloader.GetFileNamesFromFolder("res://prefabs/tiles");
        }
        public override void _Process(double delta)
        {            
            switch (MainCommand.Mode) 
            { 
                case ControlModes.TileEditor:
                    _mapEditor.EditorHandler(_camera);
                    break;
                default: 
                    _mapEditor.RemoveSelector();
                    break;
            }
        }
        public static Vector3 Vector3FloatToInt(Vector3 vector)
        {
            Vector3 newVector = new Vector3();
            newVector.X = (int)vector.X;
            newVector.Y = (int)vector.Y;
            newVector.Z = (int)vector.Z;
            return newVector;
        }

    }
}
