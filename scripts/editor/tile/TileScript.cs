using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Editor
{
    public partial class TileScript : CharacterBody3D
    {
        public override void _InputEvent(Camera3D camera, InputEvent @event, Vector3 position, Vector3 normal, int shapeIdx)
        {
            base._InputEvent(camera, @event, position, normal, shapeIdx);
            Console.WriteLine("C# event");
        }
    }
}
