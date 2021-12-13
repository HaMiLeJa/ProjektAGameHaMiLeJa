using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownDash : MonoBehaviour
{
    #region Inspector
    

    Rigidbody rb;
    GameManager gameMng;
    PlayerMovement playerMov;

    //[SerializeField] float boostCost = 1;
    [Space]
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

    AudioManager audManager;
    [SerializeField] AudioSource audioSource;

    #endregion


    void Start()
    {
        rb = ReferenceLibary.RigidbodyPl;
        gameMng = GameManager.Instance;
        playerMov = ReferenceLibary.PlayerMov;
        audManager = AudioManager.Instance;
    }

    void FixedUpdate()
    {
        if (gameMng.AllowMovement == false) return;

        if (Input.GetButton("A"))
        {
            if (gameMng.AllowMovement == false) return;
            if (playerMov.OnGround == false && buttonPressedInLastFrame == false)
            {
                boostingDown = true;
                buttonPressedInLastFrame = true;

                EnergyManager.onEnergyChange?.Invoke(-gameMng.DownDashCosts);
                
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

                rb.AddForce((rb.velocity.normalized/2 + Vector3.down) * speed * 100  *Time.deltaTime, ForceMode.Impulse);
                
            }
            else
            {
                
            }

            if(playerMov.OnGround == true)
            {                   //Stoppbewegung         //je nach winkel stopp oder rollen
                rb.velocity = new Vector3 (0, 0, 0); //(rb.velocity.x, 0, rb.velocity.z)
                boostingDown = false;
                StartCoroutine(DisableMovement());

                timer = 0;
                particleCoroutineStarted = false;

                smashParticle.Play();

                if (audioSource.isPlaying == false && audManager.allowAudio == true)
                    audioSource.Play();


                //touchedGround = true;
            }
        }
    }

    IEnumerator DisableMovement()
    {
        Vector3 pos = this.transform.position;
        gameMng.AllowMovement = false;
        gameMng.AllowHexEffects = false;

        float timer = 0;
        while (timer < 3)
        {
            timer += Time.deltaTime;
            this.transform.position = pos;
            yield return null;
        }

        gameMng.AllowMovement = true;
        gameMng.AllowHexEffects = true;

        yield return null;
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
