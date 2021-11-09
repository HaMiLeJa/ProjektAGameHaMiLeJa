using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownDash : MonoBehaviour
{
    Rigidbody rb;
    EnergyManager energyMng;
    GameManager gameMng;
    PlayerMovement playerMov;

    [SerializeField] float speed = 3;

    [SerializeField] float timer;
    [SerializeField] float boostDuration = 0.1f;
    [SerializeField] bool boostingDown = false;


    private void Awake()
    {
    }

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        energyMng = FindObjectOfType<EnergyManager>();
        gameMng = FindObjectOfType<GameManager>();
        playerMov = this.GetComponent<PlayerMovement>();
    }
        // Update is called once per frame
    void Update()
    {
        if (Input.GetButton(gameMng.DownDash))
        {
            timer += Time.deltaTime;

            if (timer < boostDuration)
            {
                boostingDown = true;
                rb.AddForce(rb.velocity.normalized + Vector3.down * speed, ForceMode.Impulse);

                if (playerMov.OnGround == false && playerMov.OnGround == false)
                {
                    //Vector3 direction = new Vector3(rb.velocity.x, -1, rb.velocity.z);
                    //direction = direction.normalized;


                }
            }
            else
                boostingDown = false;

        }
        else
        {
            timer = 0;
            boostingDown = true;
        }
        
    }
}
