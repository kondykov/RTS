using Godot;
using System;
using static Godot.TextServer;

public partial class DebugMenu : MarginContainer
{
    private bool dbgField = false;
    static Node _root;
    static public Vector3 MousePosition = new Vector3(0, 0, 0);
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
        debugInfo += $"FPS: {Engine.GetFramesPerSecond()}\n";

        debugInfo += GetCamData();
        if (Input.IsActionJustPressed("F2"))
            if (dbgField) dbgField = false;
            else dbgField = true;
        if (dbgField) debugInfo += $"\n{Get3DCursorInfo()}\n";
        else debugInfo += GetPressedKeys();
        FormatPosition(new Vector3(3, 5, 2));

        var label = GetNode<Label>("DebugLabel");
        if (Input.IsActionJustPressed("F3"))
            if (label.Visible) label.Visible = false;
            else label.Visible = true;


        label.Text = debugInfo;
    }

    private string Get3DCursorInfo()
    {
        string info = "";

        if (_root.GetNodeOrNull("3DCursor") != null)
        {
            info += $"Selector position: {_root.GetNode<Node3D>("3DCursor").Position}\n";
            info += $"3D mouse position: {MousePosition}\n";
            info += $"Grid size: {Builder.GridSize}\n";
        }
        return info;
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
        var cells = Cell.GetCells();
        int i = 0;
        foreach ( var cell in cells )
        {
            if (i >= Builder.GridSize)
            {
                field += "\n";
                i = 0;
            }
            var pos = $"{FormatPosition(cell.Node.Position)}";
            field += $"{pos}";
            i++;
        }
        return field;
    }
    
    private string FormatPosition(Vector3 position)
    {
        string X = position.X.ToString();
        string Y = position.Y.ToString();
        string Z = position.Z.ToString();

        if (X.Length < 2) X = $"0{X}";
        //if (X.Length < 3) X = $"0{X}";
/*        if (Y.Length < 2) Y = $"00{Y}";
        if (Y.Length < 3) Y = $"0{Y}";*/
        if (Z.Length < 2) Z = $"0{Z}";
        //if (Z.Length < 3) Z = $"0{Z}";
        return $"|{X}::{Z}|";
    }
}
