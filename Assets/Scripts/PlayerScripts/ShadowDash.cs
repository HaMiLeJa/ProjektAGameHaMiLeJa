using System.Collections;
using UnityEngine;
public class ShadowDash : MonoBehaviour
{
    #region Inspector
    [Space]
    private Coroutine shadowDashCoroutine;
    [SerializeField] private AnimationCurve shadowDashcurve;
    [SerializeField] public bool isShadowDashing = false; 
    private int playerLayerInt, playerNoCollisionLayerInt;
    private bool shadowDashNotPossible = false;//used to lock other boosts
    [SerializeField] private float ShadowDashForce, ShadowDashDuration;
    [SerializeField] public float  currentShadowDashForce = 0.0f, 
                                   disappearingDuringShadowDashStart, 
                                   disappearingDuringShadowDashEnd;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clip;
    public ParticleSystem particle;
    public MeshRenderer mr, mr2;
    Rigidbody rb;
    GameManager gameMng; PlayerBoost dash; PlayerSuperDash superDash; PlayerMovement playerMov;
    #endregion
    private void Awake()=>  mr = GetComponentInChildren<MeshRenderer>();
    private void Start()
    {
        rb = ReferenceLibrary.PlayerRb;
        dash = ReferenceLibrary.Dash;
        superDash = ReferenceLibrary.SuperDash;
        playerMov = ReferenceLibrary.PlayerMov;
        gameMng = ReferenceLibrary.GameMng;

        playerLayerInt = LayerMask.NameToLayer("Player");
        playerNoCollisionLayerInt = LayerMask.NameToLayer("PlayerNoCollision");
        audioSource.clip = clip;
    }
    void FixedUpdate()
    {
        if (GameStateManager.gameState == GameStateManager.GameState.Start || !gameMng.AllowMovement || superDash.isSuperDashing) return;
        if (!mr2.enabled && !mr.enabled && !particle.isPlaying) particle.Play(); else particle.Stop();
        
        if (Input.GetButton("RightBumper") && !isShadowDashing)
        {
            isShadowDashing = true;
            ShadowDashStarter();
            if (dash.IsBoosting) dash.IsBoosting = false;
        }
        if (currentShadowDashForce != 0 ) rb.AddForce(playerMov.MovementDirection.normalized * currentShadowDashForce * 400 * Time.fixedDeltaTime);
    }
    #region Shadowdash Coroutine
    public void ShadowDashStarter()
    {
        if (shadowDashCoroutine != null) StopCoroutine(shadowDashCoroutine);
        ReferenceLibrary.GameMng.InputMade();
        shadowDashCoroutine = StartCoroutine(ShadowDashCoroutine());
    }
    private IEnumerator ShadowDashCoroutine()
    {
        StartCoroutine(ReferenceLibrary.EnergyMng.ModifyEnergy(-gameMng.ShadowDashCosts));
        audioSource.pitch = Random.Range(0.8f, 1.6f);
        audioSource.PlayDelayed(0.1f);
        float t = 0;
        while (t < ShadowDashDuration)
        {
            mr.enabled = true; mr2.enabled = true;
            t += Time.fixedDeltaTime;
            float curveValue = shadowDashcurve.Evaluate(t); // / ShadowDashDuration
            currentShadowDashForce += ShadowDashForce * curveValue * Time.fixedDeltaTime;
            if (currentShadowDashForce >= disappearingDuringShadowDashStart 
                && currentShadowDashForce <= disappearingDuringShadowDashEnd) 
            {
                gameObject.layer = playerNoCollisionLayerInt;
                gameMng.AllowHexEffects = false; mr.enabled = false; mr2.enabled = false;
            }
            if (!gameMng.AllowMovement) break;
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(0.4f);
        rb.velocity = rb.velocity / 2;
        currentShadowDashForce = 0;
        isShadowDashing = false; gameMng.AllowHexEffects = true; mr.enabled = true; mr2.enabled = true;
        gameObject.layer = playerLayerInt;
    }
    #endregion
}
