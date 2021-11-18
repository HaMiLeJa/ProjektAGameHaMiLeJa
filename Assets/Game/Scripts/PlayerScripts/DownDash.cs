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

    
    [SerializeField] GameObject SlamParent;

    bool particleCoroutineStarted = false;
    [SerializeField] GameObject PlayerParticleParent;


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
        if (Input.GetButton(gameMng.DownDash) && playerMov.OnGround == false)
        {
            timer += Time.deltaTime;

            if (timer < boostDuration)
            {
                if (particleCoroutineStarted == false)
                {
                    particleCoroutineStarted = true;
                    StartCoroutine(PlayParticle());
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


            particleCoroutineStarted = false;
            
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
