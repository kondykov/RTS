using Godot;
using System;

public partial class CharacterBody3D : Godot.CharacterBody3D
{
    [Export]
	public const float Speed = 100;
    [Export]
    public int FallAcceleration = 75;
    private Vector3 _targetVelocity = Vector3.Zero;
    public override void _PhysicsProcess(double delta)
	{
		var direction = Vector3.Zero;

        if (Input.IsActionPressed("camera_forward")) direction += Vector3.Forward;
        if (Input.IsActionPressed("camera_backward")) direction += Vector3.Back;
        if (Input.IsActionPressed("camera_left")) direction += Vector3.Left;
        if (Input.IsActionPressed("camera_right")) direction += Vector3.Right;
        if (Input.IsActionPressed("ui_page_up")) direction.Y += 1f;
        if (Input.IsActionPressed("ui_page_down")) direction += Vector3.Down;

        if (Input.IsActionJustPressed("space")) Position = new Vector3(0, 0, 0);

        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized();
            //GetNode<Node3D>("Camera3D").LookAt(Position + direction, Vector3.Up);
            //GetNode<Node3D>("Camera3D").LookAt(Position + direction);
        }

/*        _targetVelocity.X = direction.Rotated(new Vector3(1, 0, 0), -48).X * Speed;
        _targetVelocity.Y = direction.Rotated(new Vector3(1, 0, 0), -48).Y * Speed;
        _targetVelocity.Z = direction.Rotated(new Vector3(1, 0, 0), -48).Z * Speed;*/
        _targetVelocity = direction.Rotated(new Vector3(1, 0, 0), -48) * Speed * (float)delta;
        Console.WriteLine($"{direction.Rotated(new Vector3(1, 0, 0), -48), 30} | {direction} | {-Rotation.X}");
        //Translate(direction.Rotated(new Vector3(1, 0, 0), -Rotation.X) * Speed * (float)delta);

        if (!IsOnFloor()) _targetVelocity.Y -= FallAcceleration * (float)delta;
        Velocity = _targetVelocity;
        MoveAndSlide();
    }
}
