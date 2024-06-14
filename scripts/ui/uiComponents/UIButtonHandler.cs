using Godot;

namespace RTS.UI;

[Tool]
public partial class UIButtonHandler : TextureButton
{
    private TextureButton _instance;
    [Export] private Label _label;
    [Export] private AudioStreamPlayer _soundByClick;
    [Export] private AudioStreamPlayer _soundByHover;

    public override void _Ready()
    {
        _label.Text = Name;
        _instance = this;
    }

    public override void _Process(double delta)
    {
        if (Engine.IsEditorHint()) _label.Text = Name;
    }

    public void ActivateSoundByHover()
    {
        if (!_instance.Disabled) _soundByHover.Play();
    }

    public void ActivateSoundByClick() => _soundByClick.Play();
}