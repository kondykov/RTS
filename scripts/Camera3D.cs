using Godot;
using Godot.Collections;
using System;

public partial class Camera3D : Godot.Camera3D
{
    [Export] private int zoomSpeed = 100;
    public int ZoomSpeed { get => zoomSpeed; set => zoomSpeed = value; }

    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("mouse_circle_up")) Size += ZoomSpeed * (float)delta;
        if (Input.IsActionPressed("mouse_circle_down")) Size -= ZoomSpeed * (float)delta;
        if (Input.IsActionJustPressed("mouse_circle_up")) Fov -= ZoomSpeed * (float)delta;
        if (Input.IsActionJustPressed("mouse_circle_down")) Fov += ZoomSpeed * (float)delta;
        Size = Math.Clamp(Size, 10, 300);
        Fov = Math.Clamp(Fov, 10, 110);
    }
}
