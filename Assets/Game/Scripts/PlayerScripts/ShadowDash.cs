using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowDash : MonoBehaviour
{
    #region Inspector
    //[SerializeField] float boostCost = 1;
    [Space]
    [SerializeField] private float ShadowDashForce;
    [SerializeField] private float ShadowDashDuration;
    [SerializeField] private AnimationCurve shadowDashcurve;
    [SerializeField] public bool isShadowDashing = false; //used to lock other boosts
    [SerializeField] private Coroutine shadowDashCoroutine;
    [SerializeField] public float currentShadowDashForce = 0.0f;
    [SerializeField] private float disappearingDuringShadowDashStart;
    [SerializeField] private float disappearingDuringShadowDashEnd;
    [SerializeField] private GameObject Flashlight;
    public ParticleSystem particle;

    public MeshRenderer mr;

    GameManager gameMng;
    PlayerBoost dash;
    PlayerSuperDash superDash;
    PlayerMovement playerMov;

    bool shadowDashNotPossible = false;

    Rigidbody rb;

    [SerializeField] LayerMask worldMask;
    int playerLayerInt;
    int playerNoCollisionLayerInt;

   // bool colliding = true;
   // [SerializeField] SphereCollider myCollider;

    AudioManager audManager;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clip;

    #endregion

    private void Awake()
    {
        mr = GetComponentInChildren<MeshRenderer>();
    }

    private void Start()
    {
        rb = ReferenceLibary.RigidbodyPl;

        dash = ReferenceLibary.Dash;
        superDash = ReferenceLibary.SuperDash;
        playerMov = ReferenceLibary.PlayerMov;
        gameMng = ReferenceLibary.GameMng;

        playerLayerInt = LayerMask.NameToLayer("Player");
        playerNoCollisionLayerInt = LayerMask.NameToLayer("PlayerNoCollision");

        //myCollider = this.gameObject.GetComponent<SphereCollider>();

        audManager = ReferenceLibary.AudMng;
        audioSource.clip = clip;

    }

    void FixedUpdate()
    {
        if (gameMng.AllowMovement == false) return;
        if (superDash.isSuperDashing == true) return;


        if (mr.enabled == false)
            particle.Play();
        else
        {
           // particle.Stop();
        }


        if (Input.GetButton("RightBumper") && isShadowDashing == false)
        {
            isShadowDashing = true;
            //Flashlight.SetActive(true);
            ShadowDashStarter();

            if (dash.IsBoosting == true)
                dash.IsBoosting = false;
        }
     
       


        if (currentShadowDashForce != 0 )
        {
            rb.AddForce(playerMov.MovementDirection.normalized * currentShadowDashForce * 400 * Time.fixedDeltaTime);

            // mr.enabled = true;
        }
        


        


    }
    #region Shadowdash Coroutine


    #region Verbuggtes CheckForCollision

    /*
     *  bool EndShadowDash = false;
     * void CheckForCollsion()
      {
          while (colliding == true)
          {
              Collider[] hitColliders;

              hitColliders = Physics.OverlapSphere(this.transform.position, myCollider.radius + 1, LayerMask.GetMask("World")); //LayerMask.GetMask("World")


              if (hitColliders.Length == 0)
              {

                  this.gameObject.layer = playerLayerInt;
                  colliding = false;
                  // Debug.Log("notColliding");

                  gameMng.AllowHexEffects = true;
                  mr.enabled = true;
              }
              else
              {
                  Debug.Log("Colliding");
              }
          }
      } */
    #endregion

    public void ShadowDashStarter()
    {
        if (shadowDashCoroutine != null)
             StopCoroutine(shadowDashCoroutine);

        ReferenceLibary.GameMng.InputMade();
        shadowDashCoroutine = StartCoroutine(ShadowDashCoroutine());
  

        //this.gameObject.layer = playerNoCollisionLayerInt;

    }

    

    private IEnumerator ShadowDashCoroutine()
    {
        

        StartCoroutine(ReferenceLibary.EnergyMng.ModifyEnergy(-gameMng.ShadowDashCosts));

        // if (audioSource.isPlaying == false)

        audioSource.pitch = Random.Range(0.8f, 1.6f);
        audioSource.PlayDelayed(0.1f);

        float t = 0;

        

        while (t < ShadowDashDuration)
        {

            mr.enabled = true;
            t += Time.fixedDeltaTime;
            float curveValue = shadowDashcurve.Evaluate(t); // / ShadowDashDuration


            currentShadowDashForce += ShadowDashForce * curveValue * Time.fixedDeltaTime;

            
            if (currentShadowDashForce >= disappearingDuringShadowDashStart && currentShadowDashForce <= disappearingDuringShadowDashEnd)  //
            {
                this.gameObject.layer = playerNoCollisionLayerInt;
                

                mr.enabled = false;
                gameMng.AllowHexEffects = false;

            }

            if (gameMng.AllowMovement == false)
                break;

            yield return new WaitForFixedUpdate();
        }




        yield return new WaitForSeconds(0.4f);


        rb.velocity = rb.velocity / 2;

        currentShadowDashForce = 0;
       // Flashlight.SetActive(false);
        isShadowDashing = false;


        this.gameObject.layer = playerLayerInt;
        
       
        gameMng.AllowHexEffects = true;
        mr.enabled = true;

    }

    

   

    #endregion

  

   


   
}
