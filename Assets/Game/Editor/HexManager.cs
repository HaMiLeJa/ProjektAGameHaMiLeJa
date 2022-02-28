using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;
public class HexManager: EditorWindow
{
    public  static Dictionary<int, Material> hasAllTheHexMaterials = new Dictionary<int, Material>();
    public  static Dictionary<int, int> hasAllTheAngles= new Dictionary<int, int>();
    private string hexTag = "Hex";
    private int 
        leftAngle = -60, rightAngle = 60,
        meshChildIndex = 0, meshGrandChildIndex = 0, propsChildIndex = 1,
        startAtMaterial = 1, stopAtMaterial, maxMaterials = 4;
    
    [MenuItem("HaMiLeJa/ Mange Hex")]
    public static void ShowWindow()
    {
        GetWindow(typeof(HexManager));
    }
    private void OnGUI()
    {
        GUILayout.Space(10);
        GUILayout.Label("Rotate Hex", EditorStyles.boldLabel);
        GUILayout.Label("Unbedingt Hex[x] parent bei allem auswählen, nicht in die Childs gehen. Kein Undo. Nutze die andere Richtung",
            EditorStyles.helpBox);
        GUILayout.Space(10);
        
        if (GUILayout.Button("Alles =>")) RotateRightEverything();
        GUILayout.Space(4);
        if (GUILayout.Button("<= Alles")) RotateLeftEverything();
        GUILayout.Space(8);
        if (GUILayout.Button("Boden =>")) RotateRightChild();
        GUILayout.Space(4);
        if (GUILayout.Button("<= Boden")) RotateLeftChild();
        GUILayout.Space(8);
        if (GUILayout.Button("Props =>")) RotateRightProps();
        GUILayout.Space(4);
        if (GUILayout.Button("<= Props")) RotateLeftProps();
        GUILayout.Space(15);
        GUILayout.Label("Materials", EditorStyles.boldLabel);
        GUILayout.Label("Zuerst must du die Materials (einmalig) reinladen", EditorStyles.helpBox);
        
        if (GUILayout.Button(">> Load Materials <<")) AddDic();
        GUILayout.Space(5);
        
        for (int i = 1; i <= maxMaterials; i++)
        {
            GUILayout.Space(6);
            if (GUILayout.Button("Set Material " + i))
                SetMaterial(i);
        }
        GUILayout.Space(15);
        GUILayout.Label("Randomize Material", EditorStyles.boldLabel);
        GUILayout.Label("Hier kannst du zwischen allen Materials randomisieren", EditorStyles.helpBox);
        if (GUILayout.Button("> Randomize Materials <")) RandomizeMaterials(1, maxMaterials);
        GUILayout.Space(5);
        GUILayout.Label("Bei dieser option kannst du ein start und End Material wählen", EditorStyles.helpBox);
        startAtMaterial = EditorGUILayout.IntSlider("Start at Material", startAtMaterial, 1, maxMaterials-1);
        stopAtMaterial = EditorGUILayout.IntSlider("Stop at Material", stopAtMaterial, 2, maxMaterials);
        if (GUILayout.Button("> Randomize between<"))
        {
            if (stopAtMaterial < startAtMaterial)
            {
                Debug.Log("StopValue kann nicht kleiner sein als StartValue"); return;
            }
            RandomizeMaterials(startAtMaterial, stopAtMaterial);
        }
        GUILayout.Space(8);
        GUILayout.Label("Vorher einmal(ig) auf Load Materials", EditorStyles.helpBox);
        if (GUILayout.Button("Randomize Ground Rotation")) RandomizeGround();
    }
    private void RandomizeGround()
    {
        foreach (GameObject rotateMe in Selection.gameObjects)
       {
           if (rotateMe.tag == hexTag)
           { 
               int randomAngle = Random.Range(1, 6), value = hasAllTheAngles[randomAngle];
               GameObject childWithMeshRnd = rotateMe.transform.GetChild(meshChildIndex).gameObject;
               Rotator(childWithMeshRnd, value);
           }
       }
    }
    private void RotateRightChild()
    {
        foreach (GameObject rotateMe in Selection.gameObjects)
        {
            if (rotateMe.tag == hexTag)
            {
                GameObject childWithMeshRnd = rotateMe.transform.GetChild(meshChildIndex).gameObject;
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
                GameObject childWithMeshRnd = rotateMe.transform.GetChild(meshChildIndex).gameObject;
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
    private void RotateRightProps()
    {
        foreach (GameObject rotateMe in Selection.gameObjects)
        {
            if (rotateMe.tag == hexTag)
            {
                GameObject childWithMeshRnd = rotateMe.transform.GetChild(propsChildIndex).gameObject;
                Rotator(childWithMeshRnd, rightAngle);
            }
        }
    }
    private void RotateLeftProps()
    {
        foreach (GameObject rotateMe in Selection.gameObjects)
        {
            if (rotateMe.tag == hexTag)
            {
                GameObject childWithMeshRnd = rotateMe.transform.GetChild(propsChildIndex).gameObject;
                Rotator(childWithMeshRnd, leftAngle );
            }
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
            GameObject childWithMeshRnd = replaceMyMat.transform.
                GetChild(meshChildIndex).GetChild(meshGrandChildIndex).gameObject;
            MeshRenderer rnd = childWithMeshRnd.GetComponent<MeshRenderer>();
            Material value = hasAllTheHexMaterials[materialID];
            rnd.material = value;
        }
    }
    private void AddDic()
    {
        hasAllTheHexMaterials.Clear();
        hasAllTheAngles.Clear();
        for (int i = 1; i <= maxMaterials; i++)
        {
            Material newMat = Resources.Load("HexMaterial " + i, typeof(Material)) as Material;
            hasAllTheHexMaterials.Add(i,newMat);
        }
        Debug.Log("Es wurden [" + maxMaterials + "] HexMaterials geladen");
        for (int i = 1; i <= 7; i++)
            hasAllTheAngles.Add(i, leftAngle*i);
    }
    private void RandomizeMaterials(int minMatIndex, int MaxMatIndex)
    {
        foreach (GameObject replaceMyMat in Selection.gameObjects)
        {
            int materialID = Random.Range(minMatIndex, MaxMatIndex+1);
            Debug.Log(materialID);
            GameObject childWithMeshRnd = replaceMyMat.transform.GetChild(meshChildIndex).GetChild(meshGrandChildIndex).gameObject;
            MeshRenderer rnd = childWithMeshRnd.GetComponent<MeshRenderer>();
            Material value = hasAllTheHexMaterials[materialID];
            rnd.material = value;
        }
    }
}