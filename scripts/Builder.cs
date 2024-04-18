using Godot;
using System;

public partial class Builder : Node3D
{
    const float _TileSize = 1.0f;
    float index2 = 0;
    private void GenerateField()
    {
        var gridSize = 50;

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
                AddChild(node);
            }
        }
    }
    public override void _Ready()
    {
        GenerateField();
    }
    public override void _Process(double delta)
    {

    }
}
