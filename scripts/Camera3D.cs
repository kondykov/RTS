using Godot;
using System;

public partial class Camera3D : Godot.Camera3D
{
    [Export] public int Speed = 100;
    [Export] public int ZoomSpeed = 100;
    bool repeat = true;

    public override void _Process(double delta)
    {
        var direction = Vector3.Zero;
        if (Input.IsActionPressed("camera_forward")) direction += Vector3.Forward;
        if (Input.IsActionPressed("camera_backward")) direction += Vector3.Back;
        if (Input.IsActionPressed("camera_left")) direction += Vector3.Left;
        if (Input.IsActionPressed("camera_right")) direction += Vector3.Right;

        if (direction != Vector3.Zero) Translate(direction.Rotated(new Vector3(1, 0, 0), -Rotation.X) * Speed * (float)delta);

        if (Input.IsActionPressed("zoom_in")) Size += ZoomSpeed * (float)delta;
        if (Input.IsActionPressed("zoom_out")) Size -= ZoomSpeed * (float)delta;

        if (Input.IsActionPressed("camera_speed_max")) Speed++;
        if (Input.IsActionPressed("camera_speed_min")) Speed--;
        Speed = Math.Clamp(Speed, 10, 100);
        Size = Math.Clamp(Size, 10, 100);

        var label = GetParent().GetNode<Label>("CanvasLayer/DebugContainer/DebugLabel");
        if (Input.IsActionJustPressed("show_debug_menu"))
            if (label.Visible) label.Visible = false;
            else label.Visible = true;

        label.Text = $"Zoom {Size}\n" +
            $"Speed: {Speed}\n" +
            $"Position:\n" +
            $"X: {Position.X + 30}\n" +
            $"Y: {Position.Y}\n" +
            $"Z: {Position.Z + 30}\n";

    }
}
