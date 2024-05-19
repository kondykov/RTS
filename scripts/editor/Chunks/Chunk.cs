using Godot;
using System;

[Tool]
public partial class Chunk : StaticBody3D
{
    [Export] public CollisionShape3D CollisionShape { get; set; }
    [Export] public MeshInstance3D MeshInstance { get; set; }
    public static Vector3I dimensions = new Vector3I(16, 64, 16);
    private static readonly Vector3I[] _verties = new Vector3I[]
    {
        new Vector3I(0,0,0),
        new Vector3I(1,0,0),
        new Vector3I(0,1,0),
        new Vector3I(1,1,0),
        new Vector3I(0,0,1),
        new Vector3I(1,0,1),
        new Vector3I(0,1,1),
        new Vector3I(1,1,1),
    };
    private static readonly int[] _top = new int[] { 2, 3, 7, 6 };
    private static readonly int[] _bottom = new int[] { 0, 4, 5, 1 };
    private static readonly int[] _left = new int[] { 6, 4, 0, 2 };
    private static readonly int[] _right = new int[] { 3, 1, 5, 7 };
    private static readonly int[] _back = new int[] { 7, 5, 4, 6 };
    private static readonly int[] _front = new int[] { 2, 0, 1, 3 };

    private SurfaceTool _surfaceTool = new();
    private Block[,,] _blocks = new Block[dimensions.X, dimensions.Y, dimensions.Z];
    public override void _Ready()
    {
        Generate();
        Update();
    }
    public void Generate()
    {
        for (int x = 0; x < dimensions.X; x++)
        {
            for (int y = 0; y < dimensions.Y; y++)
            {
                for (int z = 0; z < dimensions.Z; z++)
                {
                    try
                    {
                        Block block;
                        var groundHeight = 40;
                        if (y < groundHeight) block = BlockManager.Instance.Dirt;
                        else if (y == groundHeight) block = BlockManager.Instance.Grass;
                        else block = BlockManager.Instance.Air;
                        _blocks[x, y, z] = block;
                    }
                    catch (Exception e) 
                    {
                        Console.WriteLine($"FOR {x}x{y}x{z}, EXCEPTION {e}.");
                    }
                }
            }
        }
    }
    public void Update()
    {
        _surfaceTool.Begin(Mesh.PrimitiveType.Triangles);
        for (int x = 0; x < dimensions.X; x++)
        {
            for (int y = 0; y < dimensions.Y; y++)
            {
                for (int z = 0; z < dimensions.Z; z++)
                {
                    CreateBlockMesh(new Vector3I(x, y, z));
                }
            }
        }

        _surfaceTool.SetMaterial(BlockManager.Instance.ChunkMaterial);

        var mesh = _surfaceTool.Commit();
        MeshInstance.Mesh = mesh;
        CollisionShape.Shape = mesh.CreateTrimeshShape();
    }
    private void CreateBlockMesh(Vector3I blockPosition)
    {
        var block = _blocks[blockPosition.X, blockPosition.Y, blockPosition.Z];
        if (block == BlockManager.Instance.Air) return;

        if (CheckTransparent(blockPosition + Vector3I.Up)) CreateFaceMesh(_top, blockPosition, block.Texture);
        if (CheckTransparent(blockPosition + Vector3I.Down)) CreateFaceMesh(_bottom, blockPosition, block.Texture);
        if (CheckTransparent(blockPosition + Vector3I.Left)) CreateFaceMesh(_left, blockPosition, block.Texture);
        if (CheckTransparent(blockPosition + Vector3I.Right)) CreateFaceMesh(_right, blockPosition, block.Texture);
        if (CheckTransparent(blockPosition + Vector3I.Back)) CreateFaceMesh(_back, blockPosition, block.Texture);
        if (CheckTransparent(blockPosition + Vector3I.Forward)) CreateFaceMesh(_front, blockPosition, block.Texture);
    }
    private void CreateFaceMesh(int[] face, Vector3I blockPosition, Texture2D texture)
    {
        var texturePosition = BlockManager.Instance.GetTextureAtlasPosition(texture);
        var textureAtlasSize = BlockManager.Instance.TextureAtlasSize;
        var uvOffset = texturePosition / textureAtlasSize;
        var uvWidth = 1f / textureAtlasSize.X;
        var uvHeight = 1f / textureAtlasSize.Y;

        var uvA = uvOffset + new Vector2(0, 0);
        var uvB = uvOffset + new Vector2(0, uvHeight);
        var uvC = uvOffset + new Vector2(uvWidth, uvHeight);
        var uvD = uvOffset + new Vector2(uvWidth, 0);

        var a = _verties[face[0]] + blockPosition;
        var b = _verties[face[1]] + blockPosition;
        var c = _verties[face[2]] + blockPosition;
        var d = _verties[face[3]] + blockPosition;

        var uvTruagle1 = new Vector2[] { uvA, uvB, uvC };
        var uvTruagle2 = new Vector2[] { uvA, uvC, uvD };

        var triagle1 = new Vector3[] { a, b, c };
        var triagle2 = new Vector3[] { a, c, d };

        _surfaceTool.AddTriangleFan(triagle1, uvTruagle1);
        _surfaceTool.AddTriangleFan(triagle2, uvTruagle2);
            }
    private bool CheckTransparent(Vector3I blockPosition)
    {
        if (blockPosition.X < 0 || blockPosition.X >= dimensions.X) return true;
        if (blockPosition.Y < 0 || blockPosition.Y >= dimensions.Y) return true;
        if (blockPosition.Z < 0 || blockPosition.Z >= dimensions.Z) return true;
        return _blocks[blockPosition.X, blockPosition.Y, blockPosition.Z] == BlockManager.Instance.Air;
    }
}
