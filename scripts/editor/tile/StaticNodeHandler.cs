using Godot;
using System;

namespace RTS.Editor
{
    public partial class StaticNodeHandler : StaticBody3D
    {
        public override void _InputEvent(Camera3D camera, InputEvent @event, Vector3 position, Vector3 normal, int shapeIdx)
        {
            base._InputEvent(camera, @event, position, normal, shapeIdx);
            if (Input.IsActionJustPressed("mouse_left_click"))
            {
                Console.WriteLine($"Node clicked: {this.Name} {GlobalPosition}, mouse click position {position}, {this.GetParent().GetParent().GetPath()}.");
                MapEditorCommand.LastClickedNode = this;
                MapEditorCommand.LastClickedPosition = position;
            }
        }
    }
}
