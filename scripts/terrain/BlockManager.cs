using System;
using System.Linq;
using Godot;
using Godot.Collections;

namespace RTS.Terrain;

[Tool]
public partial class BlockManager : Node
{
    private readonly Dictionary<Texture2D, Vector2I> _atlasLookup = new();
    private int _gridHeight;
    private int _gridWidth = 4;
    [Export] private AudioStreamPlayer3D PlaceSound { get; set; }
    [Export] private AudioStreamPlayer3D DestroySound { get; set; }
    [Export] public Block Air { get; set; }
    [Export] public Block Grass { get; set; }
    [Export] public Block Dirt { get; set; }
    [Export] public Block Stone { get; set; }

    private Vector2I BlockTextureSize { get; } = new(16, 16);
    public Vector2 TextureAtlasSize { get; private set; }
    public static BlockManager Instance { get; private set; }
    public StandardMaterial3D ChunkMaterial { get; private set; }
    public Dictionary<Block, AudioStream> BlockSounds { get; private set; }

    public override void _Ready()
    {
        Instance = this;
        InitializeTextureAtlas();
        BlockSounds = new Dictionary<Block, AudioStream>
        {
            { Grass, GD.Load<AudioStream>("res://sounds/block/breakGrass.ogg") },
            { Dirt, GD.Load<AudioStream>("res://sounds/block/breakDirt.ogg") },
            { Stone, GD.Load<AudioStream>("res://sounds/block/breakStone.ogg") }
        };
    }

    public void SoundPlay(Vector3 blockPosition, Block block)
    {
        PlaceSound.GlobalPosition = blockPosition;
        BlockSounds.TryGetValue(block, out var sound);
        PlaceSound.Stream = sound;
        PlaceSound?.Play();
    }

    private void InitializeTextureAtlas()
    {
        var blockTextures = new[] { Air, Grass, Dirt, Stone }.Select(block => block.Texture)
            .Where(texture => texture != null).Distinct().ToArray();
        for (var i = 0; i < blockTextures.Length; i++)
        {
            var texture = blockTextures[i];
            _atlasLookup.Add(texture, new Vector2I(i % _gridWidth, Mathf.FloorToInt(i / _gridWidth)));
        }

        _gridHeight = Mathf.CeilToInt(blockTextures.Length / (float)_gridWidth);
        Console.WriteLine(Mathf.CeilToInt(blockTextures.Length / (float)_gridWidth) + " " + _gridHeight);
        var image = Image.Create(_gridWidth * BlockTextureSize.X, _gridHeight * BlockTextureSize.Y, false,
            Image.Format.Rgba8);
        for (var x = 0; x < _gridWidth; x++)
        for (var y = 0; y < _gridHeight; y++)
        {
            var imgIndex = x + y * _gridWidth;
            if (imgIndex >= blockTextures.Length) continue;
            var currentImage = blockTextures[imgIndex].GetImage();
            currentImage.Convert(Image.Format.Rgba8);
            image.BlitRect(currentImage, new Rect2I(Vector2I.Zero, BlockTextureSize),
                new Vector2I(x, y) * BlockTextureSize);
        }

        var textureAtlas = ImageTexture.CreateFromImage(image);
        ChunkMaterial = new StandardMaterial3D
        {
            AlbedoTexture = textureAtlas,
            TextureFilter = BaseMaterial3D.TextureFilterEnum.Nearest
        };
        TextureAtlasSize = new Vector2(_gridWidth, _gridHeight);
        GD.Print($"Done loading {blockTextures.Length} images to make {_gridWidth} x {_gridHeight} atlas.");
    }

    public Vector2I GetTextureAtlasPosition(Texture2D texture)
    {
        return texture == null ? Vector2I.Zero : _atlasLookup[texture];
    }

    public static void GetMissingTexture(Block missingBlockTexture)
    {
        missingBlockTexture.Texture = GD.Load<Texture2D>("res://prefabs/textures/MissingTexture.png");
    }
}