using Godot;
using System;

public partial class DebugMenu : MarginContainer
{
    public override void _Ready()
    {
        base._Ready();
        var label = GetNode<Label>("DebugLabel");
        label.Visible = false;
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
        string debugInfo = $"";

        debugInfo += GetCamData();

        var label = GetNode<Label>("DebugLabel");
        if (Input.IsActionJustPressed("show_debug_menu"))
            if (label.Visible) label.Visible = false;
            else label.Visible = true;

        label.Text = debugInfo;
    }
    private string GetCamData()
    {
        var camBody = GetParent().GetParent().GetNode<Node3D>("RTSCameraBody");
        var camera = GetParent().GetParent().GetNode<Camera3D>("RTSCameraBody/RTSCamera");

        return $"Position:\nX: {camBody.Position.X}\nY: {camBody.Position.Y}\nZ: {camBody.Position.Z}\n" +
            $"Zoom: {camera.Size}\n" +
            $"Speed: {camBody.Get("Speed")}";
    }
}
