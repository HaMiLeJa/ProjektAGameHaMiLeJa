using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowDash : MonoBehaviour
{
    #region Inspector
    
    [SerializeField] private float ShadowDashForce;
    [SerializeField] private float ShadowDashDuration;
    [SerializeField] private AnimationCurve shadowDashcurve;
    [SerializeField] private bool isShadowDashing = false;
    [SerializeField] private Coroutine shadowDashCoroutine;
    [SerializeField] private float currentShadowDashForce = 0.0f;
    [SerializeField] private float disappearingDuringShadowDashStart;
    [SerializeField] private float disappearingDuringShadowDashEnd;

    private MeshRenderer mr;
    
    #endregion
    
    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
    }
    void Update()
    {
        
        #region ShadowDashInputKey
        
        if (Input.GetKeyDown(KeyCode.G) && isShadowDashing == false)
        {
            isShadowDashing = true;
            ShadowDashStarter();
        }
        
        #endregion
        
        
        #region Add Dash to current Speed
        if (currentShadowDashForce != 0 )
        {
            
            //Janina ich möchte gerne das momentane mov speed mit currentShadowDashForce multiplizieren, ich weiß aber nicht wo ich das bei Playermov kann
            MOVEMENTSPEEDAUSPLAYERMOVEMENT *= currentShadowDashForce;
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
        float t = 0;
        // gameObject.layer = LayerMask.NameToLayer("PlayerDashing");
        while (t < ShadowDashDuration)
        {

            mr.enabled = true;
            t += Time.deltaTime;
            float curveValue = shadowDashcurve.Evaluate(t / ShadowDashDuration);
            currentShadowDashForce += ShadowDashForce * curveValue * Time.deltaTime;
            if (currentShadowDashForce >= disappearingDuringShadowDashStart && currentShadowDashForce <= disappearingDuringShadowDashEnd) 
                mr.enabled = false;

            yield return null;
        }

        currentShadowDashForce = 0;
        isShadowDashing = false;
        //  gameObject.layer = LayerMask.NameToLayer("Player");
    }
    
    #endregion
}
