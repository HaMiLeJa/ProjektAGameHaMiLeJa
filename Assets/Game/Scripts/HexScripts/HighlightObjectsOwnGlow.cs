using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class HighlightObjectsOwnGlow : MonoBehaviour

{
    [BoxGroup("Glow Einstellungen")][SerializeField]  private float glowAmount ;
    [BoxGroup("Glow Einstellungen")]  [SerializeField] private float CutoffEmissive;
    
    private int propIDEmissiveIntensity;
    private int propIDEmissiveCutoff;
    private Material material;
    private Hex hex;
    private float cashedEmissiveIntensity;
    private float cashedEmissiveCutoff; 
    private void Awake()
    {
        material = this.gameObject.GetComponent<Renderer>().material;
         propIDEmissiveIntensity =  Shader.PropertyToID("_emissionIntensity");
        propIDEmissiveCutoff = Shader.PropertyToID("_emissiveCutoff");
    }
    
    private void Start()
    {  
      cashedEmissiveIntensity = material.GetFloat(propIDEmissiveIntensity );
      cashedEmissiveCutoff   =  material.GetFloat(propIDEmissiveCutoff );
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == ReferenceLibary.Player)
        {
            StartCoroutine(EnableHighlightDelayed());
            StartCoroutine(DisableHighlightDelayed());
        }
    }
    
    IEnumerator EnableHighlightDelayed()
    {
        yield return new WaitForSeconds(GameManager.GlowEnableDelayObjects);
        material.SetFloat(propIDEmissiveIntensity, glowAmount );
        material.SetFloat(propIDEmissiveCutoff, CutoffEmissive );
    }

    IEnumerator DisableHighlightDelayed()
    {
        yield return new WaitForSeconds(GameManager.GlowDisableDelayObjects);
        material.SetFloat(propIDEmissiveIntensity, cashedEmissiveIntensity );
        material.SetFloat(propIDEmissiveCutoff, cashedEmissiveCutoff );
    }
}
