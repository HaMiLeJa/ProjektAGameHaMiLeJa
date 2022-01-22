using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
public class HexManager: EditorWindow
{
    public  static Dictionary<int, Material> hasAllTheHexMaterials = new Dictionary<int, Material>();
    private int maxMaterials = 4;
    private string hexTag = "Hex";
    private int leftAngle = -60;
    private int rightAngle = 60;
    private int childIndex = 0;
    private int grandChildIndex = 0;
    
    [MenuItem("HaMiLeJa/ Mange Hex")]
    public static void ShowWindow()
    {
        GetWindow(typeof(HexManager));
    }
    private void OnGUI()
    {
        GUILayout.Space(10);

        GUILayout.Label("Rotate Hex", EditorStyles.boldLabel);
        GUILayout.Label("Unbedingt Hex[x] parent bei allem auswählen, nicht in die Childs gehen", EditorStyles.helpBox);
        GUILayout.Space(10);
        if (GUILayout.Button("Alles =>"))
        {
            RotateRightEverything();
        }
        GUILayout.Space(10);
        if (GUILayout.Button("<= Alles"))
        {
            RotateLeftEverything();
        }
        GUILayout.Space(10);
        if (GUILayout.Button("Boden =>"))
        {
            RotateRightChild();
        }
        GUILayout.Space(10);
        if (GUILayout.Button("<= Boden"))
        {
            RotateLeftChild();
        }
        GUILayout.Space(20);
        GUILayout.Label("Materials", EditorStyles.boldLabel);
        GUILayout.Label("Zuerst must du die Materials (einmalig) reinladen", EditorStyles.helpBox);
        if (GUILayout.Button(">> Load Materials <<"))
        {
            AddDic();
        }
        GUILayout.Space(3);
     
        for (int i = 1; i <= maxMaterials; i++)
        {
            GUILayout.Space(8);
            if (GUILayout.Button("Set Material " + i.ToString()))
            {
                
                SetMaterial(i);
            }
        }
    }
    private void RotateRightChild()
    {
        foreach (GameObject rotateMe in Selection.gameObjects)
        {
            if (rotateMe.tag == hexTag)
            {
                GameObject childWithMeshRnd = rotateMe.transform.GetChild(childIndex).gameObject;
                Rotator(childWithMeshRnd, rightAngle);
            }
        }
    }
    
    private void RotateLeftChild()
    {
        foreach (GameObject rotateMe in Selection.gameObjects)
        {
            if (rotateMe.tag == hexTag)
            {
                GameObject childWithMeshRnd = rotateMe.transform.GetChild(childIndex).gameObject;
                Rotator(childWithMeshRnd, leftAngle );
            }
        }
    }

    private void RotateRightEverything()
    {
        foreach (GameObject rotateMe in Selection.gameObjects)
        {
            if(rotateMe.tag == hexTag)
            Rotator(rotateMe , rightAngle);
        }
    }
    private void RotateLeftEverything()
    {
        foreach (GameObject rotateMe in Selection.gameObjects)
        {
            if(rotateMe.tag == hexTag)
            Rotator(rotateMe, leftAngle );
        }
    }
    private void Rotator(GameObject gameObjectToRotate, int angle)
    {
        gameObjectToRotate.transform.Rotate(0,angle,0);
    }
    private void SetMaterial(int materialID)
    {
        foreach (GameObject replaceMyMat in Selection.gameObjects)
        {
            GameObject childWithMeshRnd = replaceMyMat.transform.GetChild(childIndex).GetChild(grandChildIndex).gameObject;
            MeshRenderer rnd = childWithMeshRnd.GetComponent<MeshRenderer>();
            Material value = hasAllTheHexMaterials[materialID];
            rnd.material = value;
        }
    }
    
    private void AddDic()
    {
        hasAllTheHexMaterials.Clear();
        for (int i = 1; i <= maxMaterials; i++)
        {
            Material newMat = Resources.Load("HexMaterial " + i.ToString(), typeof(Material)) as Material;
            hasAllTheHexMaterials.Add(i,newMat);
        }
        Debug.Log("Es wurden [" + maxMaterials.ToString() + "] HexMaterials geladen");
    }
}

    
