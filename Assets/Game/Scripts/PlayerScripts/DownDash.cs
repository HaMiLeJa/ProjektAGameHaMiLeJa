using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownDash : MonoBehaviour
{
    Rigidbody rb;
    GameManager gameMng;
    PlayerMovement playerMov;

   [SerializeField] bool buttonPressedInLastFrame = false;
    [SerializeField]  bool touchedGround = true;

    [SerializeField] float speed = 8;

    private float timer;
    private float boostDuration = 0.1f;
    [SerializeField] bool boostingDown = false;

    
    [SerializeField] GameObject SlamParent;

    bool particleCoroutineStarted = false;
    [SerializeField] GameObject PlayerParticleParent;

    [SerializeField] ParticleSystem smashParticle;



    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        gameMng = FindObjectOfType<GameManager>();
        playerMov = this.GetComponent<PlayerMovement>();
    }

    void FixedUpdate()
    {
        

        if (Input.GetButton(gameMng.DownDash) ) //&& touchedGround == true
        {
            if (playerMov.OnGround == false && buttonPressedInLastFrame == false)
            {
                boostingDown = true;
                buttonPressedInLastFrame = true;
                //touchedGround = false;
            }
        }
        else
        {
            buttonPressedInLastFrame = false;
        }


        if (boostingDown == true)
        {
            timer += Time.deltaTime;

            if (timer < boostDuration)
            {

                /*
                if (particleCoroutineStarted == false)
                {
                    particleCoroutineStarted = true;
                    StartCoroutine(PlayParticle());

                    
                }
                */

                rb.AddForce((rb.velocity.normalized/2 + Vector3.down) * speed, ForceMode.Impulse);
                
            }
            else
            {
                
            }

            if(playerMov.OnGround == true)
            {                   //Stoppbewegung         //je nach winkel stopp oder rollen
                rb.velocity = new Vector3 (0, 0, 0); //(rb.velocity.x, 0, rb.velocity.z)
                timer = 0;
                particleCoroutineStarted = false;

                smashParticle.Play();

                boostingDown = false;
                //touchedGround = true;
            }
        }
    }

    

    IEnumerator PlayParticle()
    {
        while (playerMov.OnGround == false)
        {
            yield return null;
        }

        Debug.Log("Coroutine");


        GameObject slam = Instantiate(SlamParent, this.transform.position, this.transform.rotation, PlayerParticleParent.transform);
       // slam.SetActive(true);

        slam.GetComponent<SlamParentScript>().Detach();


        

        yield return null;

    }
    
}
