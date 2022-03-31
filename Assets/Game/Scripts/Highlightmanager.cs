using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;
public class Highlightmanager : MonoBehaviour
{ 
    [BoxGroup("Delay Settings")]  [SerializeField][Range(0, 1)] float SetGlowEnableDelayHex =   0.05f ;
    [BoxGroup("Delay Settings")]  [SerializeField] [Range(0,10)]float SetGlowDisableDelayHex = 2.5f ;
    [BoxGroup("Delay Settings")]  [SerializeField] [Range(0, 1)] float SetGlowEnableDelayObjects =  0.1f ;
    [BoxGroup("Delay Settings")]  [SerializeField] [Range(1, 10)] float SetGlowDisableDelayObjects = 4f;
    public static float GlowEnableDelayObjects, GlowDisableDelayObjects, GlowEnableDelayHex, GlowDisableDelayHex;
    public static Material[] glowMaterialsStatic;
    private static readonly HashSet<Material> hasAllTheUniqueMaterialsHashSet = new HashSet<Material>();
    private static Material[] hasAllTheUniqueMaterials;
    private static  readonly List<GameObject> HexObjects = new List<GameObject>();
    private static  readonly List<GameObject> HexObjectsWithRenderer = new List<GameObject>();
    private static RendererMatIndex[] rendMatIndexStatic;
    [InfoBox("Hier alle Glowmaterials eintragen die man haben möchte für das Level. Einfach in die Glow Material List eintragen. Rend Mat Index und Materials Used bitte nur durch den Button updaten, und das ab und zu.", EInfoBoxType.Warning)]
    [BoxGroup("Glow Materials")]  [SerializeField] public  Material[] GlowMaterialList;
    [BoxGroup("Serialized Lists")] [SerializeField] public RendererMatIndex[] RendMatIndex;
    [BoxGroup("Serialized Lists")] [SerializeField] public Material[] MaterialsUsed;
    private void Awake()
    {
        Array.Resize(ref glowMaterialsStatic,GlowMaterialList.Length);
        glowMaterialsStatic = GlowMaterialList;
        updateStaticValues();
    }
    public static void GlowHighlight(ushort matSwapIndex, byte HighlightType)
    {
        for (int i = 0; i < rendMatIndexStatic[matSwapIndex].rendererList.Length; i++)
            rendMatIndexStatic[matSwapIndex].rendererList[i].material = glowMaterialsStatic[HighlightType];
    }
    public static void DisableGlowHighlight(ushort matSwapIndex)
    {
        
        for (int i = 0; i < rendMatIndexStatic[matSwapIndex].rendererList.Length; i++)
            rendMatIndexStatic[matSwapIndex].rendererList[i].material = hasAllTheUniqueMaterials[rendMatIndexStatic[matSwapIndex].materialIndexNumber[i]];
    }
    
