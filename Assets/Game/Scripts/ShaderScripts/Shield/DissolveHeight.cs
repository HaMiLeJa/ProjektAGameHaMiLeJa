using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveHeight : MonoBehaviour
{
    Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        rend.material.SetFloat("_DissolveStartHeight", transform.position.y);
    }
}
