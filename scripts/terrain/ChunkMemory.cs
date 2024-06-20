using System.Collections.Concurrent;
using System.Collections.Generic;
using Godot;

namespace RTS.Terrain;

public abstract class ChunkMemory
{
    private static readonly ConcurrentDictionary<Vector3, Block[,,]> Chunks = new();

    public static void AddCreatedChunk(Vector3 chunkPosition, Block[,,] chunk)
    {
        Chunks.TryAdd(chunkPosition, chunk);
    }

    public static Block[,,] GetChunkOrNull(Vector3 chunkPosition)
    {
        return Chunks.GetValueOrDefault(chunkPosition);
    }

    public static void UpdateChunk(Vector3 chunkPosition, Block[,,] chunk)
    {
        Chunks[chunkPosition] = chunk;
    }
}