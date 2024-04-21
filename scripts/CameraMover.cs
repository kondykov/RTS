using Godot;
using System;

public partial class CameraMover : Godot.CharacterBody3D
{
    [Export] public int Speed = 20;
    public override void _Process(double delta)
	{
        var direction = Vector3.Zero;

        if (Input.IsActionPressed("E")) RotateY(0.05f);
        if (Input.IsActionPressed("Q")) RotateY(-0.05f);
        if (Input.IsActionPressed("camera_forward")) direction += Vector3.Forward;
        if (Input.IsActionPressed("camera_backward")) direction += Vector3.Back;
        if (Input.IsActionPressed("camera_left")) direction += Vector3.Left;
        if (Input.IsActionPressed("camera_right")) direction += Vector3.Right;
        if (Input.IsActionPressed("ui_page_up")) direction += Vector3.Up;
        if (Input.IsActionPressed("ui_page_down")) direction += Vector3.Down;
        if (Input.IsActionJustPressed("space")) // Return to start position
        {
            Position = new Vector3(0, 0, 0);
            Rotation = new Vector3(0, -2.35f, 0);
        }            
        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized();
            Translate(direction.Rotated(new Vector3(1, 0, 0), -Rotation.X) * Speed * (float)delta);
        }
        if (Input.IsActionPressed("camera_speed_max")) Speed++;
        if (Input.IsActionPressed("camera_speed_min")) Speed--;
        Speed = Math.Clamp(Speed, 10, 100);
        MoveAndSlide();
    }
}
