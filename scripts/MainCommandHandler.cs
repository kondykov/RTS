using System;
using Godot;
using RTS.Debug;

namespace RTS;

public partial class MainCommandHandler : Node3D
{
    public override void _Ready() => MainCommand.RootNode = GetTree().Root.GetChild<Node3D>(0);
}