using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class BlockManager : Node
{
    [Export] public Block Air {  get; set; }
    [Export] public Block Grass { get; set; }
    [Export] public Block Dirt { get; set; }

    private readonly Dictionary<Texture2D, Vector2I> _atlasLookup = new();
    private int _gridWidth = 16;
    private int _gridHeight = 16;

    public Vector2I BlockTextureSize { get; } = new(16, 16);
    public Vector2 TextureAtlasSize { get; private set; }
    public static BlockManager Instance { get; private set; }
    public StandardMaterial3D ChunkMaterial { get; private set; }
    public override void _Ready()
    {
        Instance = this;
        var blockTextures = new Block[] { Air, Grass, Dirt, }.Select(block => block.Texture).Where(texture => texture != null).Distinct().ToArray();
        for (int i = 0; i < blockTextures.Length; i++)
        {
            var texture = blockTextures[i];
            _atlasLookup.Add(texture, new Vector2I(i % _gridWidth, Mathf.FloorToInt(i / _gridWidth)));
        }
        _gridWidth = Mathf.CeilToInt(blockTextures.Length / (float)_gridWidth);
        var image = Image.Create(_gridWidth * BlockTextureSize.X, _gridHeight * BlockTextureSize.Y, false, Image.Format.Rgba8);
        for (int x = 0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                var imgIndex = x + y * _gridWidth;
                if (imgIndex >= blockTextures.Length) continue;
                var currentImage = blockTextures[imgIndex].GetImage();
                currentImage.Convert(Image.Format.Rgba8);
                image.BlitRect(currentImage, new Rect2I(Vector2I.Zero, BlockTextureSize), new Vector2I(x,y) * BlockTextureSize);
                //image.Resize(BlockTextureSize.X, BlockTextureSize.Y);
            }
        }
        var textureAtlas = ImageTexture.CreateFromImage(image);
        ChunkMaterial = new()
        {
            AlbedoTexture = textureAtlas,
            TextureFilter = BaseMaterial3D.TextureFilterEnum.Nearest
        };
        TextureAtlasSize = new Vector2(_gridWidth, _gridHeight);
        GD.Print($"Done loading {blockTextures.Length} images to make {_gridWidth} x {_gridHeight} atlas.");
    }
    public Vector2I GetTextureAtlasPosition(Texture2D texture)
    {
        if (texture == null) return Vector2I.Zero;
        else return _atlasLookup[texture];
    }
}