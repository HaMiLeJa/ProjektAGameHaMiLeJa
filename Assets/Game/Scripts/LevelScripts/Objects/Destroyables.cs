using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyables : MonoBehaviour
{
    [SerializeField] float lifeStartAmount = 100; //=100
    
    [Tooltip("How many Points the player gets if this wall is destroyed")] [SerializeField] float value = 1;
    [Space]
    public AudioSource myAudioSource;
    [SerializeField] AudioClip collisionClip;
    [SerializeField] AudioClip destructionClip;

    PlayerSuperDash superDash;
    GameObject Player;
    Collider col;

    bool TriggerResetted = false;

    void Start()
    {
        myAudioSource = this.GetComponent<AudioSource>();
        Player = GameObject.FindGameObjectWithTag("Player");
        superDash = Player.GetComponent<PlayerSuperDash>();
        col = this.GetComponent<Collider>();
    }


    private void FixedUpdate()
    {
        /*
        if (superDash.isDestroying == true)
        {
            col.isTrigger = true;
            TriggerResetted = true;
        }
        else if (TriggerResetted == false)
        {
            TriggerResetted = true;
            col.isTrigger = false;
        }
        */
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != Player) return;
        

        if (superDash.isDestroying == true)
        {
            //Punkte
            //Sound
            //Effekte

           

            if (myAudioSource.isPlaying == false)
            {
                myAudioSource.clip = destructionClip;
                myAudioSource.Play();
            }

            Destroy(this.gameObject); //oder Set active + respawn
        }
        else
        {

            if (myAudioSource.isPlaying == false)
            {
                myAudioSource.clip = collisionClip;
                myAudioSource.Play();
            }
        }

    }
}
