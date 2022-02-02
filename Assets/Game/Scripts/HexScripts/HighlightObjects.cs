using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class HighlightObjects : MonoBehaviour

{  
    
    private Hex hex;
    private void Awake()
    {
    
        hex = transform.parent.transform.parent.GetComponent<Hex>();
    }

 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ReferenceLibary.Player)
        {
            hex.highlightProps();
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == ReferenceLibary.Player)
        {
            hex.highlightProps();
        }
        
       
    }

}
