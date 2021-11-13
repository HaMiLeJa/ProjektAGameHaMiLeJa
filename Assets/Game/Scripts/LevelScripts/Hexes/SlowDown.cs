using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDown : MonoBehaviour
{


    GameObject player;

    [SerializeField] private float BoostForce = 400f;
    private float SlowDownDuration = 0.4f;
    [SerializeField] private AnimationCurve BoostCurve;
    [SerializeField] public bool IsHexBoosting = false; //used to lock other boosts
    private Coroutine shadowDashCoroutine;



    PlayerMovement playerMov;



    Rigidbody PlayerRb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");



        playerMov = player.GetComponent<PlayerMovement>();
        PlayerRb = player.GetComponent<Rigidbody>();

    }

    void FixedUpdate()
    {

    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            ShadowDashStarter();

        }
    }


    public void ShadowDashStarter()
    {
        if (shadowDashCoroutine != null)
            StopCoroutine(shadowDashCoroutine);

        shadowDashCoroutine = StartCoroutine(HexBoostForwardCoroutine());
    }

    private IEnumerator HexBoostForwardCoroutine()
    {
        Vector3 velocity = PlayerRb.velocity;

        float t = 0;
        Vector3 halfVelocity = velocity * 0.5f;

        while (t < SlowDownDuration)
        {

            t += Time.deltaTime;
            float curveValue = BoostCurve.Evaluate(t);



            

            PlayerRb.velocity *= 0.99f;
            //playerMov.currentHexBoostForwardForce -= BoostForce * curveValue * Time.deltaTime;


            playerMov.OnBoostForwardHex = true;
            yield return null;
        }


        //PlayerRb.velocity = PlayerRb.velocity / 2;

        playerMov.OnBoostForwardHex = false;
        playerMov.currentHexForce = 0;
        IsHexBoosting = false;
    }
}
