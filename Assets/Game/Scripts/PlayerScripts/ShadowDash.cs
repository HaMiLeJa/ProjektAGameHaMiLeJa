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
    public ParticleSystem dust;

    public MeshRenderer mr;

    GameManager gameMng;
    PlayerBoost dash;
    PlayerSuperDash superDash;
    PlayerMovement playerMov;
    

    Rigidbody rb;

    [SerializeField] LayerMask worldMask;
    int playerLayerInt;
    int playerNoCollisionLayerInt;

   // bool colliding = true;
   // [SerializeField] SphereCollider myCollider;

    AudioManager audManager;
    [SerializeField] AudioSource audioSource;

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
        gameMng = GameManager.Instance;

        playerLayerInt = LayerMask.NameToLayer("Player");
        playerNoCollisionLayerInt = LayerMask.NameToLayer("PlayerNoCollision");

        //myCollider = this.gameObject.GetComponent<SphereCollider>();

        audManager = AudioManager.Instance;

    }

    void FixedUpdate()
    {
        if (gameMng.AllowMovement == false) return;
        if (dash.IsBoosting == true ||superDash.isSuperDashing == true) return;


        if (mr.enabled == false)
            dust.Play();
        else
        {
            dust.Stop();
        }
        
        #region ShadowDashInputKey
        
        if (Input.GetKeyDown(KeyCode.G) && isShadowDashing == false || Input.GetButton("RightBumper") && isShadowDashing == false)
        {
            isShadowDashing = true;
            ShadowDashStarter();
        }
        
        #endregion
        
        
        #region Add Dash to current Speed
        if (currentShadowDashForce != 0 )
        {
            rb.AddForce(playerMov.MovementDirection.normalized * currentShadowDashForce * 400 * Time.fixedDeltaTime);

            // mr.enabled = true;
        }
        #endregion


        


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
        
        shadowDashCoroutine = StartCoroutine(ShadowDashCoroutine());

        //this.gameObject.layer = playerNoCollisionLayerInt;
        
    }

    

    private IEnumerator ShadowDashCoroutine()
    {
       // Vector3 velocity = rb.velocity;
       
        EnergyManager.onEnergyChange?.Invoke(-gameMng.ShadowDashCosts);

        if (audioSource.isPlaying == false && audManager.allowAudio == true)
            audioSource.Play();

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

            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(0.3f);


        rb.velocity = rb.velocity / 2;

        currentShadowDashForce = 0;
        isShadowDashing = false;


        this.gameObject.layer = playerLayerInt;
        
       
        gameMng.AllowHexEffects = true;
        mr.enabled = true;

    }

    

   

    #endregion

  

    #region alternative 
    /*
        private IEnumerator ShadowDashCoroutineX()
        {
            Vector3 velocity = rb.velocity;

            float t = 0;

            //this.gameObject.layer = playerNoCollisionLayerInt;
            bool colliding = true;

            while (t < ShadowDashDuration)
            {

                mr.enabled = true;
                t += Time.deltaTime;
                float curveValue = shadowDashcurve.Evaluate(t); // / ShadowDashDuration


                currentShadowDashForce += ShadowDashForce * curveValue * Time.deltaTime;
                if (currentShadowDashForce >= disappearingDuringShadowDashStart && currentShadowDashForce <= disappearingDuringShadowDashEnd)
                {
                    mr.enabled = false;

                }
                yield return null;
            }


            rb.velocity = rb.velocity / 2;

            //rb.velocity = velocity;

            List<Collider> setToTriggerColliders = new List<Collider>();

            while (colliding == true)
            {
                Collider[] hitColliders;
                hitColliders = Physics.OverlapSphere(this.transform.position, myCollider.radius + 2, worldMask); //LayerMask.GetMask("World") // 7
                Debug.Log(hitColliders.Length);

                foreach (Collider col in hitColliders)
                {
                    col.isTrigger = true;


                    if (setToTriggerColliders.Contains(col) == false)
                        setToTriggerColliders.Add(col);
                    Debug.Log("set to trigger");
                }

                if (hitColliders.Length == 0)
                {
                    colliding = false;
                    Debug.Log("notColliding");
                    //this.gameObject.layer = playerLayerInt;
                }

                //yield return null;
            }

            foreach (Collider col in setToTriggerColliders)
            {
                col.isTrigger = false;
            }

            currentShadowDashForce = 0;
            isShadowDashing = false;
        }
    */
    #endregion


   
}
