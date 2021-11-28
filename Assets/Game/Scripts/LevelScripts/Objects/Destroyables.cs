using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyables : MonoBehaviour
{
    [SerializeField] float lifeStartAmount = 100; //=100
    [Tooltip ("Dont change. Its Set to lifeStartAmount in the code")] [SerializeField] float currentLife;

    [Tooltip("How many Points the player gets if this wall is destroyed")] [SerializeField] float value = 1;
    [Space]
    AudioSource myAudioSource;
    [SerializeField] AudioClip collisionClip;
    [SerializeField] AudioClip destructionClip;

    void Start()
    {
        currentLife = lifeStartAmount;
        myAudioSource = this.GetComponent<AudioSource>();
        
    }

    void Update()
    {
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //evt boost bools abfragen, um mehr abzuziehen

            GameObject player = collision.gameObject;

            if(player == null)
            {
                return;
            }


            Rigidbody playerRb = player.GetComponent<Rigidbody>();
            float totalVelocity = Mathf.Abs(playerRb.velocity.x) + Mathf.Abs(playerRb.velocity.y) + Mathf.Abs(playerRb.velocity.z);

            float multiplicator = 0.4f;

            currentLife -= totalVelocity * multiplicator;


            if (currentLife < 0)
            {

                GameManager.Instance.onDestroyableDestroyed?.Invoke(value); //Call Event


                if (myAudioSource.isPlaying == false && AudioManager.Instance.allowAudio == true)
                {
                    myAudioSource.clip = destructionClip;
                    myAudioSource.Play();
                }

                Destroy(this.gameObject);
            }
            else
            {
                if (myAudioSource.isPlaying == false && AudioManager.Instance.allowAudio == true)
                {
                    myAudioSource.clip = collisionClip;
                    myAudioSource.Play();
                }
            }
        }
    }
}
