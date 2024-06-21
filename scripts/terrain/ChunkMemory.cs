using System.Collections.Concurrent;
using System.Collections.Generic;
using Godot;

namespace RTS.Terrain;

public abstract class ChunkMemory
{
    private static readonly ConcurrentDictionary<Vector2I, Block[,,]> Chunks = new();
    public static void AddCreatedChunk(Vector2I chunkPosition, Block[,,] chunk) => Chunks.TryAdd(chunkPosition, chunk);

    public static Block[,,] GetChunkOrNull(Vector2I chunkPosition) =>
        Chunks.GetValueOrDefault(chunkPosition);

    public static void UpdateChunk(Vector2I chunkPosition, Block[,,] chunk) => Chunks[chunkPosition] = chunk;
}