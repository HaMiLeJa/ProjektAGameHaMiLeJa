using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LookAtCamera : MonoBehaviour
{
    Camera cam;
    private void Start()
    {
        cam = Camera.main;
    }
    void Update()
    {
        transform.forward = cam.transform.position - transform.position;
    }
}
