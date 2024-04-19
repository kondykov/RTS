using Godot;
using System;

public partial class Builder : Node3D
{
    const float _TileSize = 1.0f;
    float index2 = 0;
    static public int gridSize = 10;
    const string SUFFIX = "Tile_";
    private void GenerateField()
    {
        PackedScene tiles = GD.Load<PackedScene>("res://prefabs/tiles/GrassTile_Simple.tscn");
        var field = GetNode("Field");
        for (int i = 0; i < gridSize * 2; i += 2)
        {
            for (int j = 0; j < gridSize * 2; j += 2)
            {
                var newTile = tiles.Instantiate<Node3D>();

                Cell cell = new Cell();
                cell.Node = tiles.Instantiate<Node3D>();
                cell.Node.Name = $"{SUFFIX}{i}-{j}";
                cell.Node.Position = new Vector3(i, 0, j);

                field.AddChild(cell.Node);
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
