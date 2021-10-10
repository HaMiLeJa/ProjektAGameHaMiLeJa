using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float rotationSpeed;

    void FixedUpdate()
    {
        float rotationYValue = Input.GetAxis("Horizontal");
        transform.Rotate(new Vector3(0, rotationYValue * Time.deltaTime * rotationSpeed, 0));
    }
}
