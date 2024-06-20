using Godot;
using System;
namespace RTS.UI;

public partial class GUI : Control
{
    public static GUI Instance { get; private set; }
    [Export] private Label _coinLabel;
    private int _coins = 0;

    public override void _Ready()
    {
        Instance = this;
        _coinLabel.Text = _coins.ToString();
    }

    public void AddCoin()
    {
        _coins++;
        _coinLabel.Text = _coins.ToString();
    }
}
