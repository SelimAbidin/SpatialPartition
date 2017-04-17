using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static List<Vector3> clash = new List<Vector3>();

    public int ENEMY_SIZE = 500;
    public static Grid grid;
    List<GameObject> soldierList;
    public GameObject soldier;	
	void Start ()
    {
        grid = new Grid();



        soldierList = new List<GameObject>();

        GameObject _temp;
        for (int i = 0; i < ENEMY_SIZE; i++)
        {
            _temp = GameObject.Instantiate(soldier);
            float x = UnityEngine.Random.Range(0, 100);
            float y = UnityEngine.Random.Range(0, 100);
            _temp.transform.localPosition = new Vector3(x, y);
            soldierList.Add(_temp);

            Soldier soldierComp = _temp.GetComponent<Soldier>();


            if(soldierComp != null)
            {
                soldierComp.CreateUnit();
            }

            //
        }



        grid.HandleMelee();

    }


    private void OnDrawGizmos()
    {
        if(grid != null)
        {
            Gizmos.color = new Color(1,0,0);
            float sizeX = 100f / grid.WIDTH_NUM_CELLS;
            float sizeY = 100f / grid.HEIGHT_NUM_CELLS;

            for (int i = 0; i < grid.WIDTH_NUM_CELLS; i++)
            {
                Vector3 to = new Vector3(i * sizeX, 0);
                Vector3 from = new Vector3(i * sizeX, 100);
                Gizmos.DrawLine(to, from);
            }

            for (int j = 0; j < grid.HEIGHT_NUM_CELLS; j++)
            {
                Vector3 to = new Vector3(0, j * sizeY);
                Vector3 from = new Vector3(100,  j * sizeY);
                Gizmos.DrawLine(to, from);
            }

            Gizmos.color = new Color(0,1,0);
            for (int i = 0; i < clash.Count; i++)
            {
                //Gizmos.color = new Color(Random.Range(0, 100) / 100f, Random.Range(0, 100) / 100f, Random.Range(0, 100) / 100f);
                Vector3 unit = clash[i];
                i++;
                Vector3 other = clash[i];

                Gizmos.DrawLine(unit, other);
            }
        }



       
    }


    void Update ()
    {
        clash.Clear();
        if (isSpatialOn)
        {
            

            grid.HandleMelee();
        }
        else
        {
            ClassicDistance();
        }
        


	}

    private void ClassicDistance()
    {
        int say = 0;
        for (int i = 0; i < soldierList.Count; i++)
        {
            for (int j = i + 1; j < soldierList.Count; j++)
            {
                Vector3 s1 = soldierList[i].transform.position;
                Vector3 s2 = soldierList[j].transform.position;

                if(Distance(s1.x, s2.x, s1.y, s2.y) < GameManager.ATTACK_DIST)
                {
                    HandleAttack(s1, s2);
                }

                say++;

            }
        }
    }

    private void HandleAttack(Vector3 s1, Vector3 s2)
    {
        GameManager.clash.Add(s1);
        GameManager.clash.Add(s2);
    }



    public bool isSpatialOn = true;

    public static float ATTACK_DIST = 3;

    public void OnSpatialPartitionCheck(UnityEngine.UI.Toggle toggle)
    {
        isSpatialOn = toggle.isOn;
        //Debug.Log(v);
    }


    private float Distance(float x1, float x2, float y1, float y2)
    {
        float dx = x1 - x2;
        float dy = y1 - y2;

        return Mathf.Sqrt((dx * dx) + (dy * dy));
    }
}
