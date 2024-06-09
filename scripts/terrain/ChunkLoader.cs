using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Terrain;

public partial class ChunkLoader
{
    private static readonly string _chunkLoaderVersion = "0.1";
    public static ConcurrentDictionary<Vector2I, Chunk> Chunks = new();

    public async static void AddCreatedChunk(Vector2I chunkPosition, Chunk chunk) => Chunks.TryAdd(chunkPosition, chunk);

    public static Chunk GetChunkOrNull(Vector2I chunkPosition) => Chunks.TryGetValue(chunkPosition, out Chunk chunk) ? chunk : null;

    public static void UpdateChunk(Vector2I chunkPosition, Chunk chunk)
    {
        Chunks[chunkPosition] = chunk;
    }
}