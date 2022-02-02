using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class HighlightObjects : MonoBehaviour

{  
    [InfoBox("Einfache Objekte die einfach aus einem Collider bestehen, brauchen keinen extra collider, es geht dann über collision)", EInfoBoxType.Normal)]
    [InfoBox("Falls das Material von alleine leuchtet, Has Own Glow markieren.", EInfoBoxType.Normal)]
    
    [BoxGroup("Glow Einstellungen")]  [SerializeField]  private bool hasOwnGlow;
    [BoxGroup("Glow Einstellungen")][SerializeField]  private float glowAmount;
    [BoxGroup("Glow Einstellungen")]  [SerializeField] private float CutoffEmissive;
    
    private int propIDEmissiveIntensity;
    private int propIDEmissiveCutoff;
    private Material material;
    private Hex hex;
    private float cashedEmissiveIntensity;
    private float cashedEmissiveCutoff; 
    private void Awake()
    {
        propIDEmissiveIntensity =  Shader.PropertyToID("_emissionIntensity");
        propIDEmissiveCutoff = Shader.PropertyToID("_emissiveCutoff");
        material = this.gameObject.GetComponent<Renderer>().material;
        hex = transform.parent.transform.parent.GetComponent<Hex>();
    }

    private void Start()
    {
         cashedEmissiveIntensity = material.GetFloat(propIDEmissiveIntensity );
         cashedEmissiveCutoff   =  material.GetFloat(propIDEmissiveCutoff );
    }
    private void OnTriggerEnter(Collider other)
    {   
        if (other.gameObject == ReferenceLibary.Player && !hasOwnGlow) hex.highlightProps();
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == ReferenceLibary.Player && !hasOwnGlow) hex.highlightProps();
        
        if (other.gameObject == ReferenceLibary.Player && hasOwnGlow)
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
