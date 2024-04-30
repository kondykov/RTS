using Godot;
using Godot.Collections;
using System;

namespace RTS.Camera
{
    public partial class WorldCamera : Camera3D
    {
        [Export] private int zoomSpeed = 100;
        private int _arrayLenth = 1000;
        public int ZoomSpeed { get => zoomSpeed; set => zoomSpeed = value; }
        public WorldCamera() { }
        public override void _Process(double delta)
        {
            if (Input.IsActionPressed("mouse_circle_up")) Size += ZoomSpeed * (float)delta;
            if (Input.IsActionPressed("mouse_circle_down")) Size -= ZoomSpeed * (float)delta;
            if (Input.IsActionJustPressed("mouse_circle_up")) Fov -= ZoomSpeed * (float)delta;
            if (Input.IsActionJustPressed("mouse_circle_down")) Fov += ZoomSpeed * (float)delta;
            Size = Math.Clamp(Size, 10, 300);
            Fov = Math.Clamp(Fov, 10, 110);
        }
        public Vector3 GetRaycast()
        {
            var camera = MainCommand.RootNode.GetNode<Camera3D>("RTSCameraBody/RTSCamera");
            var mousePosition = MainCommand.RootNode.GetViewport().GetMousePosition();
            var from = camera.ProjectRayOrigin(mousePosition);
            var to = from + camera.ProjectRayNormal(mousePosition) * _arrayLenth;
            var space = MainCommand.RootNode.GetWorld3D().DirectSpaceState;
            var rayQuery = new PhysicsRayQueryParameters3D();
            rayQuery.From = from;
            rayQuery.To = to;
            var raycastResult = space.IntersectRay(rayQuery);
            DebugMenu.MousePosition = raycastResult["position"].AsVector3();
            return raycastResult["position"].AsVector3();
        }
    }
}
