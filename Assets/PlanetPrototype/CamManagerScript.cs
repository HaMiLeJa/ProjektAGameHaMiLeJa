using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManagerScript : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] GameObject Planet;

    [HideInInspector] public Vector3 gravityDirection;

    [SerializeField] float followSpeed = 5f;

    [SerializeField] float rotationSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        //Smooth


        //Position :> Follow Player
        transform.position = Vector3.Lerp(transform.position, Player.transform.position, followSpeed * Time.deltaTime);
        gravityDirection = (transform.position - Planet.transform.position).normalized;

        Quaternion toRotation = Quaternion.FromToRotation(transform.up, gravityDirection) * transform.rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 0.1f);


        //Rotation
        float yValue = Input.GetAxis("HorizontalRotation");
        float xValue = Input.GetAxis("VerticalRotation");

    }



    public void NewPlanet(GameObject newPlanet){

        Planet = newPlanet;
    }
}
