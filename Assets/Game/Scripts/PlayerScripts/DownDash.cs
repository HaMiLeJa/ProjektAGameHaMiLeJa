using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownDash : MonoBehaviour
{
    Rigidbody rb;
    GameManager gameMng;
    PlayerMovement playerMov;

    [SerializeField] float speed = 8;

    float timer;
    [SerializeField] float boostDuration = 0.1f;
    [SerializeField] bool boostingDown = false;


    private void Awake()
    {
    }

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        gameMng = FindObjectOfType<GameManager>();
        playerMov = this.GetComponent<PlayerMovement>();
    }
        // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetButton(gameMng.DownDash))
        {
            timer += Time.deltaTime;

            if (timer < boostDuration)
            {
               
               

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
        }
        
    }
}
