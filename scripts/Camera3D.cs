using Godot;
using System;

public partial class Camera3D : Godot.Camera3D
{
    [Export] private int zoomSpeed = 100;
    public int ZoomSpeed { get => zoomSpeed; set => zoomSpeed = value; }

    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("zoom_in")) Size += ZoomSpeed * (float)delta;
        if (Input.IsActionPressed("zoom_out")) Size -= ZoomSpeed * (float)delta;
        Size = Math.Clamp(Size, 10, 100);
    }
}
