using Godot;

namespace RTS.Debug;

public partial class DebugMenu : MarginContainer
{
    private bool dbgField = false;
    public static bool VisibleDebugMenu = false;
    static public Vector3 MousePosition = new Vector3(0, 0, 0);

    public override void _Ready()
    {
            var label = GetNode<Label>("DebugLabel");
            label.Visible = false;
        }

    public override void _Process(double delta)
    {
            var label = GetNode<Label>("DebugLabel");
            VisibleDebugMenu = label.Visible;
            if (Input.IsActionJustPressed("F3"))
                if (label.Visible) label.Visible = false;
                else label.Visible = true;

            string debugInfo = $"";
            debugInfo += $"FPS: {Engine.GetFramesPerSecond()}\n";
            debugInfo += $"Delta: {delta}";
            debugInfo += GetPlayerData();
            //debugInfo += GetCamData();
            if (Input.IsActionJustPressed("F2"))
                if (dbgField) dbgField = false;
                else dbgField = true;
            //debugInfo += GetPressedKeys();
            label.Text = debugInfo;
        }

    private string GetPlayerData()
    {
            var player = MainCommand.RootNode.GetNode<CharacterBody3D>("Player");
            return $"Position:\nX: {player.Position.X}\nY: {player.Position.Y}\nZ: {player.Position.Z}\n" +
                   $"Speed: {player.Get("Speed")}\n" +
                   $"Rotation: {player.Rotation}\n";
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
            if (Input.IsActionPressed("W")) keysPressed += "W ";
            if (Input.IsActionPressed("A")) keysPressed += "A ";
            if (Input.IsActionPressed("S")) keysPressed += "S ";
            if (Input.IsActionPressed("D")) keysPressed += "D ";
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
}