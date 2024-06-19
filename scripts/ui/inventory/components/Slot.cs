using Godot;

namespace RTS.UI.Inventory;

[Tool]
public partial class Slot : Control
{
    [Export] public TextureRect Background { get; private set; }
    [Export] public TextureRect Item { get; set; }

    public override void _Process(double delta)
    {
        Item.Texture = GD.Load<Texture2D>("res://textures/ui/grey/grey_box.png");
    }
}