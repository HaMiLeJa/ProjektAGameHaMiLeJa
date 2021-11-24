using System;
using UnityEngine;
using System.Collections;
using UnityEditor;

public static class  NewParent 
{
    private const string UNDO_STR_SNAP = "transform Objects";

    [MenuItem("HaMiLeYa/Parent Objects t%&Y", isValidateFunction: true)]
    public static bool TransformSValidate()
    {
        return Selection.gameObjects.Length > 0;
    }
    
    [MenuItem("HaMiLeYa/Parent Objects %&Y")]
    public static void ParrentTheObjects()
    {
        foreach (GameObject go in Selection.gameObjects)

        {
            LevelObj = go;
            parenting();
        }

    }
    [MenuItem("HaMiLeYa/UnParent Objects t%&X", isValidateFunction: true)]
    public static bool TransformValidate()
    {
        return Selection.gameObjects.Length > 0;
    }
    
    [MenuItem("HaMiLeYa/UnParent Objects %&X")]

    public static void UnParentAll()
    {
        foreach (GameObject go in Selection.gameObjects)
            
        {
            go.transform.parent = null;
        }
      
    }

    private static GameObject LevelObj;
    private static GameObject Hex;
    private static GameObject Props;
    
    public static void parenting ( )
    {
        HexCheck();
    SetParent(Hex);
    SetParent(Props);
    }
    public static void  SetParent(GameObject newParent)
    {
        
        LevelObj.transform.parent = newParent.transform;
        if (newParent.transform.parent != null)
        {
            Debug.Log("Player's Grand parent: " + LevelObj.transform.parent.parent.name);
        }
    }
    
    static void   HexCheck()
    {
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(LevelObj.transform.position, -LevelObj.transform.up, out hit, 1000, LayerMask.GetMask("Hex"))) //LayerMask.GetMask("Hex")
        {
            Debug.Log("Raycast hit me");
            Hex = hit.transform.gameObject;
            Props = Hex.transform.GetChild(1).gameObject;
        }
    }
    
}


