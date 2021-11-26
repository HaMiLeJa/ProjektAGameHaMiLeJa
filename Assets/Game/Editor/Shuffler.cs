using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class Shuffler : EditorWindow
{
    private GameObject hexGrid;
    private bool pivotHasMoved = false;
    private bool shuffledRight = true;
    List<GameObject> hasAllTheHexes = new List<GameObject>();

    [MenuItem("HaMiLeJa/ Shuffler")]
    public static void ShowWindow()
    {
        GetWindow(typeof(Shuffler));
    }

    private void OnGUI()
    {
        GUILayout.Space(10);

        GUILayout.Label("Shuffler", EditorStyles.boldLabel);

        hexGrid = EditorGUILayout.ObjectField("Pivot Object: ", hexGrid, typeof(GameObject), true) as GameObject;
        
        GUILayout.Space(5);
        if (GUILayout.Button("Clear List"))
        {
            clearList();
        }
        GUILayout.Space(5);
        
        if (GUILayout.Button("Make List"))
        {
            safeHexesToList();
        }

       

     
        
        GUILayout.Space(15);

      
        if (GUILayout.Button("Shuffle right"))
        {
            moveToTheRight();
        }

        //GUI.enabled = shuffledRight;
        
        GUILayout.Space(5);

        if (GUILayout.Button("Shuffle left"))
        {
            moveToTheLeft();
        }

       // GUI.enabled = true;
        
        GUILayout.Space(15);
        
        if (GUILayout.Button("Bottom"))
        {
            moveToTheBottom();
        }
        if (GUILayout.Button("Top"))
        {
            moveToTheTop();
        }

        void safeHexesToList()
        {
            hasAllTheHexes.AddRange(GameObject.FindGameObjectsWithTag("Hex"));
           
        }

        void clearList()
        {
            hasAllTheHexes.Clear();
        }

      
        void moveToTheTop()
        {
            foreach (GameObject hex in hasAllTheHexes)
            {
                if (hex.transform.position.z < hexGrid.transform.position.z)
                    hex.transform.position = new Vector3(hex.transform.position.x,
                        hex.transform.position.y, hex.transform.position.z + HexAutoTiling.zTilingDistance);
                if (pivotHasMoved == false)
                    pivotHasMoved = true;
            }

            if (pivotHasMoved)
            {
                hexGrid.transform.position =
                    new Vector3(hexGrid.transform.position.x,
                        hexGrid.transform.position.y,
                        hexGrid.transform.position.z + (HexAutoTiling.zTilingDistance / 2));
                pivotHasMoved = false;
            }
        }
        
        
        
        void moveToTheBottom()
        {
            foreach (GameObject hex in hasAllTheHexes)
            {
                if (hex.transform.position.z > hexGrid.transform.position.z)
                    hex.transform.position = new Vector3(hex.transform.position.x,
                        hex.transform.position.y, hex.transform.position.z - HexAutoTiling.zTilingDistance);
                if (pivotHasMoved == false)
                    pivotHasMoved = true;
            }

            if (pivotHasMoved)
            {
                hexGrid.transform.position =
                    new Vector3(hexGrid.transform.position.x,
                        hexGrid.transform.position.y,
                        hexGrid.transform.position.z - (HexAutoTiling.zTilingDistance / 2));
                pivotHasMoved = false;
            }
        }

        
        
        void moveToTheLeft()
        {
          
            foreach (GameObject hex in hasAllTheHexes)
            {
                if (hex.transform.position.x > hexGrid.transform.position.x)
                    hex.transform.position = new Vector3(hex.transform.position.x - HexAutoTiling.xTilingDistance,
                        hex.transform.position.y, hex.transform.position.z);
                if (pivotHasMoved == false)
                    pivotHasMoved = true;
            }

            if (pivotHasMoved)
            {
                hexGrid.transform.position =
                    new Vector3(hexGrid.transform.position.x - (HexAutoTiling.xTilingDistance / 2),
                        hexGrid.transform.position.y, hexGrid.transform.position.z);
                pivotHasMoved = false;
            }
        }
        

        void moveToTheRight()
        {
           
                foreach (GameObject hex in hasAllTheHexes)
            {

                if (hex.transform.position.x < hexGrid.transform.position.x)
                    hex.transform.position = new Vector3(hex.transform.position.x + HexAutoTiling.xTilingDistance,
                        hex.transform.position.y, hex.transform.position.z);
            
                 if (pivotHasMoved == false)
                    pivotHasMoved = true;
            
            }
            
             if (pivotHasMoved)
            {
                hexGrid.transform.position = new Vector3(hexGrid.transform.position.x + (HexAutoTiling.xTilingDistance/2),
                    hexGrid.transform.position.y, hexGrid.transform.position.z);
                pivotHasMoved = false;
            }
        }
     
      
    }
}