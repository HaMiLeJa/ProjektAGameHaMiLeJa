using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowDash : MonoBehaviour
{
    #region Inspector
    
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

    #endregion
    
    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();

        dash = this.GetComponent<PlayerBoost>();
        superDash = this.GetComponent<PlayerStartDash>();

        gameMng = FindObjectOfType<GameManager>();
    }

    void FixedUpdate()
    {
        if (dash.IsBoosting == true ||superDash.Boosting == true) return;   //dash.Boosting == true || 
        if (rb.velocity.x == 0 || rb.velocity.z == 0) return; //kein kleiner Boost am Anfang erlaubt!


        if (mr.enabled == false)
            dust.Play();
        else
        {
            dust.Stop();
        }
        
        #region ShadowDashInputKey
        
        if (Input.GetKeyDown(KeyCode.G) && isShadowDashing == false || Input.GetButton(gameMng.ShadowDash) && isShadowDashing == false)
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

        float t = 0;
        // gameObject.layer = LayerMask.NameToLayer("PlayerDashing");
        while (t < ShadowDashDuration)
        {
           
            mr.enabled = true;
            t += Time.deltaTime;
            float curveValue = shadowDashcurve.Evaluate(t ); // / ShadowDashDuration


            currentShadowDashForce += ShadowDashForce * curveValue * Time.deltaTime; 
            if (currentShadowDashForce >= disappearingDuringShadowDashStart && currentShadowDashForce <= disappearingDuringShadowDashEnd) 
            {
                mr.enabled = false;
       
            }
            yield return null;
        }


        rb.velocity = rb.velocity / 2;

        //rb.velocity = velocity;

        currentShadowDashForce = 0;
        isShadowDashing = false;
        //  gameObject.layer = LayerMask.NameToLayer("Player");
    }
    
    #endregion
}
