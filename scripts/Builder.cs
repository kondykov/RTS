using Godot;
using System;

public partial class Builder : Node3D
{
    const float _TileSize = 1.0f;
    float index2 = 0;
    private void GenerateField()
    {
        var tileIndex = 0;
        var gridSize = 20;
        int index = 0;

        PackedScene tiles = GD.Load<PackedScene>("res://prefabs/tiles/GrassTile_Simple.tscn");
        for (int i = 0; i < gridSize * 2; i += 2)
        {
            for (int j = 0; j < gridSize * 2; j += 2)
            {
                var newTile = tiles.Instantiate<Node3D>();
                Node3D node = new Node3D();
                node = tiles.Instantiate<Node3D>();
                node.Name = $"Tile_{i}-{j}";

                node.Position = new Vector3(i, 0, j);
                Console.WriteLine(node.Position);

                AddChild(node);
            }
        }
        var node2 = GetNode<Node3D>("Tile_2-2");
        node2.Position = new Vector3(3, 2, 1);
        Console.WriteLine($"{node2.Name}");
    }
    public override void _Ready()
    {
        GenerateField();
    }
    public override void _Process(double delta)
    {
        var node2 = GetNode<Node3D>("Tile_2-2");
        var node3 = GetNode<Node3D>("Tile_10-2");
        node2.Position = new Vector3(2, index2 += 0.1f, 2);
        node3.Position = new Vector3(3, 5, 1);
    }
}
