using System.Collections.Generic;
using UnityEngine;
public class GlowHighlight : MonoBehaviour
{
    #region Dictonarys
    Dictionary<Renderer, Material[]> glowMaterialDictionaryHexes = new Dictionary<Renderer, Material[]>();
    Dictionary<Renderer, Material[]> originalMaterialDictionaryHexes = new Dictionary<Renderer, Material[]>();
    Dictionary<Renderer, Material[]> glowMaterialDictionaryProps = new Dictionary<Renderer, Material[]>();
    Dictionary<Renderer, Material[]> originalMaterialDictionaryProps = new Dictionary<Renderer, Material[]>();
    Dictionary<Color, Material> cachedGlowMaterials = new Dictionary<Color, Material>();
    #endregion
    
    #region Inspector
    public Material glowMaterial;
    private bool isGlowing;
    private Color validSpaceColor = Color.green;
    private Color originalGlowColor;
    #endregion
    
    #region MaterialSwap
    private void Awake()
    {
        PrepareMaterialDictionaries();
        originalGlowColor = glowMaterial.GetColor("_GlowColor");
    }
    private void PrepareMaterialDictionaries()
    {
        if (transform.childCount > 0)
        {
            foreach (Renderer renderer in transform.GetChild(0).GetComponentsInChildren<Renderer>())
          {
              Material[] originalMaterials = renderer.materials;
                originalMaterialDictionaryHexes.Add(renderer, originalMaterials);
                Material[] newMaterials = new Material[renderer.materials.Length];
                for (int i = 0; i < originalMaterials.Length; i++)
                {
                    Material mat;
                    if (cachedGlowMaterials.TryGetValue(originalMaterials[i].color, out mat) == false)
                    {
                        mat = new Material(glowMaterial);

                        if (mat.color == null) continue;
                        mat.color = originalMaterials[i].color;
                        cachedGlowMaterials[mat.color] = mat;
                    }
                    newMaterials[i] = mat;
                }
                glowMaterialDictionaryHexes.Add(renderer, newMaterials);
           }
         }
        foreach (Renderer renderer in transform.GetChild(1).GetComponentsInChildren<Renderer>())
            {
                Material[] originalMaterials = renderer.materials;
                originalMaterialDictionaryProps.Add(renderer, originalMaterials);
                Material[] newMaterials = new Material[renderer.materials.Length];

                for (int i = 0; i < originalMaterials.Length; i++)
                {
                    Material mat = null;
                    if (cachedGlowMaterials.TryGetValue(originalMaterials[i].color, out mat) == false)
                    {
                        mat = new Material(glowMaterial);

                        if (mat.color == null) continue;
                        mat.color = originalMaterials[i].color;
                        cachedGlowMaterials[mat.color] = mat;
                    }
                    newMaterials[i] = mat;
                }
                glowMaterialDictionaryProps.Add(renderer, newMaterials);
            }
    }
    #endregion

    #region PathHighlight
    
    /*   WIRD SPÄTER WIEDER BENUTZT
    internal void HighlightValidPath(bool isProp)
    {
        if (isGlowing == false) return;
        if (!isProp)
        {
            foreach (Renderer renderer in glowMaterialDictionaryHexes.Keys)
            {
                foreach (Material item in glowMaterialDictionaryHexes[renderer])
                {
                    if (item == null) continue;
                    item.SetColor("_GlowColor", validSpaceColor);
                }
            }
        }
        if (isProp)
        {
            foreach (Renderer renderer in glowMaterialDictionaryProps.Keys)
            {
                foreach (Material item in glowMaterialDictionaryProps[renderer])
                { 
                    if (item == null) continue;
                    item.SetColor("_GlowColor", validSpaceColor);
                }
            }
        }
    }*/
    internal void ResetGlowHighlight(bool isProp)
    {
        if (!isProp)
        {
            foreach (Renderer renderer in glowMaterialDictionaryHexes.Keys)
            {
                foreach (Material item in glowMaterialDictionaryHexes[renderer])
                {
                      if (item == null) continue;
                    item.SetColor("_GlowColor", originalGlowColor);
                }
            }
        }
        if (isProp)
        {
            foreach (Renderer renderer in glowMaterialDictionaryProps.Keys)
        {
            foreach (Material item in glowMaterialDictionaryProps[renderer])
            {
                if (item == null) continue;
                item.SetColor("_GlowColor", originalGlowColor);
            }
        }
        }
    }
    public void ToggleGlow(bool isProp)
    {
        if (!isProp)
        {
            if (isGlowing == false)
            {
                ResetGlowHighlight(isProp);
                foreach (Renderer renderer in originalMaterialDictionaryHexes.Keys)
                {
                    if (renderer == null) continue;
                    renderer.materials = glowMaterialDictionaryHexes[renderer];
                }
            }
        else
        {
            foreach (Renderer renderer in originalMaterialDictionaryHexes.Keys)
            {
                if (renderer == null) continue;
                renderer.materials = originalMaterialDictionaryHexes[renderer];
            }
        }
        isGlowing = !isGlowing;
        }
        if (isProp)
        {
            if (isGlowing == false)
            {
                ResetGlowHighlight(isProp);
                foreach (Renderer renderer in originalMaterialDictionaryProps.Keys)
                {
                    if (renderer == null)
                        continue;
                    renderer.materials = glowMaterialDictionaryProps[renderer];
                }
            }
            else
            {
                foreach (Renderer renderer in originalMaterialDictionaryProps.Keys)
                {
                    if (renderer == null)
                        continue;
                    renderer.materials = originalMaterialDictionaryProps[renderer];
                }
            }
            isGlowing = !isGlowing;
        }
    }
    public void ToggleGlow(bool state, bool isProp)
    {
        if (isGlowing == state) return;
        isGlowing = !state;
        ToggleGlow(isProp);
    }
    #endregion
}