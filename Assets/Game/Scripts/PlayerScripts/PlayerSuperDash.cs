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
        rb = this.GetComponent<Rigidbody>();

        dash = this.GetComponent<PlayerBoost>();
        playerMov = this.GetComponent<PlayerMovement>();
        gameMng = GameManager.Instance;
        shadowDash = this.GetComponent<ShadowDash>();
        //playerLayerInt = LayerMask.NameToLayer("Player");
        //  playerNoCollisionLayerInt = LayerMask.NameToLayer("PlayerNoCollision");


        audManager = AudioManager.Instance;

    }

    void FixedUpdate()
    {
        if (gameMng.AllowMovement == false) return;
        if (dash.IsBoosting == true || shadowDash.isShadowDashing == true) return;   //dash.Boosting == true || 
        



        #region ShadowDashInputKey

        if (Input.GetAxisRaw("LeftTrigger") != 0 || Input.GetButton("Y"))
        {
            Debug.Log("Key");
            isSuperDashing = true;
            SuperDashStarter();
        }

        #endregion


        #region Add Dash to current Speed
        if (currentSuperDashForce != 0)
        {
            rb.AddForce(playerMov.MovementDirection.normalized * currentSuperDashForce * 400 * Time.fixedDeltaTime);

            // mr.enabled = true;
        }
        #endregion





    }

    #region SuperDash Coroutine

    public void SuperDashStarter()
    {
        if (superDashCoroutine != null)
            StopCoroutine(superDashCoroutine);

        superDashCoroutine = StartCoroutine(SuperDashCoroutine());
        Debug.Log("Start");

    }



    private IEnumerator SuperDashCoroutine()
    {
        GameManager.Instance.onUIEnergyChange?.Invoke(-gameMng.ShadowDashCosts);

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
                Debug.Log("isDestroying");

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

        Debug.Log("End");

        gameMng.AllowHexEffects = true;

        GameManager.Instance.onEnergyChange?.Invoke(-gameMng.ShadowDashCosts);
    }
    #endregion
}
