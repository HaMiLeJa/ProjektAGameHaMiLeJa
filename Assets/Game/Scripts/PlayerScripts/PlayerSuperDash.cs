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
    [SerializeField] private Coroutine superDashCoroutine;
    [SerializeField] public float currentSuperDashForce = 0.0f;
    [SerializeField] private float disappearingDuringSuperDashStart;
    [SerializeField] private float disappearingDuringSuperDashEnd; 
    public ParticleSystem effect;

    public bool isDestroying = false;
    bool superDashNotPossible = false;

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

    #endregion


    private void Start()
    {
        rb = ReferenceLibary.RigidbodyPl;

        dash = ReferenceLibary.Dash;
        playerMov = ReferenceLibary.PlayerMov;
        gameMng = GameManager.Instance;
        shadowDash = ReferenceLibary.ShadowDashPl;

        //playerLayerInt = LayerMask.NameToLayer("Player");
        //  playerNoCollisionLayerInt = LayerMask.NameToLayer("PlayerNoCollision");


        audManager = AudioManager.Instance;

    }

    void FixedUpdate()
    {
        if (gameMng.AllowMovement == false) return;
        if (shadowDash.isShadowDashing == true) return;   //dash.Boosting == true || 





        if (Input.GetButton("LeftBumper") && isSuperDashing == false || Input.GetButton("Y") && isSuperDashing == false)
        {
            Debug.Log("Key");
            isSuperDashing = true;
            SuperDashStarter();

            if (dash.IsBoosting == true)
                dash.IsBoosting = false;
        }
        else if (Input.GetButton("LeftBumper") == false && superDashNotPossible == true || Input.GetButton("Y") && superDashNotPossible == true)
        {
            isSuperDashing = false;
            superDashNotPossible = false;
        }
        


           
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

        Debug.Log(EnergyManager.CurrentEnergy);

        if (EnergyManager.Instance.CheckForRequiredEnergyAmount(gameMng.SuperDashCosts) == true) // Wenn gen�gend Energy zur verf�gung steht
            superDashCoroutine = StartCoroutine(SuperDashCoroutine());
        else
            superDashNotPossible = true;

    }


    private IEnumerator SuperDashCoroutine()
    {
       EnergyManager.onEnergyChange?.Invoke(-gameMng.SuperDashCosts);

        if (audioSource.isPlaying == false && audManager.allowAudio == true)
            audioSource.Play();

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

                gameMng.AllowHexEffects = false;

            }

            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(0.3f);
        isDestroying = false;

        effect.Stop();

        rb.velocity = rb.velocity / 2;

        currentSuperDashForce = 0;
        isSuperDashing = false;


        gameMng.AllowHexEffects = true;

    }
    #endregion
}
