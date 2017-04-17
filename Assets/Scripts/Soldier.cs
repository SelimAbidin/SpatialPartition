using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{

    Unit unit;
	void Start ()
    {
       

	}

    public void CreateUnit()
    {
        Vector3 pos = transform.position;
        unit = new Unit(GameManager.grid, pos.x, pos.y);
    }
	
	void Update ()
    {
        Vector3 pos = transform.position;
        unit.Move(pos.x, pos.y);
    }
}
