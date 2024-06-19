using Godot;
using System;

namespace RTS.Camera;

public partial class FreeCamera : Camera3D
{
    [Export] public Camera3D Camera { get; set; }
    [Export] public int Speed = 100;
    [Export] public int ZoomSpeed = 100;

    public override void _Process(double delta)
    {
            var direction = Vector3.Zero;

            if (Input.IsActionJustPressed("mouse_circle_up")) Fov -= ZoomSpeed * (float)delta;
            if (Input.IsActionJustPressed("mouse_circle_down")) Fov += ZoomSpeed * (float)delta;

            if (Input.IsActionPressed("E")) RotateY(0.05f);
            if (Input.IsActionPressed("Q")) RotateY(-0.05f);
            if (Input.IsActionPressed("W")) direction += Vector3.Forward;
            if (Input.IsActionPressed("S")) direction += Vector3.Back;
            if (Input.IsActionPressed("A")) direction += Vector3.Left;
            if (Input.IsActionPressed("D")) direction += Vector3.Right;
            if (Input.IsActionPressed("ui_page_up")) direction += Vector3.Up;
            if (Input.IsActionPressed("ui_page_down")) direction += Vector3.Down;
            if (Input.IsActionJustPressed("space"))
            {
                Position = new Vector3(0, 0, 0);
                Rotation = new Vector3(0, -2.35f, 0);
            }

            if (direction != Vector3.Zero)
            {
                Translate(direction.Rotated(new Vector3(1, 0, 0), -Rotation.X) * Speed * (float)delta);
            }

            Size = Math.Clamp(Size, 10, 100);
        }
}