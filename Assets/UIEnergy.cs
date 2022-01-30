using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class UIEnergy : MonoBehaviour
{
    private Material material;
    void Start()
    {
        material = GetComponent<MeshRenderer>().sharedMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        material.SetFloat("_control",  MathLibary.Remap(
            EnergyManager.Instance.MaxEnergyAmount*0.1f,
            EnergyManager.Instance.MaxEnergyAmount,
            0.45f,
            0,
            EnergyManager.CurrentEnergy));
    }
    
}
