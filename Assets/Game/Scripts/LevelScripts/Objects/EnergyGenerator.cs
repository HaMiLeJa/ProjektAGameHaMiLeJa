using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyGenerator : MonoBehaviour
{
    [Tooltip("How much energy this generatior currently has")]
    [SerializeField] float generatedEnergy = 0f;

    //[SerializeField] GameObject player;

    [SerializeField] ScriptableEnergyGenerator settings;

    float timer;
   

    private void Update()
    {
        if (generatedEnergy >= settings.maxEnergy) return;

        timer += Time.deltaTime;

        if (timer > settings.energyGenerationRate)
        {
            generatedEnergy += settings.energyGenerationAmount;
            timer = 0;
        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == ReferenceLibary.Player)
        {
            EnergyManager.onEnergyChange?.Invoke(generatedEnergy);
            generatedEnergy = 0;
        }
    }
   
}