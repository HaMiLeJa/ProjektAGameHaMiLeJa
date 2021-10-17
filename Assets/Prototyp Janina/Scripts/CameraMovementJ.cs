using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementJ : MonoBehaviour
{
    [SerializeField] GameObject ObjectToFollow;
    [SerializeField] float Speed;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, ObjectToFollow.transform.position, Speed * Time.deltaTime);
    }
}
