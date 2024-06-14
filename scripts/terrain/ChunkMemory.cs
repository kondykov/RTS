using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Terrain;

public class ChunkMemory
{
    private static ConcurrentDictionary<Vector2I, Chunk> _chunks = new();
    public static void AddCreatedChunk(Vector2I chunkPosition, Chunk chunk) => _chunks.TryAdd(chunkPosition, chunk);

    public static Chunk GetChunkOrNull(Vector2I chunkPosition) =>
        _chunks.GetValueOrDefault(chunkPosition);

    public static void UpdateChunk(Vector2I chunkPosition, Chunk chunk) => _chunks[chunkPosition] = chunk;
}