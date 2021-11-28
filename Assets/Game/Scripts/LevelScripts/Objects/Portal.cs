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

    AudioSource myAudioSource;

    void Start()
    {
        myAudioSource = this.GetComponent<AudioSource>();
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

                if (myAudioSource.isPlaying == false && AudioManager.Instance.allowAudio == true)
                    myAudioSource.Play();
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == player.tag && GoalPortal == true)
        {
            StartPortal = false;
            GoalPortal = false;
        }
    }



}
