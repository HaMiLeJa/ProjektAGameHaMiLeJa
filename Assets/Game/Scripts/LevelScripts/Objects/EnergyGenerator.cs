using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyGenerator : MonoBehaviour
{
    [Tooltip("How much energy this generatior currently has")]
    [SerializeField] float generatedEnergy = 0f;
    [SerializeField] float maxEnergy = 1;
    [SerializeField] float energyGenerationAmount = 0.1f;
    [SerializeField] float energyGenerationRate = 1f; //Alle 1Sekunde

    //[SerializeField] GameObject player;

    float timer;
   

    private void Update()
    {
        if (generatedEnergy >= maxEnergy) return;

        timer += Time.deltaTime;

        if (timer > energyGenerationRate)
        {
            generatedEnergy += energyGenerationAmount;
            timer = 0;
        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameManager.Instance.onUIEnergyChange?.Invoke(generatedEnergy);
            GameManager.Instance.onEnergyChange?.Invoke(generatedEnergy);
            generatedEnergy = 0;
        }
    }
}