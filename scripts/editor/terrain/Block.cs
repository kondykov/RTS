using Godot;
using System;

namespace Terrain
{
    [Tool]
    [GlobalClass]
    public partial class Block : Resource
    {
        [Export] public Texture2D Texture { get; set; }
        [Export] public Texture2D TopTexture { get; set; }
        [Export] public Texture2D BottomTexture { get; set; }
        [Export] public Texture2D SidesTexture { get; set; }
        
        public Block() { }
    }
}

