using Godot;
using System;

public partial class Builder : Node3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		GenerateField();
	}


	const float _TileSize = 1.0f;
    private void GenerateField()
    {
		var tileIndex = 0;
		var gridSize = 10;

		PackedScene tiles = GD.Load<PackedScene>("res://models/meshLibrary.tscn");
		AddChild(tiles.Get("GrassCell Simple").As<Node3D>());

		var node = tiles.Instantiate<Node3D>();
		var vector = new Vector3(10, 0, 0);
		var node2 = tiles.Instantiate();
		
		node.Name = "NewTile1";
		AddChild(node);
		node.GlobalPosition = new Vector3(2, 0, 0);
		node2.Name = "NewTile2";
		Console.Clear();
		AddChild(node2);
		
		//node = Transform.Basis.X.Rotated(new Vector3(10, 5, 0), 0);
		foreach (var child in GetChildren())
        {
			Console.WriteLine();
        }
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}
