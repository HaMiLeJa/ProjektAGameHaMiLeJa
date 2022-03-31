using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
public class RenderBounds : EditorWindow
{ 
    public static readonly HashSet<MeshFilter> RenderBoundsList = new HashSet<MeshFilter>();
    private  static readonly HashSet<MeshFilter> hasAllBoundsToModify = new HashSet<MeshFilter>();
    private static byte propsChildIndex = 1;
    private static float leftBoundMult = 0, rightBoundMult = 11, multiplicationValue = 1;
    private static readonly string ignoreLayer = "Hex";
    [MenuItem("HaMiLeJa/ RenderBounds")]
    public static void ShowWindow()
    {
        GetWindow(typeof(RenderBounds));
    }
    private void OnGUI()
    {  
        GUILayout.Space(10);
        if (GUILayout.Button("Draw Bounds on Selected")) AddSelectedToList();
        GUILayout.Space(4);
        if (GUILayout.Button("Draw Bounds Ignore " + ignoreLayer + " Selected")) AddSelectedToListIgnoreHex();
        GUILayout.Space(4);
        if (GUILayout.Button("Clear Bounds")) {RenderBoundsList.Clear(); Debug.Log("Everything cleared!!");}
        GUILayout.Space(4);
        if (GUILayout.Button("Change Bounds Size")) changeBoundsSize();
        multiplicationValue = EditorGUILayout.Slider("Multiplicationvalue", multiplicationValue, leftBoundMult, rightBoundMult);
        GUILayout.Space(4);
        if (GUILayout.Button("Recalculate Bounds")) recalculateBoundsSize();
        GUILayout.Space(18);
        if (GUILayout.Button("Draw All Ignore " + ignoreLayer + " Selected")) AddAllGameObjects();
        GUILayout.Space(4);
    }
    void AddSelectedToList()
    {
        RenderBoundsList.Clear();
        foreach (GameObject showMe in Selection.gameObjects)
        {
            if (showMe.transform.GetComponent<MeshFilter>() != null) RenderBoundsList.Add(showMe.transform.GetComponent<MeshFilter>());
            if (showMe.transform.childCount > 0) foreach (MeshFilter mf in showMe.transform.GetComponentsInChildren<MeshFilter>()) RenderBoundsList.Add(mf);
        }
        Debug.Log("You see the Render bounds of : " + RenderBoundsList.Count + " Meshfilters");
    }
    void AddSelectedToListIgnoreHex()
    {
        RenderBoundsList.Clear();
        foreach (GameObject showMe in Selection.gameObjects)
        {
            if (showMe.CompareTag("Hex"))
                foreach (MeshFilter mf in showMe.transform.GetChild(1).GetComponentsInChildren<MeshFilter>()) RenderBoundsList.Add(mf);
            if (!showMe.CompareTag("Hex"))
            {
                if (showMe.transform.GetComponent<MeshFilter>() != null)
                    RenderBoundsList.Add(showMe.transform.GetComponent<MeshFilter>());
                if (showMe.transform.childCount > 0) foreach (MeshFilter mf in showMe.transform.GetComponentsInChildren<MeshFilter>()) RenderBoundsList.Add(mf);
            }
        }
        Debug.Log("You see the Render bounds of : " + RenderBoundsList.Count + " Meshfilters");
    }
    void AddAllGameObjects()
    {
        HexAutoTiling hex = FindObjectOfType<HexAutoTiling>();
        foreach (Transform hasHexTransform in hex.hasAllTheHexGameObjectsTransformsBeforeStart)
        {
            if (hasHexTransform.GetChild(1).childCount > 0)
                foreach (MeshFilter mf in hasHexTransform.GetChild(1).GetComponentsInChildren<MeshFilter>())
                {
                    if (mf != null) RenderBoundsList.Add(mf);
                }
        }
        Debug.Log("You see the Render bounds of : " + RenderBoundsList.Count + " Meshfilters");
    }
    void changeBoundsSize()
    {
        foreach (GameObject showMe in Selection.gameObjects)
        {
            if (showMe.CompareTag("Hex"))
            
                foreach (MeshFilter mf in showMe.transform.GetChild(1).GetComponentsInChildren<MeshFilter>()) hasAllBoundsToModify.Add(mf);
            
            if (!showMe.CompareTag("Hex"))
            {
                if (showMe.transform.GetComponent<MeshFilter>() != null) RenderBoundsList.Add(showMe.transform.GetComponent<MeshFilter>());
                if (showMe.transform.childCount > 0) foreach (MeshFilter mf in showMe.transform.GetComponentsInChildren<MeshFilter>()) hasAllBoundsToModify.Add(mf);
            }
        }
        Debug.Log("You see the Render bounds of : " + hasAllBoundsToModify.Count + " Meshfilters");
        foreach (MeshFilter mf in hasAllBoundsToModify)
        {
            mf.sharedMesh.RecalculateBounds();
            mf.sharedMesh.bounds = new Bounds(mf.sharedMesh.bounds.center, mf.sharedMesh.bounds.size * multiplicationValue);
        }
    }
    void recalculateBoundsSize()
    {
        foreach (GameObject showMe in Selection.gameObjects)
        {
            if (showMe.CompareTag("Hex")) 
                foreach (MeshFilter mf in showMe.transform.GetChild(1).GetComponentsInChildren<MeshFilter>()) hasAllBoundsToModify.Add(mf);
            if (!showMe.CompareTag("Hex"))
            {
                if (showMe.transform.GetComponent<MeshFilter>() != null) RenderBoundsList.Add(showMe.transform.GetComponent<MeshFilter>());
                if (showMe.transform.childCount > 0) foreach (MeshFilter mf in showMe.transform.GetComponentsInChildren<MeshFilter>()) hasAllBoundsToModify.Add(mf);
            }
        }
        Debug.Log("Recalculating the Render bounds of : " + RenderBoundsList.Count + " Meshfilters");
        foreach (MeshFilter mf in hasAllBoundsToModify) mf.sharedMesh.RecalculateBounds();
    }
}