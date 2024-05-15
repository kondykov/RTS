using Godot;
using RTS.Camera;
using RTS.Debug;
using RTS.Editor;
using System;
using Terrain;

namespace RTS
{
    public partial class MainCommandHandler : Node3D
    {
        private Vector3 _mousePosition;
        private Vector3 _selectorPosition = new Vector3(-1, -1, -1);
        private MapEditorCommandHandler _mapEditor = new MapEditorCommandHandler();
        private WorldCamera _camera = new WorldCamera();
        private bool _camPhysicMode = false;

        public void ClearSelector() => MainCommand.Mode = ControlModes.None;
        public void SelectTileEditor() => MainCommand.Mode = ControlModes.TileEditor;
        public void SelectObjectEditor() => MainCommand.Mode = ControlModes.ObjectEditor;

        public override void _Ready()
        {
            MainCommand.CurrentCamera = _camera;
            MainCommand.RootNode = GetTree().Root.GetChild<Node3D>(0);
            //Preloader.GetTiles("res://prefabs/tiles");
            //AddChild(GD.Load<PackedScene>("res://testFiles/terrain.tscn").Instantiate<Node3D>());
            //_mapEditor.GenerateSimpleField();
            HTerrainTest hTerrainTest = new HTerrainTest();
            hTerrainTest._Init();

        
        }
        public override void _Process(double delta)
        {
            DebugConsole.Print();
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