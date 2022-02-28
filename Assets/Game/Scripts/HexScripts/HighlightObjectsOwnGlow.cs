using System.Collections;
using UnityEngine;
using NaughtyAttributes;
public class HighlightObjectsOwnGlow : MonoBehaviour
{
    [BoxGroup("Glow Einstellungen")][SerializeField]  private float glowAmount ;
    [BoxGroup("Glow Einstellungen")]  [SerializeField] private float CutoffEmissive;
    private int propIDEmissiveIntensity, propIDEmissiveCutoff;
    private float cashedEmissiveIntensity, cashedEmissiveCutoff; 
    private Material material;
    private Hex hex;
    private void Awake()
    {
        material = gameObject.GetComponent<Renderer>().material;
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
