using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Godot;
using RTS.Debug;
using RTS.Terrain.Character;

namespace RTS.Terrain;

[Tool]
public partial class ChunkManager : Node
{
    private readonly Dictionary<Chunk, Vector2I> _chunkToPosition = new();
    private readonly object _playerPositionLock = new();
    private readonly Dictionary<Vector2I, Chunk> _positionToChunk = new();
    private readonly int _renderDistance = 5;
    private List<Chunk> _visibleChunks;
    public Dictionary<Vector2I, Chunk> Chunks { get; set; } = new();
    private Vector3 _playerPosition;
    private bool _toUpdate = true;
    [Export] public bool MovementChunkRender = true;
    [Export] public PackedScene ChunkScene { get; private set; }
    public static ChunkManager Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
        _visibleChunks = GetParent().GetChildren().OfType<Chunk>().ToList();
        for (var i = _visibleChunks.Count; i < _renderDistance * _renderDistance; i++)
        {
            var chunk = ChunkScene.Instantiate<Chunk>();
            GetParent().CallDeferred(Node.MethodName.AddChild, chunk);
            _visibleChunks.Add(chunk);
        }

        for (var x = 0; x < _renderDistance; x++)
        for (var y = 0; y < _renderDistance; y++)
        {
            var index = y * _renderDistance + x;
            var halfwidth = Mathf.FloorToInt(_renderDistance / 2);
            _visibleChunks[index].SetChunkPosition(new Vector2I(x - halfwidth, y - halfwidth));
        }

        if (!Engine.IsEditorHint() && MovementChunkRender) new Thread(ThreadProcess).Start();
    }

    public override void _Process(double delta)
    {
        if (Engine.IsEditorHint()) return;
        if (Input.IsActionJustPressed("F5")) _toUpdate = !_toUpdate;
        if (Input.IsActionJustPressed("F6"))
        {
            var chunk = _positionToChunk?[new Vector2I((int)(_playerPosition.X / 16), (int)(_playerPosition.Y / 16))];
        }
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
        var chunkTilePosition = new Vector2I(Mathf.FloorToInt(globalPosition.X / (float)Chunk.Dimensions.X),
            Mathf.FloorToInt(globalPosition.Z / (float)Chunk.Dimensions.Z));
        lock (_chunkToPosition)
            if (_positionToChunk.TryGetValue(chunkTilePosition, out var chunk))
                chunk.SetBlock((Vector3I)(globalPosition - chunk.GlobalPosition), block);
    }

    public Block GetBlock(Vector3I globalBlockPosition)
    {
        var chunkTilePosition = new Vector2I(Mathf.FloorToInt(globalBlockPosition.X / (float)Chunk.Dimensions.X),
            Mathf.FloorToInt(globalBlockPosition.Z / (float)Chunk.Dimensions.Z));
        lock (_chunkToPosition)
            if (_positionToChunk.TryGetValue(chunkTilePosition, out var chunk))
                return chunk.GetBlock((Vector3I)(globalBlockPosition - chunk.GlobalPosition));
        return null;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Engine.IsEditorHint()) return;
        lock (_playerPositionLock) _playerPosition = Player.Instance.GlobalPosition;
    }

    private void ThreadProcess()
    {
        while (IsInstanceValid(this))
        {
            if (!_toUpdate) continue;
            int playerChunkX, playerChunkZ;
            lock (_playerPositionLock)
            {
                playerChunkX = Mathf.FloorToInt(_playerPosition.X / Chunk.Dimensions.X);
                playerChunkZ = Mathf.FloorToInt(_playerPosition.Z / Chunk.Dimensions.Z);
            }

            foreach (var chunk in _visibleChunks)
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
                            chunk.CallDeferred(nameof(Chunk.SetChunkPosition), [newPosition]);
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

    public bool CheckExistsSolidBlock(Vector2I chunkPosition, Vector3I blockPosition)
    {
        return true;
    }
}