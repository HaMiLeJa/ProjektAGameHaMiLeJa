using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDirection : MonoBehaviour
{
    GameObject player;

    [SerializeField] private float BoostForce = 200f;
    private float BoostDuration = 0.8f;
    [SerializeField] private AnimationCurve BoostCurve;
    private bool isChangingDirection = false;
    private Coroutine changeDirectionCoroutine;
    private bool allowChangeDirection = true;


    Rigidbody PlayerRb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");


        PlayerRb = player.GetComponent<Rigidbody>();

    }

    public void ChangeDirectionStarter()
    {
        if (allowChangeDirection == false) return;

        allowChangeDirection = false;

        
        if (changeDirectionCoroutine != null)
            StopCoroutine(changeDirectionCoroutine);
        

        isChangingDirection = true;
        changeDirectionCoroutine = StartCoroutine(ChangeDirectionCoroutine());
    }

    private IEnumerator ChangeDirectionCoroutine()
    {
        Vector3 velocity = PlayerRb.velocity;

        PlayerRb.velocity = velocity * -1;
        yield return new WaitForSeconds(0.5f);

        /*
        float t = 0;

        while (t < BoostDuration)
        {

            t += Time.deltaTime;
            float curveValue = BoostCurve.Evaluate(t);


            

            playerMov.currentHexChangeDirectionForce -= BoostForce * curveValue * Time.deltaTime;


            playerMov.OnChangeDirectionHex = true;
            yield return null;
        }
        */

       // PlayerRb.velocity = PlayerRb.velocity / 2;

        //playerMov.OnChangeDirectionHex = false;
       // playerMov.currentHexChangeDirectionForce = 0;
        isChangingDirection = false;


        yield return null;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == player)
        {
            allowChangeDirection = true; ;
        }
    }
}
