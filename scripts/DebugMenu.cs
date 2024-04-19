using Godot;
using System;
using static Godot.TextServer;

public partial class DebugMenu : MarginContainer
{
    private bool dbgField = false;
    static Node _root;
    public override void _Ready()
    {
        base._Ready();
        var label = GetNode<Label>("DebugLabel");
        label.Visible = false;
        _root = GetParent().GetParent();
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
        string debugInfo = $"";

        debugInfo += GetCamData();
        if (Input.IsActionJustPressed("dbg_field"))
            if (dbgField) dbgField = false;
            else dbgField = true;
        if (dbgField) debugInfo += GetField();
        else debugInfo += GetPressedKeys();

        var label = GetNode<Label>("DebugLabel");
        if (Input.IsActionJustPressed("dbg"))
            if (label.Visible) label.Visible = false;
            else label.Visible = true;

        label.Text = debugInfo;
    }
    private string GetCamData()
    {
        var camBody = _root.GetNode<Node3D>("RTSCameraBody");
        var camera = _root.GetNode<Camera3D>("RTSCameraBody/RTSCamera");

        return $"Position:\nX: {camBody.Position.X}\nY: {camBody.Position.Y}\nZ: {camBody.Position.Z}\n" +
            $"Zoom: {camera.Size}\n" +
            $"Speed: {camBody.Get("Speed")}\n";
    }
    private string GetPressedKeys()
    {
        string keysPressed = "Pressed keys:\n";
        if (Input.IsActionPressed("camera_forward")) keysPressed+="W ";
        if (Input.IsActionPressed("camera_right")) keysPressed += "A ";
        if (Input.IsActionPressed("camera_backward")) keysPressed += "S ";
        if (Input.IsActionPressed("camera_left")) keysPressed += "D ";
        if (Input.IsActionPressed("zoom_in")) keysPressed += "Q ";
        if (Input.IsActionPressed("zoom_out")) keysPressed += "E ";
        if (Input.IsActionPressed("space")) keysPressed += "Space ";
        if (Input.IsActionPressed("camera_speed_max")) keysPressed += "+ ";
        if (Input.IsActionPressed("camera_speed_min")) keysPressed += "- ";
        if (Input.IsActionPressed("ui_page_up")) keysPressed += "Page_up ";
        if (Input.IsActionPressed("ui_page_down")) keysPressed += "Page_down ";

        return keysPressed;
    }
    private string GetField()
    {
        string field = "";
        for (int i = 0; i < Builder.gridSize; i+=2)
        {
            for (int j = 0; j < Builder.gridSize; j += 2)
            {
                field += $"{Cell.GetCell(new Vector3(i, 0, j)).Name, 10}";
            }
            field += "\n";
        }
        return field;
    }
}
