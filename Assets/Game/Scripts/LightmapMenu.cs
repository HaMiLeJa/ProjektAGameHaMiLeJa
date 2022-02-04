using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LightmapMenu : MonoBehaviour
{
    private static float boostEmissive = 35;
    
    [MenuItem ("HaMiLeJa/Bake")]

    static void Bake ()

    {
        List<GameObject> hasAllTheEmmisiveObjects = new List<GameObject>();
        Dictionary<Material, float> hasAllTheOrginalEmmissiveIntensity = new Dictionary<Material, float>();
        List<Material> hasAlltheModifiedGameObjects= new List<Material>();
        hasAllTheEmmisiveObjects.Clear();
        hasAllTheOrginalEmmissiveIntensity.Clear();
        hasAllTheEmmisiveObjects.AddRange(GameObject.FindGameObjectsWithTag("Emissve_baked"));
        
        foreach (GameObject tmpObj in hasAllTheEmmisiveObjects)
        {
            Material tmpMaterial = tmpObj.GetComponent<Renderer> ().sharedMaterial;
            tmpMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.BakedEmissive;
            float originalEmissiveIntensity = 0;
            try
            {
                if (hasAllTheOrginalEmmissiveIntensity.ContainsKey(tmpMaterial))
                    ;
                else
                {    originalEmissiveIntensity = tmpMaterial.GetFloat("_emssiveIntesnity");
                    Debug.Log(originalEmissiveIntensity + "orginal");
                    hasAllTheOrginalEmmissiveIntensity.Add(tmpMaterial, originalEmissiveIntensity);
                    hasAlltheModifiedGameObjects.Add(tmpMaterial);
                    tmpMaterial.SetFloat("_emssiveIntesnity", originalEmissiveIntensity+boostEmissive);
                }
            }
            catch (NullReferenceException e)
            {
                
            }
        }
        Lightmapping.Bake ();
        foreach (Material tmpMaterial in hasAlltheModifiedGameObjects)
        {
            float value = 0;
            if (hasAllTheOrginalEmmissiveIntensity.TryGetValue(tmpMaterial , out value))
            {
                tmpMaterial.SetFloat("_emssiveIntesnity", value);
                Debug.Log(value + "value");
            }
            Debug.Log("yay");

        }
        
    }

}