using Godot;
using System;

namespace Terrain.Player
{
    public partial class Player : CharacterBody3D
    {
        [Export] public Node3D Head { get; set; }
        [Export] public Camera3D Camera { get; set; }
        [Export] public RayCast3D RayCast { get; set; }
        [Export] public MeshInstance3D BlockHighlight { get; set; }

        [Export] private float _mouseSensivity = .3f;
        [Export] private float _movementSpeed = 16f;
        [Export] private float _jumpVelocity = 10f;
        private float _cameraXRotation;
        private float _gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
        public static Player Instance { get; private set; }

        public override void _Ready()
        {
            Instance = this;
            Input.MouseMode = Input.MouseModeEnum.Captured;
        }
        public override void _Input(InputEvent @event)
        {
            if(@event is InputEventMouseMotion)
            {
                var mouseMotion = @event as InputEventMouseMotion;
                var deltaX = mouseMotion.Relative.Y * _mouseSensivity;
                var deltaY = -mouseMotion.Relative.X * _mouseSensivity;

                Head.RotateY(Mathf.DegToRad(deltaY));
                if (_cameraXRotation + deltaX > -90 && _cameraXRotation + deltaX < 90)
                {
                    Camera.RotateX(Mathf.DegToRad(-deltaX));
                    _cameraXRotation += deltaX;
                }
            }
        }
        public override void _Process(double delta)
        {
            if(RayCast.IsColliding() && RayCast.GetCollider() is Chunk chunk)
            {
                BlockHighlight.Visible = true;
                var blockPosition = RayCast.GetCollisionPoint() - .5f * RayCast.GetCollisionNormal();
                var intBlockPosition = new Vector3(Mathf.FloorToInt(blockPosition.X), Mathf.FloorToInt(blockPosition.Y), Mathf.FloorToInt(blockPosition.Z));
                BlockHighlight.GlobalPosition = intBlockPosition + new Vector3(.5f, .5f, .5f);
                if (Input.IsActionJustPressed("mouse_left_click"))
                    chunk.SetBlock((Vector3I)(intBlockPosition - chunk.GlobalPosition), BlockManager.Instance.Air);
                if (Input.IsActionJustPressed("mouse_right_click"))
                    chunk.SetBlock((Vector3I)(intBlockPosition - chunk.GlobalPosition + RayCast.GetCollisionNormal()), BlockManager.Instance.Dirt);
            }
            else
            {
                BlockHighlight.Visible = false;
            }
        }
        public override void _PhysicsProcess(double delta)
        {
            var velocity = Velocity;
            if (!IsOnFloor()) velocity.Y -= _gravity * (float)delta;
            if (Input.IsActionJustPressed("space") && IsOnFloor()) velocity.Y = _jumpVelocity;
            var inputDirection = Input.GetVector("A", "D", "W", "S").Normalized();
            var direction = Vector3.Zero;
            direction += inputDirection.X * Head.GlobalBasis.X;
            direction += inputDirection.Y * Head.GlobalBasis.Z;

            velocity.X = direction.X * _movementSpeed;
            velocity.Z = direction.Z * _movementSpeed;

            Velocity = velocity;
            MoveAndSlide();
        }
    }
}
