using Godot;
using RTS.Camera;

namespace RTS
{
    public partial class MainCommandHandler : Node3D
    {
        private Vector3 _mousePosition;
        private Vector3 _selectorPosition = new Vector3(-1, -1, -1);
        private WorldCamera _camera = new WorldCamera();
        private bool _camPhysicMode = false;

        public void ClearSelector() => MainCommand.Mode = ControlModes.None;
        public void SelectTileEditor() => MainCommand.Mode = ControlModes.TileEditor;
        public void SelectObjectEditor() => MainCommand.Mode = ControlModes.ObjectEditor;

        public override void _Ready()
        {
            _Init();
        }
        private void _Init()
        {
            try
            {
                MainCommand.CurrentCamera = _camera;
                MainCommand.RootNode = GetTree().Root.GetChild<Node3D>(0);
            }
            catch
            {
                throw;
            }
        }
        public override void _Process(double delta)
        {
            switch (MainCommand.Mode)
            {
                case ControlModes.TileEditor:
                    break;
                default:
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
