using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDirectionProp : MonoBehaviour
{

    Vector3 Direction;

    public float angularSpeed = 400f;
    Quaternion desiredRot;

    void Start()
    {
        
    }

    
    void Update()
    {
        InOppositePlayerDirection();
    }



    void InOppositePlayerDirection()
    {
       
        SetDesiredRotation(-1);
        RotateTowardsDesiredPos();
    }


    void SetDesiredRotation(int sign)
    {
        Direction = new Vector3(ReferenceLibary.PlayerMov.Velocity.normalized.x, 0, ReferenceLibary.PlayerMov.Velocity.normalized.z);
        if (Direction == Vector3.zero) return;
        desiredRot = Quaternion.LookRotation(Direction*sign, Vector3.up);
    }

    void RotateTowardsDesiredPos()
    {
        this.transform.rotation = Quaternion.RotateTowards(from: this.transform.rotation,to: desiredRot, maxDegreesDelta: angularSpeed * Time.deltaTime);
    }

    void InPlayerDirection()
    {
        SetDesiredRotation(1);
        RotateTowardsDesiredPos();
    }
}
