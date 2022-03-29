using System.Collections;
using UnityEngine;
using NaughtyAttributes;
public class HighlightObjectsOwnGlow : MonoBehaviour
{
    [BoxGroup("Glow Einstellungen")][SerializeField]  private float glowAmount, CutoffEmissive;
    private float cashedEmissiveIntensity, cashedEmissiveCutoff; 
    private Material material;
    private Hex hex;
    private MaterialPropertyBlock mpb;
    
    private MaterialPropertyBlock MPB
    {
        get
        {
            if (mpb == null) mpb = new MaterialPropertyBlock();
            return mpb;
        }
    }
    static readonly int propIDEmissiveCutoff = Shader.PropertyToID("_emissiveCutoff");
    static readonly int propIDEmissiveIntensity =  Shader.PropertyToID("_emissionIntensity");
    private void Awake() => material = gameObject.GetComponent<Renderer>().sharedMaterial;
    
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
        yield return new WaitForSeconds(Highlightmanager.GlowEnableDelayObjects);
        MPB.SetFloat(propIDEmissiveIntensity, glowAmount );
        MPB.SetFloat(propIDEmissiveCutoff, CutoffEmissive );
        this.gameObject.GetComponent<MeshRenderer>().SetPropertyBlock(MPB);
    }
    IEnumerator DisableHighlightDelayed()
    {
        yield return new WaitForSeconds(Highlightmanager.GlowDisableDelayObjects);
        MPB.SetFloat(propIDEmissiveIntensity, cashedEmissiveIntensity );
        MPB.SetFloat(propIDEmissiveCutoff, cashedEmissiveCutoff );
        this.gameObject.GetComponent<MeshRenderer>().SetPropertyBlock(MPB);
    }
}
