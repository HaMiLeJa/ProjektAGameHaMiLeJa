using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyables : MonoBehaviour
{
    public ScriptableLevelObject settings;
    [Space]
    public AudioSource myAudioSource;
    [SerializeField] AudioClip collisionClip;
    [SerializeField] AudioClip destructionClip;

    PlayerSuperDash superDash;
    [SerializeField] GameObject player;
    Collider col;


    bool TriggerResetted = false;

    void Start()
    {
        myAudioSource = this.GetComponent<AudioSource>();
        col = this.GetComponent<Collider>();

        superDash = ReferenceLibary.SuperDash;
        player = ReferenceLibary.Player;
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
        if (collision.gameObject != player) return;
        

        if (superDash.isDestroying == true || ReferenceLibary.DownDashPl.isDestroying == true)
        {
            //Sound
            //Effekte

            col.enabled = false;

            ReferenceLibary.RigidbodyPl.velocity *= -1; 

            ScoreManager.OnScoring?.Invoke(settings.value);

            if (myAudioSource.isPlaying == false)
            {
                myAudioSource.clip = destructionClip;
                myAudioSource.Play();
            }

            //Destroy(this.gameObject); //oder Set active + respawn

            StartCoroutine(PlayParticleAndDestroyObj());
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

    IEnumerator PlayParticleAndDestroyObj()
    {
        //play stuff
        yield return new WaitForSeconds(2f);

        this.gameObject.SetActive(false);
    }
}
