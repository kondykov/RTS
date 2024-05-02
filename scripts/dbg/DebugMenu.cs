using Godot;
using RTS;
using RTS.Loaders;
using System;
using static Godot.HttpRequest;
using static Godot.TextServer;

public partial class DebugMenu : MarginContainer
{
    private bool dbgField = false;
    static public Vector3 MousePosition = new Vector3(0, 0, 0);
    static public string SelectedNodeCollision;
    static public string Test;
    public override void _Ready()
    {
        base._Ready();
        var label = GetNode<Label>("DebugLabel");
        label.Visible = false;
        string info = "Controlls:\n" +
            "WASD - Walk\n" +
            "Page Up - To up\n" +
            "Page Down - To down\n" +
            "Mouse wheel - Zoom\n" +
            "Mouse left btn - Select cell\n" +
            "Mouse right btn - N/A\n" +
            "Space - Return position to start\n" +
            "Escape - Deselect cell\n";
        var rightLabel = GetParent().GetNode<Label>("DebugContainerRight/Label");
        rightLabel.Text = info;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        string debugInfo = $"";
        debugInfo += $"FPS: {Engine.GetFramesPerSecond()}\n";
        debugInfo += $"Mode: {MainCommand.Mode}\n";
        debugInfo += GetCamData();
        if (Input.IsActionJustPressed("F2"))
            if (dbgField) dbgField = false;
            else dbgField = true;
        debugInfo += GetPressedKeys();
        if (dbgField) debugInfo += $"\n{Get3DCursorInfo()}\n";

        var label = GetNode<Label>("DebugLabel");
        if (Input.IsActionJustPressed("F3"))
            if (label.Visible) label.Visible = false;
            else label.Visible = true;

        if (Input.IsActionJustPressed("F5"))
        {
            debugInfo += GetTileList();
        }
        label.Text = debugInfo;
    }

    private string Get3DCursorInfo()
    {
        string info = "";

        info += $"Grid size: {MainCommand.GridSize}\n";
        info += $"3D mouse position: {MousePosition}\n";
        if (MainCommand.RootNode.GetNodeOrNull("3DCursor") != null)
            info += $"Selector position: {MainCommand.RootNode.GetNode<Node3D>("3DCursor").Position}\n";
        return info;
    }

    private string GetCamData()
    {
        var camBody = MainCommand.RootNode.GetNode<Node3D>("RTSCameraBody");
        var camera = MainCommand.RootNode.GetNode<Camera3D>("RTSCameraBody/RTSCamera");

        return $"Position:\nX: {camBody.Position.X}\nY: {camBody.Position.Y}\nZ: {camBody.Position.Z}\n" +
            $"Zoom isometric cam: {camera.Size}\n" +
            $"Zoom perspective cam: {camera.Fov}\n" +
            $"Speed: {camBody.Get("Speed")}\n" +
            $"Rotation: {camBody.Rotation}\n";
    }
    private string GetPressedKeys()
    {
        string keysPressed = "Pressed keys:\n";
        if (Input.IsActionPressed("camera_forward")) keysPressed+="W ";
        if (Input.IsActionPressed("camera_right")) keysPressed += "A ";
        if (Input.IsActionPressed("camera_backward")) keysPressed += "S ";
        if (Input.IsActionPressed("camera_left")) keysPressed += "D ";
        if (Input.IsActionPressed("mouse_circle_up")) keysPressed += "Q ";
        if (Input.IsActionPressed("mouse_circle_down")) keysPressed += "E ";
        if (Input.IsActionPressed("space")) keysPressed += "Space ";
        if (Input.IsActionPressed("camera_speed_max")) keysPressed += "+ ";
        if (Input.IsActionPressed("camera_speed_min")) keysPressed += "- ";
        if (Input.IsActionPressed("ui_page_up")) keysPressed += "Page_up ";
        if (Input.IsActionPressed("ui_page_down")) keysPressed += "Page_down ";
        keysPressed += '\n';
        return keysPressed;
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
    private string GetTreeDBG()
    {
        string str = "";
        foreach (var item in MainCommand.RootNode.GetChildren())
        {
            try
            {
                str += $"{item,30}:{item.Name}:{item.GetChildren().Count}:{MainCommand.AsNode3D(item).Position}\n";
            }
            catch
            {
                str += $"{item,30}:{item.Name}:NULL\n";
            }
            if (item.GetChildren().Count > 0) str += GetTreeDBG2(item);
        }
        return str;
    }
    private string GetTreeDBG2(Node str) 
    {
        string result = "";
        foreach (var item in str.GetChildren())
        {
            try
            {
                result += $"\t{item,30}:{item.Name}:{item.GetChildren().Count}:{MainCommand.AsNode3D(item).Position}\n";
            }
            catch
            {
                result += $"\t{item,30}:{item.Name}:{item.GetChildren().Count}:NULL\n";
            }
        }
        return result;
    }
    private string GetTileList() 
    {
        string result = "";
        foreach (var item in Preloader.Tiles)
        {
            result += item.ToString() + '\n';
        }
        return result;
    }
}
