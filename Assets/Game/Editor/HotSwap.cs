using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
public class HotSwap : EditorWindow
{
    
    
    GameObject newType;
  //  private List<GameObject> hasAlltheOldObjects = new List<GameObject>();
    //private List<GameObject> hasAlltheNewObjects = new List<GameObject>();
    [MenuItem("HaMiLeJa/Hotswap")]
    
    public static void ShowWindow()
    {
        GetWindow(typeof(HotSwap));
    }

    private void OnGUI()
    {
        GUILayout.Space(10);

        GUILayout.Label("!! NO UNDO !!  One Way Ticket BE CAREFUL !!", EditorStyles.boldLabel);
        GUILayout.Space(10);
        GUILayout.Label("HotSwap", EditorStyles.boldLabel);
        GUILayout.Label("Changes the selected object(s) to the choosen prefab", EditorStyles.helpBox);
        newType = EditorGUILayout.ObjectField("Will be swapped to: ", newType, typeof(GameObject), false) as GameObject;
        
        GUILayout.Space(5);

        if (GUILayout.Button("Replace"))
        {
            swapTheObjects();
        }
    //     if (GUILayout.Button("SwapBack"))
    //     {
    //         swapBack();
    //     }
     }

    /*void swapBack()
    {
        int counter = 0;
        foreach (GameObject changeMeBack in hasAlltheNewObjects)
        {
            GameObject newObject;
            newObject = changeMeBack;
            
            newObject.transform.position = hasAlltheOldObjects[counter].transform.position;
            newObject.transform.rotation = hasAlltheOldObjects[counter].transform.rotation;
            newObject.transform.parent = hasAlltheOldObjects[counter].transform.parent;
            String rename = hasAlltheOldObjects[counter].name;
            if (counter < hasAlltheOldObjects.Count)
            counter++;
            newObject.name = rename;
        }

        hasAlltheOldObjects.Clear();
        hasAlltheNewObjects.Clear();
    }*/
    
    void swapTheObjects()
    {
     
      //  hasAlltheOldObjects.Clear();
        
     //   foreach (GameObject go in Selection.gameObjects)
       // {
         //   hasAlltheOldObjects.Add(go);
     //   }
        
        foreach (GameObject swapMe in Selection.gameObjects)
        {
           
            
            GameObject newObject;
            newObject = (GameObject)PrefabUtility.InstantiatePrefab(newType);
            newObject.transform.position = swapMe.transform.position;
            newObject.transform.rotation = swapMe.transform.rotation;
            newObject.transform.parent = swapMe.transform.parent;
            String rename = swapMe.name;
            newObject.name = rename;
            // String name = "undo hotswap";
            DestroyImmediate(swapMe);
         //   hasAlltheNewObjects.Add(newObject);
        }
 
        
    }
}