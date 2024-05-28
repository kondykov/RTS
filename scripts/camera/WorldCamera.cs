using Godot;
using Godot.Collections;
using RTS.Debug;
using System;

namespace RTS.Camera
{
    public partial class WorldCamera : Camera3D
    {
        [Export] private int zoomSpeed = 100;
        private int _arrayLenth = 1000;
        public bool IsActive = true;
        public int ZoomSpeed { get => zoomSpeed; set => zoomSpeed = value; }
        public WorldCamera() { }
        public override void _Ready() { MainCommand.CurrentCamera = this; }
        public override void _Process(double delta)
        {
            if (Input.IsActionPressed("mouse_circle_up")) Size += ZoomSpeed * (float)delta;
            if (Input.IsActionPressed("mouse_circle_down")) Size -= ZoomSpeed * (float)delta;
            if (Input.IsActionJustPressed("mouse_circle_up")) Fov -= ZoomSpeed * (float)delta;
            if (Input.IsActionJustPressed("mouse_circle_down")) Fov += ZoomSpeed * (float)delta;
            Size = Math.Clamp(Size, 10, 300);
            Fov = Math.Clamp(Fov, 10, 110);
        }
        public Dictionary GetRaycast()
        {
            var camera = MainCommand.CurrentCamera;
            try
            {
                var mousePosition = MainCommand.RootNode.GetViewport().GetMousePosition();
                var from = camera.ProjectRayOrigin(mousePosition);
                var to = from + camera.ProjectRayNormal(mousePosition) * _arrayLenth;
                var space = MainCommand.RootNode.GetWorld3D().DirectSpaceState;
                var rayQuery = new PhysicsRayQueryParameters3D();
                rayQuery.From = from;
                rayQuery.To = to;
                var raycastResult = space.IntersectRay(rayQuery);
                try { Debug.DebugMenu.MousePosition = raycastResult["position"].AsVector3(); return raycastResult; }
                catch { DebugConsole.WriteMessage(FunctionStatus.WARNING_WORLD_CAMERA_RAYCAST_RETURNS_NULL); return null; }                
            }
            catch { DebugConsole.WriteMessage(FunctionStatus.ERR_FAILED_TO_GET_CAMERA_DATA); return null; }
        }
    }
}
