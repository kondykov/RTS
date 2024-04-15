using Godot;
using System;

public partial class CharacterBody3D : Godot.CharacterBody3D
{
    [Export]
	public const float Speed = 5.0f;
    [Export]
    public int FallAcceleration = 75;

    private Vector3 _targetVelocity = Vector3.Zero;
    bool ready = true;

	public override void _PhysicsProcess(double delta)
	{
		var direction = Vector3.Zero;

		if (Input.IsActionPressed("ui_right")) direction.X -= 1f;
		if (Input.IsActionPressed("ui_left")) direction.X += 1f;
		if (Input.IsActionPressed("ui_up")) direction.Z += 1f;
		if (Input.IsActionPressed("ui_down")) direction.Z -= 1f;
        if (Input.IsActionPressed("ui_page_up")) direction.Y += 1f;
        if (Input.IsActionPressed("ui_page_down")) direction.Y -= 1f;

        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized();
            //GetNode<Node3D>("Camera3D").LookAt(Position + direction, Vector3.Up);
            //GetNode<Node3D>("Camera3D").LookAt(Position + direction);
        }

        _targetVelocity.X = direction.X * Speed;
        _targetVelocity.Z = direction.Z * Speed;
        _targetVelocity.Y = direction.Y * Speed;

        //if (!IsOnFloor()) _targetVelocity.Y -= FallAcceleration * (float)delta;
        Velocity = _targetVelocity;
        MoveAndSlide();
    }
}
