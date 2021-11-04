using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementJ : MonoBehaviour
{
    Rigidbody MyRigidbody;
    [SerializeField] float forceJump;
    float timer;

    [SerializeField] float jumpDuration;
    [SerializeField] bool grounded;
    [SerializeField] float distance;

    Earthquake earthquake;
    [SerializeField] float forceEarthquake;
    float timerEarthquake = 0;
    [SerializeField] float earthquakeJumpDuration;

    private void Start()
    {
        MyRigidbody = GetComponent<Rigidbody>();
        earthquake = FindObjectOfType<Earthquake>();
    }

    void Update()
    {
        if(earthquake.earthquakeing == true && grounded == true &&timerEarthquake < earthquakeJumpDuration)
        {
            timerEarthquake += Time.deltaTime;

            MyRigidbody.AddForce(Vector2.up * forceEarthquake);
            Debug.Log("Earthquake jump");
        }
        else if(earthquake.earthquakeing == false)
        {
            timerEarthquake = 0;
        }



        if (Input.GetButton("JumpJanina") && grounded == true && timer < jumpDuration)
        {
            timer += Time.deltaTime;
            MyRigidbody.AddForce(Vector2.up * forceJump);
        }
        else
        {
            timer = 0;
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            grounded = false;
        }
    }

}
