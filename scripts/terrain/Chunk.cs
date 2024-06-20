using System;
using Godot;
using LoggerService;
using RTS.Debug;

namespace RTS.Terrain;

[Tool]
public partial class Chunk : StaticBody3D
{
    public static Vector3I Dimensions = new(16, 64, 16);

    private static readonly Vector3I[] Verties =
    {
        new(0, 0, 0),
        new(1, 0, 0),
        new(0, 1, 0),
        new(1, 1, 0),
        new(0, 0, 1),
        new(1, 0, 1),
        new(0, 1, 1),
        new(1, 1, 1)
    };

    private static readonly int[] Top = { 2, 3, 7, 6 };
    private static readonly int[] Bottom = { 0, 4, 5, 1 };
    private static readonly int[] Left = { 6, 4, 0, 2 };
    private static readonly int[] Right = { 3, 1, 5, 7 };
    private static readonly int[] Back = { 7, 5, 4, 6 };
    private static readonly int[] Front = { 2, 0, 1, 3 };
    private readonly Block[,,] _blocks = new Block[Dimensions.X, Dimensions.Y, Dimensions.Z];
    private SurfaceTool _surfaceTool = new();
    [Export] public CollisionShape3D CollisionShape { get; set; }
    [Export] public MeshInstance3D MeshInstance { get; set; }
    [Export] public FastNoiseLite Noise { get; set; }
    [Export] public Label3D DebugLabel3D { get; set; }

    private Vector2I ChunkPosition { get; set; }

    public void SetChunkPosition(Vector2I position)
    {
        ChunkManager.Instance.UpdateChunkPosition(this, position, ChunkPosition);
        ChunkPosition = position;
        try
        {
            CallDeferred(Node3D.MethodName.SetGlobalPosition,
                new Vector3(ChunkPosition.X * Dimensions.X, 0, ChunkPosition.Y * Dimensions.Z));
        }
        catch (Exception e)
        {
            Console.WriteLine(StatusHandler.GetMessage(Status.WarningChunkmanagerThreadInterrupted));
            return;
        }

        GenerateChunk();
        UpdateChunk();
    }


    private void GenerateChunk()
    {
        var blocks = ChunkMemory.GetChunkOrNull(GlobalPosition);
        if (blocks != null)
        {
            Console.WriteLine($"Loaded {ChunkPosition}.");
            Logger<string> logger = new(new FileService());
            logger.Log(LogStatus.Ok, $"Loaded chunk {ChunkPosition.ToString()}");

            for (var y = 0; y < Dimensions.Y; y++)
            for (var x = 0; x < Dimensions.X; x++)
            for (var z = 0; z < Dimensions.Z; z++)
                _blocks[x, y, z] = blocks[x, y, z];

            DebugLabel3D.Text = ChunkPosition.ToString();
        }
        else
        {
            Console.WriteLine($"Generated {ChunkPosition}.");
            Logger<string> logger = new(new FileService());
            logger.Log(LogStatus.Ok, $"Generated chunk {ChunkPosition.ToString()}");

            for (var y = 0; y < Dimensions.Y; y++)
            for (var x = 0; x < Dimensions.X; x++)
            for (var z = 0; z < Dimensions.Z; z++)
            {
                Block block;
                var globalBlockPosition = ChunkPosition * new Vector2I(Dimensions.X, Dimensions.Z) + new Vector2(x, z);
                var groundHeight = GetHeightMap(globalBlockPosition);

                if (y < groundHeight / 2) block = BlockManager.Instance.Stone;
                else if (y < groundHeight) block = BlockManager.Instance.Dirt;
                else if (y == groundHeight) block = BlockManager.Instance.Grass;
                else block = BlockManager.Instance.Air;
                _blocks[x, y, z] = block;
            }
            DebugLabel3D.Text = ChunkPosition.ToString();
            ChunkMemory.AddCreatedChunk(GlobalPosition, _blocks);
        }
    }

    private void UpdateChunk()
    {
        ChunkMemory.UpdateChunk(GlobalPosition, _blocks);
        _surfaceTool.Begin(Mesh.PrimitiveType.Triangles);
        for (var x = 0; x < Dimensions.X; x++)
        for (var y = 0; y < Dimensions.Y; y++)
        for (var z = 0; z < Dimensions.Z; z++)
            CreateBlockMesh(new Vector3I(x, y, z));

        _surfaceTool.SetMaterial(BlockManager.Instance.ChunkMaterial);

        var mesh = _surfaceTool.Commit();
        MeshInstance.Mesh = mesh;
        CollisionShape.Shape = mesh.CreateTrimeshShape();
    }

