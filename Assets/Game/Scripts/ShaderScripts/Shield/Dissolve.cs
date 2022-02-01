using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    Renderer renderer; 
    [SerializeField] float DisolveSpeed = 0.8f;
    

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }


    public IEnumerator Coroutine_DisolveShield(float target)
    {
        Debug.Log("Dissolving");
        float start = renderer.material.GetFloat("_Disolve");
        float lerp = 0;

        while (lerp < 1)
        {
            renderer.material.SetFloat("_Disolve", Mathf.Lerp(start, target, lerp));
            lerp += Time.deltaTime * DisolveSpeed;
            yield return null;
        }

        Debug.Log("Dissolving END");
    }
}