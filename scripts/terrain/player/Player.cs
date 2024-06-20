using Godot;
using RTS.Terrain;
using RTS.UI;

namespace RTS.Terrain.Character;

public enum GamemodeEnum
{
    COLLISION,
    NOCLIP
}

[Tool]
public partial class Player : CharacterBody3D
{
    private static GamemodeEnum _gamemode = GamemodeEnum.NOCLIP;
    private float _cameraXRotation;
    private float _gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
    [Export] private float _jumpVelocity = 15f;

    [Export] private float _mouseSensivity = .3f;
    [Export] private float _movementSpeed = 15f;
    [Export] public Node3D Head { get; set; }
    [Export] public Camera3D Camera { get; set; }
    [Export] public RayCast3D RayCast { get; set; }
    [Export] public MeshInstance3D BlockHighlight { get; set; }
    public static Player Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
        if (!Engine.IsEditorHint()) Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is not InputEventMouseMotion mouseMotion || Engine.IsEditorHint()) return;
        var deltaX = mouseMotion.Relative.Y * _mouseSensivity;
        var deltaY = -mouseMotion.Relative.X * _mouseSensivity;

        Head.RotateY(Mathf.DegToRad(deltaY));
        if (!(_cameraXRotation + deltaX > -90) || !(_cameraXRotation + deltaX < 90)) return;
        Camera.RotateX(Mathf.DegToRad(-deltaX));
        _cameraXRotation += deltaX;
    }

    public override void _Process(double delta)
    {
        if (Engine.IsEditorHint()) return;
        if (Input.IsActionJustPressed("F2")) ChangeGamemode();
        if (RayCast.IsColliding() && RayCast.GetCollider() is Chunk chunk)
        {
            BlockHighlight.Visible = true;
            var blockPosition = RayCast.GetCollisionPoint() - .5f * RayCast.GetCollisionNormal();
            var intBlockPosition = new Vector3(Mathf.FloorToInt(blockPosition.X), Mathf.FloorToInt(blockPosition.Y),
                Mathf.FloorToInt(blockPosition.Z));
            BlockHighlight.GlobalPosition = intBlockPosition + new Vector3(.5f, .5f, .5f);
            if (Input.IsActionJustPressed("mouse_left_click"))
            {
                var block = chunk.GetBlock((Vector3I)(intBlockPosition - chunk.GlobalPosition));
                chunk.SetBlock((Vector3I)(intBlockPosition - chunk.GlobalPosition), BlockManager.Instance.Air);
                if (block == BlockManager.Instance.Grass) GUI.Instance.AddCoin();
            }
            if (Input.IsActionJustPressed("mouse_right_click"))
                ChunkManager.Instance.SetBlock((Vector3I)(intBlockPosition + RayCast.GetCollisionNormal()),
                    BlockManager.Instance.Dirt);
        }
        else
        {
            BlockHighlight.Visible = false;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        var velocity = Velocity;

        if (!Engine.IsEditorHint() && !IsOnFloor() && _gamemode == GamemodeEnum.COLLISION)
            velocity.Y -= _gravity * (float)delta;
        var direction = Vector3.Zero;
        if (!Engine.IsEditorHint())
        {
            if (_gamemode == GamemodeEnum.NOCLIP)
            {
                if (Input.IsActionPressed("Space")) direction += Vector3.Up;
                if (Input.IsActionPressed("Shift")) direction += Vector3.Down;
            }
            else
            {
                if (Input.IsActionJustPressed("Space") && IsOnFloor())
                    velocity.Y = _jumpVelocity;
            }

            var inputDirection = Input.GetVector("A", "D", "W", "S").Normalized();
            direction += inputDirection.X * Head.GlobalBasis.X;
            direction += inputDirection.Y * Head.GlobalBasis.Z;
        }

        if (_gamemode == GamemodeEnum.NOCLIP) velocity.Y = direction.Y * _movementSpeed;

        velocity.X = direction.X * _movementSpeed;
        velocity.Z = direction.Z * _movementSpeed;

        Velocity = velocity;
        MoveAndSlide();
    }

    private static void ChangeGamemode()
    {
        _gamemode = _gamemode == GamemodeEnum.COLLISION ? GamemodeEnum.NOCLIP : GamemodeEnum.COLLISION;
    }
}