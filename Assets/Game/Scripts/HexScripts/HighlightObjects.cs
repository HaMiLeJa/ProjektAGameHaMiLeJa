using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Random = System.Random;

public class HighlightObjects : MonoBehaviour

{
    [SerializeField] private GameObject Lampe;
    [SerializeField] private int headshakes = 4;
    private Hex hex;
    private void Awake()
    {
    
        hex = transform.parent.transform.parent.GetComponent<Hex>();
        if(Lampe == null)
        return;
    }

 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ReferenceLibary.Player)
        {
            hex.highlightProps();
            if(Lampe == null)
                return;
            if (Mathf.Abs(ReferenceLibary.RigidbodyPl.velocity.x) + Mathf.Abs(ReferenceLibary.RigidbodyPl.velocity.z) > 70)
            {
                int randomDirection =UnityEngine.Random.Range(0, 1);
                if (randomDirection == 0)
                    StartCoroutine(Rotate(Lampe,headshakes,0.17f, 50, Vector3.up));
                else if (randomDirection == 1)
                    StartCoroutine(Rotate(Lampe,headshakes,0.17f, 50, Vector3.down));
            }
          
        }
    
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == ReferenceLibary.Player)
        {
            hex.highlightProps();
        }
        
       
    }
    public IEnumerator Rotate(GameObject rotateMe,int headshakes , float duration, float angle, Vector3 firstDirection)
    { 
        if (headshakes == 0)
            yield break;
        Quaternion startRot = rotateMe.transform.rotation;
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            rotateMe.transform.rotation = startRot * Quaternion.AngleAxis(t / duration * angle, firstDirection);
            yield return null;
        }
        headshakes--;
        yield return Rotate(rotateMe,headshakes,UnityEngine.Random.Range(1.2f,1.4f)*duration,
            UnityEngine.Random.Range(angle, angle + 5), -firstDirection);
    }

}
