using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSuperDash : MonoBehaviour
{
    #region Inspector
    //[SerializeField] float boostCost = 1;
    [Space]
    [SerializeField] private float SuperDashForce;
    [SerializeField] private float ShadowDashDuration;
    [SerializeField] private AnimationCurve SuperDashcurve;
    [SerializeField] public bool isSuperDashing = false; //used to lock other boosts
    
    public Coroutine superDashCoroutine;

    [SerializeField] public float currentSuperDashForce = 0.0f;
    [SerializeField] private float disappearingDuringSuperDashStart;
    [SerializeField] private float disappearingDuringSuperDashEnd; 
    public ParticleSystem effect;

    public bool isDestroying = false;
   [SerializeField] bool superDashNotPossible = false;

   // public MeshRenderer mr;

    GameManager gameMng;
    PlayerBoost dash;
    PlayerMovement playerMov;
    ShadowDash shadowDash;

    Rigidbody rb;

    [SerializeField] LayerMask worldMask;
    int playerLayerInt;
    int playerDestructionLayerInt;

    AudioManager audManager;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip startClip;
    [SerializeField] AudioClip endClip;

    #endregion


    private void Start()
    {
        rb = ReferenceLibary.RigidbodyPl;

        dash = ReferenceLibary.Dash;
        playerMov = ReferenceLibary.PlayerMov;
        gameMng = ReferenceLibary.GameMng;
        shadowDash = ReferenceLibary.ShadowDashPl;

        //playerLayerInt = LayerMask.NameToLayer("Player");
        //  playerNoCollisionLayerInt = LayerMask.NameToLayer("PlayerNoCollision");


        audManager = ReferenceLibary.AudMng;

    }

    void FixedUpdate()
    {
        if (GameStateManager.gameState == GameStateManager.GameState.Start) return;
        if (gameMng.AllowMovement == false) return;
        if (shadowDash.isShadowDashing == true) return;   //dash.Boosting == true || 


        if (Input.GetButton("LeftBumper") && isSuperDashing == false)
        {
            if (playerMov.MovementDirection.normalized == Vector3.zero) return;

            isSuperDashing = true;
            SuperDashStarter();
            if (dash.IsBoosting == true)
                dash.IsBoosting = false;
        }
        else if (Input.GetButton("LeftBumper") == false && superDashNotPossible == true)
        {
            isSuperDashing = false;
            superDashNotPossible = false;
        }

#if UNITY_EDITOR

        if (Input.GetButton("Y") && isSuperDashing == false)
        {
            if (playerMov.MovementDirection.normalized == Vector3.zero) return;

            isSuperDashing = true;
            SuperDashStarter();
            if (dash.IsBoosting == true)
                dash.IsBoosting = false;
        }
        else if ( Input.GetButton("Y") && superDashNotPossible == true)
        {
            isSuperDashing = false;
            superDashNotPossible = false;
        }


#endif


        if (currentSuperDashForce != 0)
        {
            rb.AddForce(playerMov.MovementDirection.normalized * currentSuperDashForce * 400 * Time.fixedDeltaTime);

            // mr.enabled = true;
        }
        





    }

    #region SuperDash Coroutine

    public void SuperDashStarter()
    {
        if (superDashCoroutine != null)
            StopCoroutine(superDashCoroutine);

        ReferenceLibary.GameMng.InputMade();

        // if (EnergyManager.Instance.CheckForRequiredEnergyAmount(gameMng.SuperDashCosts) == true) // Wenn genügend Energy zur verfügung steht
        superDashCoroutine = StartCoroutine(SuperDashCoroutine());
       // else
        //    superDashNotPossible = true;

    }

     

    private IEnumerator SuperDashCoroutine()
    {
        StartCoroutine(ReferenceLibary.EnergyMng.ModifyEnergy(-gameMng.SuperDashCosts));

        if (audioSource.isPlaying == false)
        {
            audioSource.pitch = Random.Range(0.8f, 1.6f);
            audioSource.clip = startClip;
            audioSource.Play();
        }

        float t = 0;

        while (t < ShadowDashDuration)
        {

            t += Time.fixedDeltaTime;
            float curveValue = SuperDashcurve.Evaluate(t); // / ShadowDashDuration


            currentSuperDashForce += SuperDashForce * curveValue * Time.fixedDeltaTime;


            if (currentSuperDashForce >= disappearingDuringSuperDashStart && currentSuperDashForce <= disappearingDuringSuperDashEnd)  //
            {
                isDestroying = true;

                //EFFEKT
                if(effect.isPlaying == false)
                     effect.Play();

                //gameMng.AllowHexEffects = false;

            }

            if (gameMng.AllowMovement == false)
                break;

            yield return new WaitForFixedUpdate();
        }

       // Debug.Log("PlayingSecond");
        //audioSource.clip = endClip;
        //audioSource.Play();


        yield return new WaitForSeconds(0.3f);
        isDestroying = false;

        effect.Stop();

        rb.velocity = rb.velocity / 2;

        currentSuperDashForce = 0;
        isSuperDashing = false;


        //gameMng.AllowHexEffects = true;

    }
    #endregion
}
