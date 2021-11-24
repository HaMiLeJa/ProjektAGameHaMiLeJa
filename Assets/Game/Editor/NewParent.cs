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
        if (Physics.SphereCast(LevelObj.transform.position, 5, -LevelObj.transform.up, out hit, 100000, LayerMask.GetMask("Hex"))) //LayerMask.GetMask("Hex")
        {   
            Hex = hit.transform.gameObject;
            /*
            #region  Need Bugfix
            
           float angle = 60;
           // angle = angle * Mathf.Deg2Rad;
            //Vector3 dir = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
            
            Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;

            if (Hex == null) {
                if (Physics.Raycast(LevelObj.transform.position, -LevelObj.transform.up*dir.x, out hit, 100000,
                    LayerMask.GetMask("Hex")))
                {
                    
                    Hex = hit.transform.gameObject;
                } }
            
            if (Hex == null) {
                if (Physics.Raycast(LevelObj.transform.position, -LevelObj.transform.up*-dir.x, out hit, 100000,
                    LayerMask.GetMask("Hex")))
                { 
                    Hex = hit.transform.gameObject;
                } }
            
            if (Hex == null) {
                if (Physics.Raycast(LevelObj.transform.position, -LevelObj.transform.up*dir.z, out hit, 100000,
                    LayerMask.GetMask("Hex")))
                { 
                    Hex = hit.transform.gameObject;
                } }
            
            if (Hex == null) {
                if (Physics.Raycast(LevelObj.transform.position, -LevelObj.transform.up*-dir.z, out hit, 100000,
                    LayerMask.GetMask("Hex")))
                { 
                    Hex = hit.transform.gameObject;
                } }
            
            #endregion
            */
            
            Props = Hex.transform.GetChild(1).gameObject;
        }
    }
}


