using System.Collections.Generic;
using UnityEngine;
public class Highlightmanager : MonoBehaviour
{
    public static readonly List<Material> GlowMaterials = new List<Material>();
    [SerializeField] private List<Material> GlowMaterialList = new List<Material>();
    private void Awake()
    {
        GlowMaterials.Clear();
        foreach (Material mat in GlowMaterialList)
        {
            GlowMaterials.Add(mat);
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)) Debug.Log(GlowMaterials.Count);
    }
}