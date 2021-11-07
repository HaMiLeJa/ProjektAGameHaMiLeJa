using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyGenerator : MonoBehaviour
{
    [Tooltip ("How much energy this generatior currently has")]
    public float GeneratedEnergy = 0f;
    [SerializeField] float maxEnergy = 1;
    [SerializeField] float energyGenerationRate = 0.1f;

    float timer;

    private void Update()
    {


        if (GeneratedEnergy >= maxEnergy) return;

        timer += Time.deltaTime;

        if (timer > 1)
        {
            GeneratedEnergy += energyGenerationRate;
            timer = 0;
        }

    }
}