using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Grid {

    public const int HEAT_MAP_MAX_VALUE = 100;
    public const int HEAT_MAP_MIN_VALUE = 0;
    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs {
        public int x;
        public int y;
    }

    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private int[,] gridArray;
    private TextMesh[,] debugTextArray;

    public Grid(int width, int height, float cellSize, Vector3 originPosition){
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new int[width, height];
        //debugTextArray = new TextMesh[width, height];

        //Debug.Log(width + " " + height);

        bool showDebug = true;
        // bool showDebug = false;  tangalin visual ng grid and values
        if (showDebug) {
            TextMesh[,] debugTextArray = new TextMesh[width, height];
        
            for (int x = 0; x < gridArray.GetLength(0); x++){
                for (int y = 0; y < gridArray.GetLength(1); y++){
                    //Debug.Log(x + ", " + y);
                    debugTextArray[x, y] =  UtilsClass.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y)+ new Vector3(cellSize, cellSize) * .5f, 30, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>{
                debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y].ToString();
            };
        }
        //SetValue(2, 1, 56);
    }

    public int GetWidth(){
        return width;
    }

    public int GetHeight(){
        return height;
    }

    public float GetCellSize(){
        return cellSize;
    }

    public Vector3 GetWorldPosition (int x, int y){
        return new Vector3(x, y) * cellSize + originPosition;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y){
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    public void SetValue(int x, int y, int value){
        if (x >= 0 && y >= 0 && x < width && y < height){
            //gridArray[x, y] = value;
            gridArray[x, y] = Mathf.Clamp(value, HEAT_MAP_MIN_VALUE, HEAT_MAP_MAX_VALUE);
            //debugTextArray[x, y].text = gridArray[x, y].ToString();
            if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs {x = x, y = y});
        }
    }

    public void SetValue(Vector3 worldPosition, int value){
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public void AddValue(int x, int y, int value){
        SetValue(x, y, GetValue(x, y) + value);
    }

    public int GetValue(int x, int y){
        if (x >= 0 && y >= 0 && x < width && y < height){
            return gridArray[x, y];
        } else{
            return 0;
        }
    }

    public int GetValue(Vector3 worldPosition){
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }

    public void AddValue(Vector3 worlPosition, int value, int fullValueRange, int totalRange){
        int lowerValueAmount = Mathf.RoundToInt((float)value / (totalRange - fullValueRange));
        GetXY(worlPosition, out int originX, out int originY);
        for (int x = 0; x < totalRange; x++){
            for (int y = 0; y < totalRange - x; y++){
                int radius = x + y;
                int addValueAmount = value;
                if (radius > fullValueRange){
                    addValueAmount -= lowerValueAmount * (radius - fullValueRange);
                }

                AddValue(originX + x, originY + y, addValueAmount); // right triangle

                if (x !=0){   // All 5 value sa triangle
                    AddValue(originX - x, originY + y, addValueAmount); // triangle
                }
                if (y != 0){
                    AddValue(originX + x, originY - y, addValueAmount); //diamond na lower part
                    if (x != 0){
                          AddValue(originX - x, originY - y, addValueAmount); //diamond na lower part
                    }
                }
            }
        }
    }

}

/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Grid <TGridObject> {

    public const int HEAT_MAP_MAX_VALUE = 100;
    public const int HEAT_MAP_MIN_VALUE = 0;
    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs {
        public int x;
        public int y;
        public int value;
    }

    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private TGridObject [,] gridArray;
    private TextMesh[,] debugTextArray;

    public Grid(int width, int height, float cellSize, Vector3 originPosition){
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width, height];
        //debugTextArray = new TextMesh[width, height];

        //Debug.Log(width + " " + height);

        bool showDebug = true;
        // bool showDebug = false;  tangalin visual ng grid and values
        if (showDebug) {
            TextMesh[,] debugTextArray = new TextMesh[width, height];
        
            for (int x = 0; x < gridArray.GetLength(0); x++){
                for (int y = 0; y < gridArray.GetLength(1); y++){
                    //Debug.Log(x + ", " + y);
                    debugTextArray[x, y] =  UtilsClass.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y)+ new Vector3(cellSize, cellSize) * .5f, 30, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>{
                debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y].ToString();
            };
        }
        //SetValue(2, 1, 56);
    }

    public int GetWidth(){
        return width;
    }

    public int GetHeight(){
        return height;
    }

    public float GetCellSize(){
        return cellSize;
    }

    public Vector3 GetWorldPosition (int x, int y){
        return new Vector3(x, y) * cellSize + originPosition;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y){
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    public void SetValue(int x, int y, TGridObject value){
        if (x >= 0 && y >= 0 && x < width && y < height){
            //gridArray[x, y] = value;
            gridArray[x, y] = value;  //Mathf.Clamp(value, HEAT_MAP_MIN_VALUE, HEAT_MAP_MAX_VALUE);
            //debugTextArray[x, y].text = gridArray[x, y].ToString();
            if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs {x = x, y = y});
        }
    }

    public void SetValue(Vector3 worldPosition, TGridObject value){
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    /*public void AddValue(int x, int y, int value){
        SetValue(x, y, GetValue(x, y) + value);
    }

    public TGridObject GetValue(int x, int y){
        if (x >= 0 && y >= 0 && x < width && y < height){
            return gridArray[x, y];
        } else{
            return  default(TGridObject);
        }
    }

    public TGridObject GetValue(Vector3 worldPosition){
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }

}    

/*
    public void AddValue(Vector3 worlPosition, int value, int fullValueRange, int totalRange){
        int lowerValueAmount = Mathf.RoundToInt((float)value / (totalRange - fullValueRange));
        GetXY(worlPosition, out int originX, out int originY);
        for (int x = 0; x < totalRange; x++){
            for (int y = 0; y < totalRange - x; y++){
                int radius = x + y;
                int addValueAmount = value;
                if (radius > fullValueRange){
                    addValueAmount -= lowerValueAmount * (radius - fullValueRange);
                }

                AddValue(originX + x, originY + y, addValueAmount); // right triangle

                if (x !=0){   // All 5 value sa triangle
                    AddValue(originX - x, originY + y, addValueAmount); // triangle
                }
                if (y != 0){
                    AddValue(originX + x, originY - y, addValueAmount); //diamond na lower part
                    if (x != 0){
                          AddValue(originX - x, originY - y, addValueAmount); //diamond na lower part
                    }
                }
            }
        }
    }








/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;


public class Grid <TGridObject>
{
    public const int HEAT_MAP_MAX_VALUE = 100; //max
    public const int HEAT_MAP_MIN_VALUE = 0; //minimum value for detection in grid

         public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
            public class OnGridValueChangedEventArgs : EventArgs {
        public int x;
        public int y;
    }

    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private TGridObject [,] gridArray; //multifunctional array
    private TextMesh[,] debugTextArray;


    public Grid (int width, int height, float cellSize, Vector3 originPosition){
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width,height]; //underlying array
        
        /*bool showDebug = false;
        if (showDebug) {
            TextMesh[,] debugTextArray = new TextMesh[width, height];

            for (int x = 0; x < gridArray.GetLength(0); x++) {
                for (int y = 0; y < gridArray.GetLength(1); y++) {
                    debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 30, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) => {
                debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y].ToString();
            };
        }
        debugTextArray = new TextMesh[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++){ //display function
            for (int y =0; y < gridArray.GetLength(1); y++)
            {
                debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y] .ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize)* .5f, 5, Color.white, TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 10f); 
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 10f);
            }   
        }
            Debug.DrawLine(GetWorldPosition(0, height),GetWorldPosition(width, height), Color.white, 10f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 10f);          
    }
        public int GetWidth(){
            return width;
        }

        public int GetHeight(){
            return height;
        }
    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3 (x, y) * cellSize + originPosition;
    }

    private void GetXY (Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    public void SetValue (int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < width && y <height)
        {
            gridArray[x, y] = value;                      //Mathf.Clamp(value, HEAT_MAP_MIN_VALUE, HEAT_MAP_MAX_VALUE); 
            debugTextArray[x, y].text = gridArray[x, y].ToString();
        }
    }

    public void SetValue(Vector3 worldPosition, TGridObject value) 
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public TGridObject GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y <height){
            return gridArray [x, y];
        } else {
            return default(TGridObject);
        } 
    }
        public TGridObject GetValue(Vector3 worldPosition) //reads the positions
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            return GetValue(x, y);
        }
}


    // Create text in the world (recommend; adding the code monkey utilities dahil takaw bug ang part na ito)
   /* public static TextMesh CreateWorldText (Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor){
    GameObject gameObject = new GameObject ("World_Text", typeof(TextMesh));
    Transform transform = gameObject.transform;
    transform.SetParent(parent, false);
    transform.localPosition = localPosition;
    TextMesh textMesh = gameObject.GetComponent <TextMesh>();
    textMesh.anchor = textAnchor;
    textMesh.alignment = textAlignment;
    textMesh.text = text;
    textMesh.fontSize = fontSize;
    textMesh.color = color;
    textMesh.GetComponent <MeshRenderer>().sortingOrder = sortingOrder;
    return textMesh;
    }*/

