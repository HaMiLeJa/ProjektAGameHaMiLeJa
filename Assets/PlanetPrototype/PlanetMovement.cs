using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMovement : MonoBehaviour
{
    float horizontal = 0;
    float vertical = 0;
    [SerializeField] float speed;

    void Start()
    {
        
    }

   
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        //transform.RotateAround(this.transform.position, transform.up, -horizontal * speed * Time.deltaTime);
        //transform.RotateAround(this.transform.position, transform.right, vertical * speed * Time.deltaTime);

        this.transform.Rotate(new Vector3(-vertical * speed * Time.deltaTime, 0, horizontal * speed * Time.deltaTime), Space.World);

    }
}