    private void CreateBlockMesh(Vector3I blockPosition)
    {
        var block = _blocks[blockPosition.X, blockPosition.Y, blockPosition.Z];
        if (block == BlockManager.Instance.Air) return;
        if (CheckTransparent(blockPosition + Vector3I.Up)) CreateFaceMesh(Top, blockPosition, block.Texture);
        if (CheckTransparent(blockPosition + Vector3I.Down)) CreateFaceMesh(Bottom, blockPosition, block.Texture);
        if (CheckTransparent(blockPosition + Vector3I.Left)) CreateFaceMesh(Left, blockPosition, block.Texture);
        if (CheckTransparent(blockPosition + Vector3I.Right)) CreateFaceMesh(Right, blockPosition, block.Texture);
        if (CheckTransparent(blockPosition + Vector3I.Back)) CreateFaceMesh(Back, blockPosition, block.Texture);
        if (CheckTransparent(blockPosition + Vector3I.Forward)) CreateFaceMesh(Front, blockPosition, block.Texture);
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

        var a = Verties[face[0]] + blockPosition;
        var b = Verties[face[1]] + blockPosition;
        var c = Verties[face[2]] + blockPosition;
        var d = Verties[face[3]] + blockPosition;

        var uvTriagle1 = new[] { uvA, uvB, uvC };
        var uvTriagle2 = new[] { uvA, uvC, uvD };

        var triagle1 = new Vector3[] { a, b, c };
        var triagle2 = new Vector3[] { a, c, d };

        var normal = ((Vector3)(c - a)).Cross(b - a).Normalized();
        var normals = new[] { normal, normal, normal };

        _surfaceTool.AddTriangleFan(triagle1, uvTriagle1, normals: normals);
        _surfaceTool.AddTriangleFan(triagle2, uvTriagle2, normals: normals);
    }

    private bool CheckTransparent(Vector3I blockPosition)
    {
        if (blockPosition.X < 0 || blockPosition.X >= Dimensions.X) return true;
        if (blockPosition.Y < 0 || blockPosition.Y >= Dimensions.Y) return true;
        if (blockPosition.Z < 0 || blockPosition.Z >= Dimensions.Z) return true;
        return _blocks[blockPosition.X, blockPosition.Y, blockPosition.Z] == BlockManager.Instance.Air;
    }

    //ToDo: Доделать проверку на прозрачность блоков на стыках чанков
    private bool CheckTransparentBlockChunks(Vector3I blockPosition)
    {
        if (blockPosition.X < 0 || blockPosition.X >= Dimensions.X) return true;
        if (blockPosition.Y < 0 || blockPosition.Y >= Dimensions.Y) return true;
        if (blockPosition.Z < 0 || blockPosition.Z >= Dimensions.Z) return true;
        return _blocks[blockPosition.X, blockPosition.Y, blockPosition.Z] == BlockManager.Instance.Air;
    }


    public void SetBlock(Vector3I blockPosition, Block block)
    {
        if (blockPosition.Y < 0 || blockPosition.Y >= Dimensions.Y) return;
        _blocks[blockPosition.X, blockPosition.Y, blockPosition.Z] = block;
        UpdateChunk();
    }

    public Block GetBlock(Vector3I blockPosition)
    {
        return _blocks[blockPosition.X, blockPosition.Y, blockPosition.Z];
    }

    private int GetHeightMap(Vector2 blockPosition)
    {
        return (int)(Dimensions.Y * ((Noise.GetNoise2D(blockPosition.X, blockPosition.Y) + 1f) / 2f));
    }

    public void SetNewBlocks()
    {
        for (var y = 0; y < Dimensions.Y; y++)
        for (var x = 0; x < Dimensions.X; x++)
        for (var z = 0; z < Dimensions.Z; z++)
        {
            Block block;
            Random rnd = new Random();
            int groundHeight = rnd.Next();

            if (y < groundHeight / 2) block = BlockManager.Instance.Stone;
            else if (y < groundHeight) block = BlockManager.Instance.Dirt;
            else if (y == groundHeight) block = BlockManager.Instance.Grass;
            else block = BlockManager.Instance.Air;
            _blocks[x, y, z] = block;
        }
        UpdateChunk();
    }
}