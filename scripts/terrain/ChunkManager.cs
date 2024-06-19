using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Godot;
using RTS.Debug;
using RTS.Terrain.Character;

namespace Terrain;

[Tool]
public partial class ChunkManager : Node
{
    private readonly Dictionary<Chunk, Vector2I> _chunkToPosition = new();
    private readonly object _playerPositionLock = new();
    private readonly Dictionary<Vector2I, Chunk> _positionToChunk = new();
    private List<Chunk> _chunks;
    private Vector3 _playerPosition;
    public readonly int _renderDistance = 20;
    [Export] public bool MovementChunkRender = true;
    [Export] public PackedScene ChunkScene { get; set; }
    public static ChunkManager Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
        _chunks = GetParent().GetChildren().OfType<Chunk>().ToList();
        for (var i = _chunks.Count; i < _renderDistance * _renderDistance; i++)
        {
            var chunk = ChunkScene.Instantiate<Chunk>();
            GetParent().CallDeferred(Node.MethodName.AddChild, chunk);
            _chunks.Add(chunk);
        }

        for (var x = 0; x < _renderDistance; x++)
        for (var y = 0; y < _renderDistance; y++)
        {
            var index = y * _renderDistance + x;
            var halfwidth = Mathf.FloorToInt(_renderDistance / 2);
            _chunks[index].SetChunkPosition(new Vector2I(x - halfwidth, y - halfwidth));
        }

        if (!Engine.IsEditorHint() && MovementChunkRender) new Thread(ThreadProcess).Start();
    }

    public void UpdateChunkPosition(Chunk chunk, Vector2I currentPosition, Vector2I previousPosition)
    {
        if (_positionToChunk.TryGetValue(previousPosition, out var chunkAtPosition) && chunkAtPosition == chunk)
            _positionToChunk.Remove(previousPosition);

        _chunkToPosition[chunk] = currentPosition;
        _positionToChunk[currentPosition] = chunk;
    }

    public void SetBlock(Vector3I globalPosition, Block block)
    {
        var chunkTilePosition = new Vector2I(Mathf.FloorToInt(globalPosition.X / (float)Chunk.dimensions.X),
            Mathf.FloorToInt(globalPosition.Z / (float)Chunk.dimensions.Z));
        lock (_chunkToPosition)
        {
            if (_positionToChunk.TryGetValue(chunkTilePosition, out var chunk))
                chunk.SetBlock((Vector3I)(globalPosition - chunk.GlobalPosition), block);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Engine.IsEditorHint()) return;
        lock (_playerPositionLock)
        {
            _playerPosition = Player.Instance.GlobalPosition;
        }
    }

    private void ThreadProcess()
    {
        while (IsInstanceValid(this))
        {
            int playerChunkX, playerChunkZ;
            lock (_playerPositionLock)
            {
                playerChunkX = Mathf.FloorToInt(_playerPosition.X / Chunk.dimensions.X);
                playerChunkZ = Mathf.FloorToInt(_playerPosition.Z / Chunk.dimensions.Z);
            }

            foreach (var chunk in _chunks)
            {
                var chunkPosition = _chunkToPosition[chunk];

                var chunkX = chunkPosition.X;
                var chunkZ = chunkPosition.Y;

                var newChunkX = Mathf.PosMod(chunkX - playerChunkX + _renderDistance / 2, _renderDistance) +
                    playerChunkX - _renderDistance / 2;
                var newChunkZ = Mathf.PosMod(chunkZ - playerChunkZ + _renderDistance / 2, _renderDistance) +
                    playerChunkZ - _renderDistance / 2;

                if (newChunkX != chunkX || newChunkZ != chunkZ)
                    lock (_positionToChunk)
                    {
                        _positionToChunk.Remove(chunkPosition);
                        var newPosition = new Vector2I(newChunkX, newChunkZ);
                        _chunkToPosition[chunk] = newPosition;
                        _positionToChunk[newPosition] = chunk;
                        try
                        {
                            chunk.CallDeferred(nameof(Chunk.SetChunkPosition), newPosition);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(
                                $"{StatusHandler.GetMessage(Status.WarningChunkmanagerThreadInterrupted)}");
                            return;
                        }
                    }

                Thread.Sleep(10);
            }
        }
    }
}