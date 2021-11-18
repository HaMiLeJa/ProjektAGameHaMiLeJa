using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownDash : MonoBehaviour
{
    Rigidbody rb;
    GameManager gameMng;
    PlayerMovement playerMov;

    [SerializeField] float speed = 8;

    private float timer;
    private float boostDuration = 0.1f;
    [SerializeField] bool boostingDown = false;

    
    [SerializeField]  ParticleSystem SlamParticlePrefab;
    ParticleSystem SlamObject;

    bool particleCoroutineStartet = false;
    


    private void Awake()
    {
    }

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        gameMng = FindObjectOfType<GameManager>();
        playerMov = this.GetComponent<PlayerMovement>();
    }

    void FixedUpdate()
    {
        if (Input.GetButton(gameMng.DownDash))
        {
            timer += Time.deltaTime;

            if (timer < boostDuration)
            {
                if (particleCoroutineStartet == false)
                {
                    particleCoroutineStartet = true;
                    //StartCoroutine(PlayParticle());
                }

                if (playerMov.OnGround == false && playerMov.OnGround == false)
                {
                    boostingDown = true;
                    rb.AddForce((rb.velocity.normalized + Vector3.down) * speed, ForceMode.Impulse);

                }
            }
            else
                boostingDown = false;

        }
        else
        {
            timer = 0;
            boostingDown = false;
            //StopCoroutine(PlayParticle());

            /*if(SlamObject != null)
                Destroy(SlamObject); */


            particleCoroutineStartet = false;
            
        }

    }

    /*

    IEnumerator PlayParticle()
    {
        while (playerMov.OnGround == false)
        {
            yield return null;
        }


        SlamObject = Instantiate(SlamParticlePrefab, this.transform.position, this.transform.rotation);

        SlamObject.Play();
        
        while (SlamObject.isPlaying == true)
        {
            yield return null;
        }

        if (SlamObject != null)
            Destroy(SlamObject);

        yield return null;

    }
    */
}
