using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime : MonoBehaviour
{
    float time = 8;
    float timer = 0;
    void Start()
    {
        timer = 0;
    }

   
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > time)
            Destroy(this.gameObject);
    }
}
