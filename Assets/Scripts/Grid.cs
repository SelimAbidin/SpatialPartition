using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public int WIDTH_NUM_CELLS = 30;
    public int HEIGHT_NUM_CELLS = 30;


    Unit[][] _cells;
    public Grid()
    {
        _cells = new Unit[WIDTH_NUM_CELLS][];
        for (int x = 0; x < WIDTH_NUM_CELLS; x++)
        {
            _cells[x] = new Unit[HEIGHT_NUM_CELLS];
            for (int y = 0; y < HEIGHT_NUM_CELLS; y++)
            {
                _cells[x][y] = null;
            }
        }
    }

    internal void Move(Unit unit,float x, float y)
    {
        int oldCellX = (int)(unit.x / this.WIDTH_NUM_CELLS);
        int oldCellY = (int)(unit.y / this.HEIGHT_NUM_CELLS);

        // See which cell it's moving to.
        int cellX = (int)(x / this.WIDTH_NUM_CELLS);
        int cellY = (int)(y / this.HEIGHT_NUM_CELLS);

        unit.x = x;
        unit.y = y;

        // If it didn't change cells, we're done.
        if (oldCellX == cellX && oldCellY == cellY) return;

        // Unlink it from the list of its old cell.
        if (unit._prev !=  null)
        {
            unit._prev._next = unit._next;
        }

        if (unit._next != null)
        {
            unit._next._prev = unit._prev;
        }

        // If it's the head of a list, remove it.
        if (_cells[oldCellX][oldCellY] == unit)
        {
            _cells[oldCellX][oldCellY] = unit._next;
        }

        // Add it back to the grid at its new cell.
        Add(unit);
    }

    public void Add(Unit unit)
    {
        int cellX = (int)(unit.x / WIDTH_NUM_CELLS);
        int cellY = (int)(unit.y / HEIGHT_NUM_CELLS);

        unit._prev  = null;
        unit._next = _cells[cellX][cellY];
        _cells[cellX][cellY] = unit;

        if (unit._next != null)
        {
            unit._next._prev = unit;
        }

    }


    public void HandleMelee()
    {
        for (int x = 0; x < WIDTH_NUM_CELLS; x++)
        {
            for (int y = 0; y < HEIGHT_NUM_CELLS; y++)
            {
                HandleCell(x,y);
            }
        }
    }

    private void HandleCell(int x , int y)
    {
        Unit unit = _cells[x][y];
        while (unit != null)
        {
            HandleUnit(unit, unit._next);

            if (x > 0 && y > 0) HandleUnit(unit, _cells[x - 1][y - 1]);
            if (x > 0) HandleUnit(unit, _cells[x - 1][y]);
            if (y > 0) HandleUnit(unit, _cells[x][y - 1]);
            if (x > 0 && y < HEIGHT_NUM_CELLS - 1)
            {
                HandleUnit(unit, _cells[x - 1][y + 1]);
            }

            unit = unit._next;
        }
    }

    void HandleUnit(Unit unit, Unit other)
    {
        while (other != null)
        {
            if (Distance(unit.x, other.x, unit.y, other.y) < GameManager.ATTACK_DIST)
            {
                HandleAttack(unit, other);
            }

            other = other._next;
        }
    }

    private void HandleAttack(Unit unit, Unit other)
    {
        GameManager.clash.Add(new Vector3(unit.x, unit.y, 0));
        GameManager.clash.Add(new Vector3(other.x, other.y, 0));
    }

    private float Distance(float x1, float x2, float y1, float y2)
    {
        float dx = x1 - x2;
        float dy = y1 - y2;

        return Mathf.Sqrt((dx * dx) + (dy * dy));
    }
}
