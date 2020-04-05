using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;


public class Testing : MonoBehaviour
{
    private Grid grid;
    private void Start() {
        Grid grid = new Grid(4, 2,1f,new Vector3(20,0));
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            grid.SetValue( UtilsClass.GetMouseWorldPosition(),56);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetValue(UtilsClass.GetMouseWorldPosition()));
        }
    }
}
