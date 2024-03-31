using Godot;
using System;

public partial class GridMap : Godot.GridMap
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		var map = new GridMap();
		Console.Clear();
		Console.WriteLine(GridMap.GetGodotMethodList());

        foreach (var item in GetGodotMethodList())
        {
			Console.Write(item);
        }

		map.Get("River");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
