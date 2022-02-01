using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class HighlightObjects : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ReferenceLibary.Player)
        {
          transform.parent.transform.parent.GetComponent<Hex>().highlightProps();
        }
        
    }
    
    }
