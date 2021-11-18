using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Snapper
{
    private const string UNDO_STR_SNAP = "snap Objects";

    [MenuItem("Edit/Snap Selected Object %&S", isValidateFunction: true)]
    public static bool SnapTheThingsValidate()
    {
        return Selection.gameObjects.Length > 0;
    }
    
    [MenuItem("Edit/Snap Selected Object %&S")]
    public static void SnapTheThings()
    {
        foreach (GameObject go in Selection.gameObjects)
            //expensive to use 
        {Undo.RecordObject(go.transform, UNDO_STR_SNAP);
            go.transform.position = go.transform.position.Round();
               
        }

    }
    
    public static Vector3 Round(this Vector3 v)
    {
        v.x = Mathf.FloorToInt(v.x);
        v.y = Mathf.Round(v.y);
        v.z = Mathf.Round(v.z);
        return v;
    }
}
