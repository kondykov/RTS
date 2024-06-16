using System;
using Godot;
using RTS.Debug;

namespace RTS;

public partial class MainCommandHandler : Node3D
{
    [Export] private AudioStreamPlayer _sound;
    public override void _Ready()
    {
        MainCommand.RootNode = GetTree().Root.GetChild<Node3D>(0);
        _sound?.Play();
        
    }

    public override void _Process(double delta)
    {
        if (_sound.StreamPaused)
        {
            _sound?.Play();
        }
    }
}