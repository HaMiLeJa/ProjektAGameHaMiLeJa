using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class BoostInDirectionProp : MonoBehaviour
{
     [HideInInspector]public Hex MyHex;
    Vector3 Direction;
    Quaternion desiredRot;

    void Start()
    {
      //  MyHex = this.transform.parent.transform.parent.GetComponent<Hex>(); //später über instatniaten zuweisen

    }

    
    void Update()
    {
        SetDesiredRotation();
        RotateTowardsDesiredPos();
    }

  
    void SetDesiredRotation()
    {
        Direction = new Vector3(MyHex.XDirection, 0, MyHex.ZDirection);
        desiredRot = Quaternion.LookRotation(Direction, Vector3.up);
    }

    void RotateTowardsDesiredPos()
    {
       // this.transform.rotation = Quaternion.RotateTowards(from: this.transform.rotation, to: desiredRot, maxDegreesDelta: angularSpeed * Time.deltaTime);


        this.transform.rotation = desiredRot;
    }
}
