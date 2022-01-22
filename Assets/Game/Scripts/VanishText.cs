using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishText : MonoBehaviour
{
    

    void Start()
    {
        
    }

    float timer;
   [SerializeField] float duration;

    void Update()
    {
        timer += Time.deltaTime;

        if(timer > duration)
        {
            timer = 0;
            this.gameObject.SetActive(false);
        }
        

    }
}
