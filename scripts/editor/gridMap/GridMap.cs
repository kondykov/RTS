using Godot;
using Godot.Collections;
using RTS;
using System;
using System.Collections.Generic;

public partial class GridMap : Godot.GridMap
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Clear();
		SetCellItem(new Vector3I(-1, 0, -1), 0);
		var cell = GetCellItem(new Vector3I(-1, 0, -1));
		Vector3 pos = MainCommand.CurrentCamera.GetRaycast()["Position"].AsVector3();

		var item = GetCellItemBasis(LocalToMap(pos));


		Console.WriteLine(GetCellItemBasis(LocalToMap(pos)));
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}
