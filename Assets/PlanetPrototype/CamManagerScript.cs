using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManagerScript : MonoBehaviour
{
    public GameObject Player;
    public GameObject Planet;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Smooth


        //Position :> Follow Player
          transform.position = Vector3.Lerp(transform.position,Player.transform.position,0.1f);
          Vector3 gravDirection = (transform.position - Planet.transform.position).normalized;


        //Rotation
        Quaternion toRotation = Quaternion.FromToRotation(transform.up, gravDirection) * transform.rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 0.1f);


    }



    public void NewPlanet(GameObject newPlanet){

        Planet = newPlanet;
    }
}
