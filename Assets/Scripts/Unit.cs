using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit
{
    Grid _grid;
    public float x;
    public float y;
    public Unit _prev;
    public Unit _next;
    public Unit(Grid grid, float x, float y)
    {
        this._grid = grid;
        this.x = x;
        this.y = y;

        this._grid.Add(this);
    }


    public void Move(float x, float y)
    {
        _grid.Move(this,x, y);
    }
}
