using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDown : MonoBehaviour
{


    GameObject player;

    [SerializeField] private float SlowDownForce = 400f;
    private float SlowDownDuration = 0.4f;
    [SerializeField] private AnimationCurve SlowDornCurve;
    private bool IsSlowingDown = false; //used to lock other boosts
    private Coroutine slowDownCoroutine;


    Rigidbody PlayerRb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        PlayerRb = player.GetComponent<Rigidbody>();

    }


    public void SlowDownStarter()
    {
           if (slowDownCoroutine != null)
               StopCoroutine(slowDownCoroutine);

           slowDownCoroutine = StartCoroutine(SlowDownCoroutine());
    }

    private IEnumerator SlowDownCoroutine()
    {
        Debug.Log("S");
        Vector3 velocity = PlayerRb.velocity;

        float t = 0;
       // Vector3 halfVelocity = velocity * 0.5f;

        while (t < SlowDownDuration)
        {

            t += Time.deltaTime;
            float curveValue = SlowDornCurve.Evaluate(t);



            

            PlayerRb.velocity *= 0.99f;
            //playerMov.currentHexBoostForwardForce -= BoostForce * curveValue * Time.deltaTime;


            //playerMov.OnBoostForwardHex = true;
            yield return null;
        }


        //PlayerRb.velocity = PlayerRb.velocity / 2;

        //playerMov.OnBoostForwardHex = false;
        //playerMov.currentHexFowardForce = 0;
        IsSlowingDown = false;
    }
}
