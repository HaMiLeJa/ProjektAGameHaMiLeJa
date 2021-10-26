using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallEnergy : MonoBehaviour
{
    public float energy = 0.5f;

    float timer;

    private void Update()
    {
        if (energy >= 1) return;

        timer += Time.deltaTime;

        if(timer > 1)
        {
            energy += 0.1f;
            timer = 0;
        }

    }
}
