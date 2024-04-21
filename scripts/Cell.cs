using Godot;
using System;
using System.Collections.Generic;

public partial class Cell : Node
{
    int _id;
    Vector3 _position;
    Node3D node;
    /// <summary>
    /// List of field cells
    /// </summary>
    static List<Cell> Cells = new List<Cell>();

    public int Id { get => _id; set => _id = value; }
    public Node3D Node { get => node; set => node = value; }
    public Cell() { }
    public static List<Cell> GetCells() => Cells;
    public static void AppendCell(Cell cell) => Cells.Add(cell);
    public static void RemoveCell(Cell cell) => Cells.Remove(cell);
    public static Cell GetCell(Vector3 position)
    {
        foreach (var cell in Cells)
            if(cell._position == position) return cell;
        return null;
    }
}
