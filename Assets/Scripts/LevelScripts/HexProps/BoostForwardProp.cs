using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostForwardProp : MonoBehaviour
{
    Vector3 Direction;

    public float angularSpeed = 400f;
    Quaternion desiredRot;



    void Update()
    {
        InPlayerDirection();
    }





    void SetDesiredRotation(int sign)
    {
        Direction = new Vector3(ReferenceLibrary.PlayerMov.Velocity.normalized.x, 0, ReferenceLibrary.PlayerMov.Velocity.normalized.z);
        if (Direction == Vector3.zero) return;
        desiredRot = Quaternion.LookRotation(Direction * sign, Vector3.up);
    }

    void RotateTowardsDesiredPos()
    {
        this.transform.rotation = Quaternion.RotateTowards(from: this.transform.rotation, to: desiredRot, maxDegreesDelta: angularSpeed * Time.deltaTime);
    }

    void InPlayerDirection()
    {
        SetDesiredRotation(1);
        RotateTowardsDesiredPos();
    }
}
