using Godot;
using System;

public partial class Builder : Node3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Console.Clear();
		Console.WriteLine("Started");
		base._Ready();
		
		GenerateField();
    }

	const float TILESIZE = 1.0f;

	private void GenerateField()
	{
		Console.WriteLine("Generating field!");
		var tileIndex = 0;
		var gridSize = 10;


		for (int x = 0; x < gridSize; x++)
		{
			var tileCoords = Vector2.Zero;
			tileCoords.X = x * TILESIZE * Mathf.Cos(Mathf.DegToRad(30));
			tileCoords.Y = x % 2 == 0 ? 0 : TILESIZE / 2;
			for (int y = 0; y < gridSize; y++)
			{
				
                tileIndex++;
			}
		}
		Console.WriteLine("Loading mesh.");
		PackedScene tiles = GD.Load<PackedScene>("res://models/meshLibrary.tscn");

		tiles.Get("GrassCell Simple");
		Node node = new Node();

		node = tiles.Instantiate();

		Console.WriteLine("Mesh loaded.");
		Console.WriteLine("Adding child node.");

		Console.WriteLine(node.GetChild(0).Name);
		Console.WriteLine();

        foreach (var item in node.GetChildren())
        {
			Console.WriteLine(item.Name);
        }
		Console.WriteLine();

		AddChild(node);

        foreach (var item in GetChildren())
        {
			Console.WriteLine($"GET CHILDREN: {item.Name}");
        }

		var n3d = GetNode("Node3D");
		

		Console.WriteLine("Added child node.");


		Console.WriteLine("Generating field completed!");
	}

	private Node GetNodeFromScene() => null;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