    void updateStaticValues()
    {
        hasAllTheUniqueMaterials = MaterialsUsed;
        rendMatIndexStatic = RendMatIndex;
        MaterialsUsed = null; RendMatIndex = null; 
        GlowEnableDelayObjects = SetGlowEnableDelayObjects;
        GlowDisableDelayObjects = SetGlowDisableDelayObjects;
        GlowEnableDelayHex = SetGlowEnableDelayHex;
        GlowDisableDelayHex = SetGlowDisableDelayHex;
    }
#if UNITY_EDITOR
    [Button] public void UpdateAllMaterialIndexies()
    {
        if (Application.isPlaying) return;
        clearAllContainer();
        populateHexObjectLists();
        pupulateUniqueMaterials();
        setArraySizes();
        populateSerializedPropertys();
    }
    private void clearAllContainer()
    {
       if(hasAllTheUniqueMaterialsHashSet !=null)      hasAllTheUniqueMaterialsHashSet.Clear();
       if(HexObjects !=null)                          HexObjects.Clear();
       if(HexObjectsWithRenderer !=null)            HexObjectsWithRenderer.Clear();
       if(RendMatIndex != null) Array.Clear(RendMatIndex,0,RendMatIndex.Length);
    }
    private void populateHexObjectLists()
    {
        foreach (GameObject hex in GameObject.FindGameObjectsWithTag("Hex")) HexObjects.Add(hex);
        foreach (GameObject hex in HexObjects)
        {   
            int rendererCount = hex.transform.GetChild(1).GetComponentsInChildren<Renderer>().Length;
            int isValidCount = hex.transform.GetChild(1).GetComponentsInChildren<HighlightObjects>().Length;
            if (rendererCount > 0 && isValidCount > 0) HexObjectsWithRenderer.Add(hex);
        }
    }
    private void pupulateUniqueMaterials()
    {
        foreach (GameObject hex in HexObjects)
        {
            for (int i = 0; i < hex.transform.GetChild(1).childCount; i++)
                if (hex.transform.GetChild(1).GetChild(i).GetComponent<HighlightObjects>() != null)
                {
                    foreach (Renderer rend in hex.transform.GetChild(1).GetComponentsInChildren<Renderer>())
                    {
                        if(rend.sharedMaterial !=null) hasAllTheUniqueMaterialsHashSet.Add(rend.sharedMaterial);
                    }
                }
        }
        hasAllTheUniqueMaterials = hasAllTheUniqueMaterialsHashSet.ToArray();
        MaterialsUsed = hasAllTheUniqueMaterials;
    }
    private void setArraySizes()
    {
        Array.Resize(ref RendMatIndex, HexObjectsWithRenderer.Count+1);
        int rendMatIndexNumber = 1;
        foreach (GameObject hex in HexObjectsWithRenderer)
        {
            int arrayLength = hex.transform.GetChild(1).GetComponentsInChildren<Renderer>().Length;
            foreach (Renderer renderer in hex.transform.GetChild(1).GetComponentsInChildren<Renderer>())
            {
                if (renderer.sharedMaterial == null) arrayLength--;
            }
            Array.Resize(ref RendMatIndex[rendMatIndexNumber].materialIndexNumber, arrayLength);
            Array.Resize(ref RendMatIndex[rendMatIndexNumber].rendererList, arrayLength);
            rendMatIndexNumber++;
        }
    }
    private void populateSerializedPropertys()
    {
        SerializedObject SerializedHexObject;
        SerializedObject SerializedObject = new SerializedObject(this );
        SerializedProperty rendMatIndexProp = SerializedObject.FindProperty("RendMatIndex");
        int rendererIndexNumber = 1;
        foreach (GameObject hex in HexObjectsWithRenderer)
        {
            foreach (HighlightObjects hiOBJ in hex.transform.GetChild(1).GetComponentsInChildren<HighlightObjects>())
            {
                SerializedHexObject = new SerializedObject(hiOBJ);
                SerializedHexObject.FindProperty("matSwapIndex").intValue = rendererIndexNumber;
                SerializedHexObject.ApplyModifiedPropertiesWithoutUndo();
            }
            int countIndexInnerArrays = 0;
            for (ushort i = 0; i < hasAllTheUniqueMaterials.Length; i++)
                foreach (Renderer rend in hex.transform.GetChild(1).GetComponentsInChildren<Renderer>())
                {
                    if (rend.gameObject.GetComponent<HighlightObjectsOwnGlow>() != null) continue;
                    if (hasAllTheUniqueMaterials[i] == rend.sharedMaterial)
                    {
                        rendMatIndexProp.GetArrayElementAtIndex(rendererIndexNumber)
                            .FindPropertyRelative("materialIndexNumber").GetArrayElementAtIndex(countIndexInnerArrays).intValue = i;
                         rendMatIndexProp .GetArrayElementAtIndex(rendererIndexNumber)
                           .FindPropertyRelative("rendererList").GetArrayElementAtIndex(countIndexInnerArrays).objectReferenceValue = rend;
                        countIndexInnerArrays++;
                    }
                }
            rendererIndexNumber++;
        }
        SerializedObject.ApplyModifiedPropertiesWithoutUndo();
    }
#endif
}
