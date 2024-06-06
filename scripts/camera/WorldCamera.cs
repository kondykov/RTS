using System;
using Godot;
using Godot.Collections;
using RTS.Debug;

namespace RTS.Camera;

public partial class WorldCamera : Camera3D
{
    private int _arrayLength = 1000;
    [Export] public WorldCamera Camera;
    [Export] private int _zoomSpeed = 100;

    public int ZoomSpeed
    {
        get => _zoomSpeed;
        set => _zoomSpeed = value;
    }


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
        try
        {
            var mousePosition = MainCommand.RootNode.GetViewport().GetMousePosition();
            var from = Camera.ProjectRayOrigin(mousePosition);
            var to = from + Camera.ProjectRayNormal(mousePosition) * _arrayLength;
            var space = MainCommand.RootNode.GetWorld3D().DirectSpaceState;
            var rayQuery = new PhysicsRayQueryParameters3D();
            rayQuery.From = from;
            rayQuery.To = to;
            var raycastResult = space.IntersectRay(rayQuery);
            try
            {
                DebugMenu.MousePosition = raycastResult["position"].AsVector3();
                return raycastResult;
            }
            catch
            {
                DebugConsole.WriteMessage(Status.WARNING_RAYCAST_RETURNS_NULL);
                return null;
            }
        }
        catch
        {
            DebugConsole.WriteMessage(Status.ERR_FAILED_TO_GET_CAMERA_DATA);
            return null;
        }
    }
}