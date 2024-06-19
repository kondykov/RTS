using Godot;

namespace RTS.Camera;

public partial class CharacterController : CharacterBody3D
{
    private int _speed = 20;
    private float _gravity = 14f;
    private float _jumpVelocity = 5f;

    public override void _Process(double delta)
    {
            Vector3 direction = Vector3.Zero;
            Vector3 velocity = Velocity;

            if (Input.IsActionPressed("E")) RotateY(0.05f);
            if (Input.IsActionPressed("Q")) RotateY(-0.05f);

            if (Input.IsActionPressed("ui_page_up")) direction += Vector3.Up;
            if (Input.IsActionPressed("ui_page_down")) direction += Vector3.Down;
            if (Input.IsActionJustPressed("space"))
            {
                Position = new Vector3(0, 0, 0);
                Rotation = new Vector3(0, -2.35f, 0);
            }

            if (!IsOnFloor() && !Input.IsActionPressed("mouse_left_click"))
                velocity.Y -= _gravity * (float)delta;

            if (Input.IsActionJustPressed("ui_page_up") && IsOnFloor())
                velocity.Y = _jumpVelocity;

            Vector2 inputDir = Input.GetVector("A", "D", "W", "S");
            inputDir = inputDir.Lerp(inputDir, 2f);
            direction = new Vector3(inputDir.X, 0, inputDir.Y)
                .Rotated(Vector3.Up, MainCommand.RootNode.GetNode<Node3D>("RTSCameraBody").Rotation.Y)
                .Normalized(); //rotates the input direction with camera rotation


            direction = direction.Rotated(new Vector3(1, 0, 0), -Rotation.X) * _speed * (float)delta;
            if (direction != Vector3.Zero)
            {
                velocity.X = direction.X * _speed * (float)delta * 60;
                velocity.Z = direction.Z * _speed * (float)delta * 60;
            }
            else
            {
                velocity.X = Mathf.MoveToward(Velocity.X, 0, _speed);
                velocity.Z = Mathf.MoveToward(Velocity.Z, 0, _speed);
            }

            Velocity = velocity;
            MoveAndSlide();
        }
}