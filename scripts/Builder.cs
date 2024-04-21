using Godot;
using Godot.Collections;
using System;

public partial class Builder : Node3D
{
    static public int GridSize = 20;
    const string SUFFIX = "Tile_";
    int ArrayLenth = 1000;
    private Vector3 MousePosition;
    private Vector3 SelectorPosition = new Vector3(-1, -1, -1);

    public override void _Ready()
    {
        GenerateSimpleField();
    }
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ESC"))
            if (GetNodeOrNull("3DCursor") != null)
                RemoveSelector();
        if (Input.IsActionJustPressed("mouse_left_click"))
        {
            var raycastResult = Get3DMousePositon();
            for (int i = 0; i < GridSize * 2; i += 2)
            {
                for (int j = 0; j < GridSize * 2; j += 2)
                {
                    if (raycastResult.X >= i && raycastResult.X < GridSize * 2 && raycastResult.Z >= j && raycastResult.Z < GridSize * 2)
                    {
                        PackedScene tiles = GD.Load<PackedScene>("res://prefabs/ui/Selector.tscn");
                        Node3D node = tiles.Instantiate<Node3D>();
                        var vec = Vector3FloatToInt(raycastResult);
                        if ((int)vec.X % 2 == 0) vec.X += 1;
                        if ((int)vec.Z % 2 == 0) vec.Z += 1;
                        vec.Y = 1;
                        node.Position = vec;
                        node.Name = "3DCursor";
                        if (GetNodeOrNull("3DCursor") != null) RemoveSelector();
                        SelectorPosition = node.Position;
                        AddChild(node);
                    }
                }    
            }
        }
        if (Input.IsActionJustPressed("F5") && SelectorPosition != new Vector3(-1, -1, -1))
        {
            Node3D node;
            Console.WriteLine(GetNode($"Field/{SUFFIX}{SelectorPosition.X - 1}-{SelectorPosition.Z - 1}"));
            Console.WriteLine();
            Node field = GetNode("Field");
            var nodeForRemoving = field.GetNode($"{SUFFIX}{SelectorPosition.X - 1}-{SelectorPosition.Z - 1}");
            field.RemoveChild(nodeForRemoving);
            node = GD.Load<PackedScene>("res://prefabs/tiles/SandTile_Simple.tscn").Instantiate<Node3D>();
            node.Position = new Vector3(SelectorPosition.X, 0, SelectorPosition.Z);
            AddChild(node);
        }
    }
    private void RemoveSelector()
    {
        SelectorPosition = new Vector3(-1, -1, -1);
        RemoveChild(GetNode("3DCursor"));
    }
    public Vector3 Get3DMousePositon()
    {
        var camera = GetNode<Camera3D>("RTSCameraBody/RTSCamera");
        var mousePosition = GetViewport().GetMousePosition();
        var from = camera.ProjectRayOrigin(mousePosition);
        var to = from + camera.ProjectRayNormal(mousePosition) * ArrayLenth;
        var space = GetWorld3D().DirectSpaceState;
        var rayQuery = new PhysicsRayQueryParameters3D();
        rayQuery.From = from;
        rayQuery.To = to;
        var raycastResult = space.IntersectRay(rayQuery);
        DebugMenu.MousePosition = raycastResult["position"].AsVector3();
        return raycastResult["position"].AsVector3();
    }
    private Vector3 Vector3FloatToInt(Vector3 vector)
    {
        Vector3 newVector = new Vector3();
        newVector.X = (int)vector.X;
        newVector.Y = (int)vector.Y;
        newVector.Z = (int)vector.Z;
        return newVector;
    }
    private void GenerateSimpleField()
    {
        PackedScene tiles = GD.Load<PackedScene>("res://prefabs/tiles/GrassTile_Simple.tscn");
        var field = GetNode("Field");
        for (int i = 0; i < GridSize * 2; i += 2)
        {
            for (int j = 0; j < GridSize * 2; j += 2)
            {
                var newTile = tiles.Instantiate<Node3D>();

                Cell cell = new Cell();
                cell.Node = tiles.Instantiate<Node3D>();
                cell.Node.Name = $"{SUFFIX}{i}-{j}";
                cell.Node.Position = new Vector3(i, 0, j);
                Cell.AppendCell(cell);

                field.AddChild(cell.Node);
            }
        }
    }
}
