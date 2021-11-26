using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostForward : MonoBehaviour
{

    GameObject player;
    
    [SerializeField] private float BoostForce = 200f;
    private float BoostDuration = 0.8f;
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

    public void BoostForwardStarter()
    {
        if (shadowDashCoroutine != null)
            StopCoroutine(shadowDashCoroutine);

        shadowDashCoroutine = StartCoroutine(HexBoostForwardCoroutine());
    }

    private IEnumerator HexBoostForwardCoroutine()
    {
        Vector3 velocity = PlayerRb.velocity;

        float t = 0;
        Debug.Log("B");
        while (t < BoostDuration)
        {

            t += Time.deltaTime;
            float curveValue = BoostCurve.Evaluate(t);



            //
            playerMov.CurrentHexFowardForce += BoostForce * curveValue * Time.deltaTime;


            playerMov.OnBoostForwardHex = true;
            yield return null;
        }


        PlayerRb.velocity = PlayerRb.velocity / 2;

        playerMov.OnBoostForwardHex = false;
        playerMov.CurrentHexFowardForce = 0;
        IsHexBoosting = false;
    }



    #region Variante 1
    /*private IEnumerator SpeedUpAndDown()
    {
        Debug.Log("Coroutine");


        float timer = 0;

        while (timer < 0.5f)
        {
            timer += Time.deltaTime;
            playerRb1.AddForce(playerRb1.velocity * Time.deltaTime * boostPower, ForceMode.Force);
            yield return null;
        }



        timer = 0;

        while (timer > 0.5f)
        {
            playerRb1.velocity *= -0.9f;
            Debug.Log("decreasing");
            yield return null;
        }


        yield return null;


        /*
        float timer = 0;
        while (timer < 0.5f)
        {
            timer += Time.deltaTime;
            playerRb.velocity *= 1.1f; //*time.deltatime?
            Debug.Log("Adding");
            yield return null;
        }

        Debug.Log("Waiting");
        yield return new WaitForSeconds(2f);


        timer = 0;

        while (timer > 0.5f)
        {
            playerRb.velocity *= -0.9f;
            Debug.Log("decreasing");
        }

        Debug.Log("End");

        
    } */
    #endregion
}
