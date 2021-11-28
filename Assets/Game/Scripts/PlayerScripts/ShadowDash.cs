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
    PlayerStartDash superDash;
    

    Rigidbody rb;

    [SerializeField] LayerMask worldMask;
    int playerLayerInt;
    int playerNoCollisionLayerInt;

    bool colliding = true;
    [SerializeField] SphereCollider myCollider;

    AudioManager audManager;
    [SerializeField] AudioSource audioSource;

    #endregion

    private void Awake()
    {
        mr = GetComponentInChildren<MeshRenderer>();
    }

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();

        dash = this.GetComponent<PlayerBoost>();
        superDash = this.GetComponent<PlayerStartDash>();

        gameMng = GameManager.Instance;

        playerLayerInt = LayerMask.NameToLayer("Player");
        playerNoCollisionLayerInt = LayerMask.NameToLayer("PlayerNoCollision");

        //myCollider = this.gameObject.GetComponent<SphereCollider>();

        audManager = AudioManager.Instance;

    }

    void FixedUpdate()
    {
        if (gameMng.AllowMovement == false) return;
        if (dash.IsBoosting == true ||superDash.Boosting == true) return;   //dash.Boosting == true || 
        //if (rb.velocity.x == 0 || rb.velocity.z == 0) return; //kein kleiner Boost am Anfang erlaubt!


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



            //Janina ich möchte gerne das momentane mov speed mit currentShadowDashForce multiplizieren, ich weiß aber nicht wo ich das bei Playermov kann
            //   ---->>>> MOVEMENTSPEEDAUSPLAYERMOVEMENT  <<<<----- *= currentShadowDashForce;
            //rb.velocity *= currentShadowDashForce;

            mr.enabled = true;
        }
        #endregion

    }

    #region Shadowdash Coroutine
    
    public void ShadowDashStarter()
    {
        if (shadowDashCoroutine != null)
         StopCoroutine(shadowDashCoroutine);
        
        shadowDashCoroutine = StartCoroutine(ShadowDashCoroutine());
    }

    

    private IEnumerator ShadowDashCoroutine()
    {
        Vector3 velocity = rb.velocity;
        GameManager.Instance.onUIEnergyChange?.Invoke(-gameMng.ShadowDashCosts);

        if (audioSource.isPlaying == false && audManager.allowAudio == true)
            audioSource.Play();

        float t = 0;

        this.gameObject.layer = playerNoCollisionLayerInt;
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
                gameMng.AllowHexEffects = false;

            }
            yield return null;
        }


        rb.velocity = rb.velocity / 2;

        //rb.velocity = velocity;


        
        while (colliding == true)
        {
            Collider[] hitColliders;

            hitColliders = Physics.OverlapSphere(this.transform.position, myCollider.radius + 1, LayerMask.GetMask("World")); //LayerMask.GetMask("World")
            

            if (hitColliders.Length == 0)
            {
                colliding = false;
                Debug.Log("notColliding");
                this.gameObject.layer = playerLayerInt;
                gameMng.AllowHexEffects = true;
            }
        }
        




        currentShadowDashForce = 0;
        isShadowDashing = false;
        GameManager.Instance.onEnergyChange?.Invoke(-gameMng.ShadowDashCosts);
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(this.transform.position, myCollider.radius + 1);
    }

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
