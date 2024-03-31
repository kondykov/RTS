using Godot;
using System;

public partial class Builder : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Console.WriteLine("Started");
		base._Ready();
        

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
