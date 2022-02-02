using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class UIEnergy : MonoBehaviour
{
    private Material material;
    public ParticleSystem _energyPartikel;
    void Start()
    {
        material = GetComponent<MeshRenderer>().sharedMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        material.SetFloat("_control",  MathLibary.Remap(
            ReferenceLibary.EnergyMng.MaxEnergyAmount*0.1f,
            ReferenceLibary.EnergyMng.MaxEnergyAmount,
            0.45f,
            0,
            EnergyManager.CurrentEnergy));
        playUIVFX();
    }

    void playUIVFX()
    {
        if (EnergyManager.energyGotHigher)
        {
            if (_energyPartikel.isPlaying)
                return;
            else
            {
               StopCoroutine(playUIVFX_Coroutine());
               StartCoroutine(playUIVFX_Coroutine());
               EnergyManager.energyGotHigher = false;
            }
        }

    }

    IEnumerator playUIVFX_Coroutine()
    { 
        _energyPartikel.Play(true);
        yield return null;
        
    }
}
