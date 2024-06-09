using System;
using System.Collections.Generic;
using Godot;
using RTS.Debug;

namespace Terrain;

[Tool]
public partial class Chunk : StaticBody3D
{
    public static Vector3I dimensions = new(16, 64, 16);

    private static readonly Vector3I[] _verties =
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

    private static readonly int[] _top = { 2, 3, 7, 6 };
    private static readonly int[] _bottom = { 0, 4, 5, 1 };
    private static readonly int[] _left = { 6, 4, 0, 2 };
    private static readonly int[] _right = { 3, 1, 5, 7 };
    private static readonly int[] _back = { 7, 5, 4, 6 };
    private static readonly int[] _front = { 2, 0, 1, 3 };
    private Block[,,] _blocks = new Block[dimensions.X, dimensions.Y, dimensions.Z];
    private SurfaceTool _surfaceTool = new();
    [Export] public CollisionShape3D CollisionShape { get; set; }
    [Export] public MeshInstance3D MeshInstance { get; set; }
    [Export] public FastNoiseLite Noise { get; set; }

    public Vector2I ChunkPosition { get; private set; }

    public void SetChunkPosition(Vector2I position)
    {
        ChunkManager.Instance.UpdateChunkPosition(this, position, ChunkPosition);
        ChunkPosition = position;
        try
        {
            CallDeferred(Node3D.MethodName.SetGlobalPosition,
                new Vector3(ChunkPosition.X * dimensions.X, 0, ChunkPosition.Y * dimensions.Z));
        }
        catch (Exception e)
        {
            Console.WriteLine(StatusHandler.GetMessage(Status.WARNING_CHUNKMANAGER_THREAD_INTERRUPTED));
            return;
        }

        GenerateChunk();
        UpdateChunk();
    }

    private void GenerateChunk()
    {
        GenerateBlockInstances();
        var chunk = ChunkLoader.GetChunkOrNull(ChunkPosition);
        if(chunk == null) GenerateBlockInstances();
        else SetBlockInstances(chunk);
    }

    private void GenerateBlockInstances()
    {
        for (var y = 0; y < dimensions.Y; y++)
        for (var x = 0; x < dimensions.X; x++)
        for (var z = 0; z < dimensions.Z; z++)
        {
            Block block;
            var globalBlockPosition = ChunkPosition * new Vector2I(dimensions.X, dimensions.Z) + new Vector2(x, z);
            var groundHeight = GetHeightMap(globalBlockPosition);

            if (y < groundHeight / 2) block = BlockManager.Instance.Stone;
            else if (y < groundHeight) block = BlockManager.Instance.Dirt;
            else if (y == groundHeight) block = BlockManager.Instance.Grass;
            else block = BlockManager.Instance.Air;
            _blocks[x, y, z] = block;
        }
        ChunkLoader.AddCreatedChunk(ChunkPosition, this);
    }
    private void SetBlockInstances(Chunk chunk)
    {
        for (var y = 0; y < dimensions.Y; y++)
        for (var x = 0; x < dimensions.X; x++)
        for (var z = 0; z < dimensions.Z; z++)
            _blocks[x, y, z] = chunk._blocks[x, y, z];
        TestGetHeightMap(chunk.ChunkPosition);
    }

    private void TestGetHeightMap(Vector2 chunkPosition = new())
    {
        for (var x = 0; x < 16; x++)
        {
            for (var y = 0; y < 16; y++)
                Console.Write(
                    $"[{GetHeightMap(chunkPosition + new Vector2I(dimensions.X, dimensions.Z) * new Vector2(x, y))}]");
            Console.WriteLine();
        }
    }
    
    private void UpdateChunk()
    {
        _surfaceTool.Begin(Mesh.PrimitiveType.Triangles);
        for (var x = 0; x < dimensions.X; x++)
        for (var y = 0; y < dimensions.Y; y++)
        for (var z = 0; z < dimensions.Z; z++)
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
        if (blockPosition.X < 0 || blockPosition.X >= dimensions.X) return true;
        if (blockPosition.Y < 0 || blockPosition.Y >= dimensions.Y) return true;
        if (blockPosition.Z < 0 || blockPosition.Z >= dimensions.Z) return true;
        return _blocks[blockPosition.X, blockPosition.Y, blockPosition.Z] == BlockManager.Instance.Air;
    }

    //ToDo: Доделать проверку на прозрачность блоков на стыках чанков
    private bool CheckTransparentBlockChunks(Vector3I blockPosition)
    {
        if (blockPosition.X < 0 || blockPosition.X >= dimensions.X) return true;
        if (blockPosition.Y < 0 || blockPosition.Y >= dimensions.Y) return true;
        if (blockPosition.Z < 0 || blockPosition.Z >= dimensions.Z) return true;
        return _blocks[blockPosition.X, blockPosition.Y, blockPosition.Z] == BlockManager.Instance.Air;
    }


    public void SetBlock(Vector3I blockPosition, Block block)
    {
        if (blockPosition.Y < 0 || blockPosition.Y >= dimensions.Y) return;
        _blocks[blockPosition.X, blockPosition.Y, blockPosition.Z] = block;
        UpdateChunk();
        ChunkLoader.UpdateChunk(ChunkPosition, this);
    }

    private int GetHeightMap(Vector2 blockPosition)
    {
        return (int)(dimensions.Y * ((Noise.GetNoise2D(blockPosition.X, blockPosition.Y) + 1f) / 2f));
    }
}