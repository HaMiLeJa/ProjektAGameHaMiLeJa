using System;
using UnityEngine;
using System.Collections;
using log4net.Core;
using UnityEditor;

public static class  NewParent 
{

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
            Hex = null;
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
            Debug.Log("Das Objekt ["+ go +"] wurde unparented");
        }
      
    }

    private static GameObject LevelObj;
    private static GameObject Hex;
    private static GameObject Props;
    
    public static void parenting ( )
    {
        
        HexCheck();
        if (Hex == null)
        {
            Debug.Log("Beweg mal das Objekt höher oder überhaupt in die Nähe von Hexes");
            return;
        }
        SetParent(Hex);
        SetParent(Props);

    }
    public static void  SetParent(GameObject newParent)
    {
        LevelObj.transform.parent = newParent.transform;
        if (newParent.transform.parent != null)
        {
            Debug.Log("Das Objekt ["+ LevelObj +"] wurde in [Props] unter >>> " + LevelObj.transform.parent.parent.name +" <<< platziert");
        }
    }
    
    static void   HexCheck()
    {
        RaycastHit hit = new RaycastHit();
        float sphereCastRadius = 0.1f;
        float sphereCastRadiusStop = 500;
        float sphereCastRadiusDown = sphereCastRadius;
        float sphereCastRadiusUp = sphereCastRadius;
        
        while (Hex == null && sphereCastRadiusDown <sphereCastRadiusStop)
        {
            if (Physics.SphereCast(LevelObj.transform.position, sphereCastRadiusDown,-LevelObj.transform.up, out hit, 1000, LayerMask.GetMask("Hex"))) //LayerMask.GetMask("Hex")
        {
            Hex = hit.transform.gameObject;
            Props = Hex.transform.GetChild(1).gameObject;
        }
            sphereCastRadiusDown++;
        }

        while (Hex == null && sphereCastRadiusUp < sphereCastRadiusStop)
        {
            if (Physics.SphereCast(LevelObj.transform.position, sphereCastRadiusUp, LevelObj.transform.up, out hit, 1000,
                LayerMask.GetMask("Hex"))) 
            {
                Hex = hit.transform.gameObject;
                Props = Hex.transform.GetChild(1).gameObject;
            }

            sphereCastRadiusUp++;
        }
    }
}


