using Godot;
using System;

[Tool]
[GlobalClass]
public partial class Block : Resource
{
    [Export] public Texture2D Texture { get; set; }
    public Block() { }
}
