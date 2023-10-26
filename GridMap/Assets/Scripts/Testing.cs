using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEditor;

public class Testing2 : MonoBehaviour
{

    [SerializeField] private HeatMapVisual heatMapVisual;
    //[SerializeField] private HeatMapBoolVisual heatMapBoolVisual;
    private Grid grid;

    private void Start(){
        grid = new Grid (30, 20, 7f, Vector3.zero);

        //heatMapVisual.SetGrid(grid);
    }

    private void Update (){
        if (Input.GetMouseButtonDown(0)){
            Vector3 position = UtilsClass.GetMouseWorldPosition();
            // int value = grid.GetValue(UtilsClass.GetMouseWorldPosition());
            // grid.SetValue(position, value + 5);
            grid.AddValue(position, 50, 2, 25);
            //grid.SetValue(position, true);
        }
    }

}

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Testing
{

    private Grid <bool> grid;

    private void Start (){
        grid = new Grid <bool> (10, 5, 2f, Vector3.zero); //setvalue function
    }

    private void Update (){
        if (Input.GetMouseButtonDown(0)){
            Vector3 position = UtilsClass.GetMouseWorldPosition();
            grid.AddValue(position, 100, 2, 15);
        }
    }



   /* private void Update(){
        if (Input.GetMouseButtonDown(0)){
            Vector3 position = UtilsClass.GetMouseWorldPosition();
            int value = grid.GetValue(position);
            grid.SetValue(position, value + 5);
        }
    }*/


/*private void Start (){
        grid = new Grid (10, 5, 2f, new Vector3.zero(0,-10)
        );
    }
    private void Update() {
        if (Input.GetMouseButtonDown(0)){ //left click
            grid.SetValue(UtilsClass.GetMouseWorldPosition(), 56);
        }

        if (Input.GetMouseButtonDown(1)){ //right click
            Debug.Log(grid.GetValue(UtilsClass.GetMouseWorldPosition()));
        }
    }
/*
    private class HeatMapVisual {
        private Grid grid;

        public HeatMapVisual(Grid grid){
            this.grid = grid;

            Vector3[] vertices;
            Vector2[] uv;
            int[] triangles;

            MeshUtils.CreateEmptyMeshArrays(grid.GetWidth()* grid.GetHeight(), out vertices, out uv, out triangles);
                for(int x = 0; x <grid.GetWidth(); x++){
                    for (int y = 0; y < grid.GetHeight(); y++){
                        int index = x * y;
                        MeshUtils.AddToMeshArrays(vertices, uv, triangles);
                    }
                }
        
        }   
    }*/


