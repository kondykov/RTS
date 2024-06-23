using Godot;

namespace RTS.Terrain;

[Tool]
[GlobalClass]
public partial class Block : Resource
{
    [Export] public Texture2D Texture { get; set; }
    [Export] public Texture2D TopTexture { get; set; }
    [Export] public Texture2D BottomTexture { get; set; }
    [Export] public Texture2D SidesTexture { get; set; }
    [Export] public BlockType BlockType { get; set; } = BlockType.Air;
}

public enum BlockType
{
    Air,
    Grass,
    Dirt,
    Stone,
}

public enum BlockActionType
{
    System,
    PlayerPlace,
    PlayerDestroy,
}