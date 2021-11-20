using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour //Portal in zwei Richtungen, Frei untereinander verlinkbar
{
    [Tooltip ("Link the Goal Portal here")]
    [SerializeField] GameObject Goal;
    GameObject player;

    [HideInInspector] public bool StartPortal = false;
    [HideInInspector] public bool GoalPortal = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == player.tag)
        {
            if (GoalPortal == false) //Wenn diese Portal das startPoral ist fortfahren
            {
                StartPortal = true;
                GoalPortal = false;

                Rigidbody playerRb = player.GetComponent<Rigidbody>();

                Goal.GetComponent<Portal>().GoalPortal = true;
                

                playerRb.transform.position = Goal.transform.position;
            }


            if(GoalPortal == true)
            {
                StartPortal = false;

            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == player.tag)
        {
            StartPortal = false;
            GoalPortal = false;
        }
    }



}
