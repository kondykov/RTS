using Godot;

namespace RTS.UI;

[Tool]
public partial class UIButtonHandler : Control
{
    [Export] private Label _label;
    [Export] private AudioStreamPlayer _soundByClick;
    [Export] private AudioStreamPlayer _soundByHover;
    public override void _Ready()
    {
        _label.Text = Name;
    }

    public override void _Process(double delta)
    {
        if (Engine.IsEditorHint()) _label.Text = Name;
    }
    
    public void ActivateSoundByHover() => _soundByHover.Play();

    public void ActivateSoundByClick() => _soundByClick.Play();
}