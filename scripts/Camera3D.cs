using Godot;
using System;

public partial class Camera3D : Godot.Camera3D
{
	[Export]
	public int Speed { get; set; } = 14;
	[Export]
	public int FallAcceleration { get; set; } = 75;

	private Vector3 _targetVelocity = Vector3.Zero;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	public override void _Process(double delta)
	{
	}
}
