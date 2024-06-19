using System.Collections.Concurrent;
using System.Collections.Generic;
using Godot;

namespace Terrain;

public class ChunkMemory
{
    private static ConcurrentDictionary<Vector2I, Block[,,]> _chunks = new();
    public static void AddCreatedChunk(Vector2I chunkPosition, Block[,,] chunk) => _chunks.TryAdd(chunkPosition, chunk);

    public static Block[,,] GetChunkOrNull(Vector2I chunkPosition) =>
        _chunks.GetValueOrDefault(chunkPosition);

    public static void UpdateChunk(Vector2I chunkPosition, Block[,,] chunk) => _chunks[chunkPosition] = chunk;
}