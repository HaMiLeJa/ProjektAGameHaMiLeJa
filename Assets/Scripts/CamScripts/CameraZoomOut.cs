using System.Collections;
using UnityEngine;
using Cinemachine;
using Unity.Mathematics;
using Random = UnityEngine.Random;
using NaughtyAttributes;
public class CameraZoomOut : MonoBehaviour
{
    [Header("GameObjects")]
    [Space]
    public static CinemachineVirtualCamera vcamera;
    public CinemachineVirtualCamera vcam, vcamZoom;
    private Transform lookAtCashed, targetAtCashed;
    public GameObject Moon, GhostLayer;
    private float 
        xVelocity, zVelocity, xzVelocity,
        lerpedValue, cashedFov,
        lerpedValueMoonX, lerpedValueMoonZ, lerpedValueGhostLayerX, lerpedValueGhostLayerZ,
        cashedXScaleMoon, cashedZScaleMoon, cashedXScaleGhostLayer, cashedZScaleGhostLayer;

    [Header("Camera Zoomout")]
    [Space]
    [Tooltip("Wie weit soll es rauszoomen")] [Range(50,300)][SerializeField] private float maxFov = 115;

    [MinMaxSlider(0, 1500f)] [Tooltip("Ab wann und bis wann wird gezoomed abhängig vom movement ")] 
    [SerializeField] private Vector2 ZoomingValue = new Vector2(0.01f, 300f);
    [Range(0.01f, 20)]
   [Tooltip("Wie Smooth soll die Kamera zwischen den Werten Lerpen")] [SerializeField] private float zoomOutRoughness = 2;
   [Range(0, 50)]
   [Tooltip("Davor wird nichts gemacht")] [SerializeField] private float ZoomOutDelay = 3;
   [Range(0, 30)]
   [Tooltip("Vertikale und Horizontale Achse start zoom")] [SerializeField] private float HorizontalVerticalStartZoom = 3;

   [Header("Moon und GhostLayer")]
   [Space]
   [Range(0.01f, 20)]
   [Tooltip("Wie Smooth soll die Kamera zwischen den Werten Lerpen")] [SerializeField] private float moonZoomOutRoughness = 2;
   [SerializeField] private float addXScaleMoon = 5;
   [SerializeField] private float addZScaleMoon = 5;
   [Range(0.01f, 20)]
  [Tooltip("Wie Smooth soll die Kamera zwischen den Werten Lerpen")] [SerializeField] private float ghostLayerZoomOutRoughness = 2;
  [SerializeField] private float addXScaleGhostLayer = 10;
  [SerializeField] private float addZScaleGhostLayer = 10;

  [Space] [Space] [SerializeField] private CameraShake _cameraShake;
 private float shakeAngle, shakeStrength, shakeSpeed, shakeDuration, shakeNoisePercent, shakeDampingPercet, shakeRotationPercent;
 
 [Header("Camera Shake Management")] [Space]
 [Tooltip("Für das optionsmenü")] [SerializeField] private bool deactivateShaking = false;
 [Tooltip("Ab wann started das shaken")] [Range(0f, 300)] [SerializeField] private float  StartShakingAtVelocity = 90f;
 [Tooltip("Je höher die zahl, desto weniger wirken alle effekte")] [Range(0.02f, 1200)] [SerializeField] private float SpeedInflunceDampeningForAll = 300;
 [Space]
 [Space] [MinMaxSlider(0, 1)]  [Tooltip("Dampening nach dem overall Dampening zum finetunen")] [SerializeField] private Vector2 ShakeDamping  = new Vector2(0.21f, 0.53f);
 [Space] [MinMaxSlider(0, 1)] [SerializeField] private Vector2 ShakeNoise = new Vector2(0.23f, 0.56f);
 [Space] [Space] [MinMaxSlider(0, 1)] [SerializeField] private Vector2 ShakeRotation = new Vector2(0.26f, 0.84f);
 [Space] [MinMaxSlider(0, 2) ] [SerializeField] private Vector2 ShakeStrength = new Vector2(0.07f, 0.56f);
 [Space] [MinMaxSlider(0, 5)] [SerializeField] private Vector2 ShakeDuration= new Vector2(0.61f, 1.32f);
 [Space][MinMaxSlider(0, 20)] [SerializeField] private Vector2 ShakeSpeed =  new Vector2(0.53f, 1.8f);

 [SerializeField] private float secNextShakeAllowed;
 [SerializeField] private bool nextShakeAllowed = true;
 [SerializeField]  private CameraShakeCollision _cameraShakeCollision;
 private void Awake()
 {
     vcamera = vcam;
     GameManager.LerpCameraBack = false;
 }
 void Start()
   {
       cashedFov = vcam.m_Lens.FieldOfView;
        cashedXScaleMoon = Moon.transform.localScale.x;
        cashedZScaleMoon = Moon.transform.localScale.z;
        cashedXScaleGhostLayer = GhostLayer.transform.localScale.x;
        cashedZScaleGhostLayer = GhostLayer.transform.localScale.z;
   }

 private void FixedUpdate()
 {
     xVelocity = math.abs(ReferenceLibrary.PlayerRb.velocity.x);
     zVelocity = math.abs(ReferenceLibrary.PlayerRb.velocity.z);
     xzVelocity = xVelocity + zVelocity;
     if (xzVelocity < zVelocity + HorizontalVerticalStartZoom) xzVelocity = zVelocity*2;
     if (xzVelocity < xVelocity + HorizontalVerticalStartZoom) xzVelocity = xVelocity*2;
     lerpedValue = MathLibary.RemapClamped( ZoomingValue.x, ZoomingValue.y, cashedFov, maxFov, xzVelocity);
     if (cashedFov + ZoomOutDelay < lerpedValue) vcam.m_Lens.FieldOfView = Mathf.Lerp(vcam.m_Lens.FieldOfView, lerpedValue, zoomOutRoughness * Time.deltaTime);
     if(_cameraShakeCollision.camShakeActivated && !deactivateShaking && nextShakeAllowed && !GameManager.bridgePause && xzVelocity > StartShakingAtVelocity)
     {
         shakeAngle = Random.Range(1,9);
         shakeStrength = MathLibary.RemapClamped(ZoomingValue.x, SpeedInflunceDampeningForAll, ShakeStrength.x,
             ShakeStrength.y, xzVelocity);
         shakeDuration = MathLibary.RemapClamped(ZoomingValue.x, SpeedInflunceDampeningForAll, ShakeDuration.x,
             ShakeDuration.y, xzVelocity);
         shakeSpeed = MathLibary.RemapClamped(ZoomingValue.x, SpeedInflunceDampeningForAll, ShakeSpeed.y, 
             ShakeSpeed.x, xzVelocity);
         shakeNoisePercent = MathLibary.RemapClamped(ZoomingValue.x, SpeedInflunceDampeningForAll, ShakeNoise.x,
             ShakeNoise.y, xzVelocity);
         shakeDampingPercet = MathLibary.RemapClamped(ZoomingValue.x, SpeedInflunceDampeningForAll, ShakeDamping.x,
             ShakeDamping.y, xzVelocity);
         shakeRotationPercent = MathLibary.RemapClamped(ZoomingValue.x, SpeedInflunceDampeningForAll, ShakeRotation.x,
             ShakeRotation.y, xzVelocity);
                
         _cameraShake.StartShake(new CameraShake.Einstellungen(shakeAngle, shakeStrength, shakeSpeed, 
             shakeDuration, shakeNoisePercent, shakeDampingPercet, shakeRotationPercent));
         StartCoroutine(Coroutine_TimeBetweenShakes());
     }

     if (Input.GetKeyDown(KeyCode.M))
     {
         shakeAngle = Random.Range(1,9);
         shakeStrength = MathLibary.RemapClamped(ZoomingValue.x, SpeedInflunceDampeningForAll, ShakeStrength.x,
             ShakeStrength.y, xzVelocity);
         shakeDuration = MathLibary.RemapClamped(ZoomingValue.x, SpeedInflunceDampeningForAll, ShakeDuration.x,
             ShakeDuration.y, xzVelocity);
         shakeSpeed = MathLibary.RemapClamped(ZoomingValue.x, SpeedInflunceDampeningForAll, ShakeSpeed.y, 
             ShakeSpeed.x, xzVelocity);
         shakeNoisePercent = MathLibary.RemapClamped(ZoomingValue.x, SpeedInflunceDampeningForAll, ShakeNoise.x,
             ShakeNoise.y, xzVelocity);
         shakeDampingPercet = MathLibary.RemapClamped(ZoomingValue.x, SpeedInflunceDampeningForAll, ShakeDamping.x,
             ShakeDamping.y, xzVelocity);
         shakeRotationPercent = MathLibary.RemapClamped(ZoomingValue.x, SpeedInflunceDampeningForAll, ShakeRotation.x,
             ShakeRotation.y, xzVelocity);
                
         _cameraShake.StartShake(new CameraShake.Einstellungen(shakeAngle, shakeStrength, shakeSpeed, 
             shakeDuration, shakeNoisePercent, shakeDampingPercet, shakeRotationPercent));
         StartCoroutine(Coroutine_TimeBetweenShakes());
     }
     _cameraShakeCollision.camShakeActivated = false;
 }

 private float xVelocityUpdate, zVelocityUpdate, xzVelocityUpdate;
 void Update() 
    {
        xVelocityUpdate = math.abs(ReferenceLibrary.PlayerRb.velocity.x);
        zVelocityUpdate = math.abs(ReferenceLibrary.PlayerRb.velocity.z);
        xzVelocityUpdate= xVelocityUpdate + zVelocityUpdate;
        if (xzVelocityUpdate < zVelocityUpdate + HorizontalVerticalStartZoom) xzVelocityUpdate = zVelocity*2;
        if (xzVelocityUpdate < xVelocityUpdate + HorizontalVerticalStartZoom) xzVelocityUpdate = xVelocityUpdate*2;
        
        if(!PortalManager.CameraTeleportActive)
     {
         lerpedValueMoonX = MathLibary.RemapClamped( ZoomingValue.x, ZoomingValue.y, cashedXScaleMoon, cashedXScaleMoon+ addXScaleMoon, xzVelocityUpdate);
            lerpedValueMoonZ = MathLibary.RemapClamped( ZoomingValue.x, ZoomingValue.y, cashedZScaleMoon, cashedZScaleMoon+ addZScaleMoon, xzVelocityUpdate);
            lerpedValueGhostLayerX = MathLibary.RemapClamped( ZoomingValue.x, ZoomingValue.y, cashedXScaleGhostLayer, cashedXScaleGhostLayer+ addXScaleGhostLayer, xzVelocityUpdate);
            lerpedValueGhostLayerZ = MathLibary.RemapClamped( ZoomingValue.x, ZoomingValue.y, cashedZScaleGhostLayer, cashedZScaleGhostLayer+ addZScaleGhostLayer, xzVelocityUpdate);
            if (cashedFov + ZoomOutDelay < lerpedValue)
       {
           vcam.m_Lens.FieldOfView = Mathf.Lerp(vcam.m_Lens.FieldOfView, lerpedValue, zoomOutRoughness*Time.deltaTime);
           Moon.transform.localScale = new Vector3(
                   Mathf.Lerp(Moon.transform.localScale.x, lerpedValueMoonX, moonZoomOutRoughness*Time.deltaTime),
                   Moon.transform.localScale.y ,
                   Mathf.Lerp(Moon.transform.localScale.z, lerpedValueMoonZ, moonZoomOutRoughness*Time.deltaTime)
               ) ;
           if ( GameManager.StartMovingGhostLayer && !GameManager.LerpCameraBack)
           {
               GhostLayer.transform.localScale = new Vector3(
                   Mathf.Lerp(GhostLayer.transform.localScale.x, 4.3f, ghostLayerZoomOutRoughness*Time.deltaTime),
                   Mathf.Lerp(GhostLayer.transform.localScale.y, 4.3f, ghostLayerZoomOutRoughness*Time.deltaTime),
                   Mathf.Lerp(GhostLayer.transform.localScale.z, 4.3f, ghostLayerZoomOutRoughness*Time.deltaTime)
               );
               Moon.transform.localScale = new Vector3(
                  Moon.transform.localScale.x,
                   Mathf.Lerp(Moon.transform.localScale.y, 0.1f, ghostLayerZoomOutRoughness*Time.deltaTime),
                  Moon.transform.localScale.z
               );
           }
           if (Mathf.Abs(GhostLayer.transform.localScale.x - 3) < 0.01f &&  (Mathf.Abs(Moon.transform.localScale.y -19.65329f) < 0.01f)) GameManager.LerpCameraBack = false;
           if (GameManager.LerpCameraBack && Mathf.Abs(GhostLayer.transform.localScale.x - 3) > 0.01f)
           {
               GhostLayer.transform.localScale = new Vector3(
                   Mathf.Lerp(GhostLayer.transform.localScale.x, 3,
                       ghostLayerZoomOutRoughness * Time.deltaTime),
                   Mathf.Lerp(GhostLayer.transform.localScale.y, 3,
                       ghostLayerZoomOutRoughness * Time.deltaTime),
                   Mathf.Lerp(GhostLayer.transform.localScale.z, 3,
                       ghostLayerZoomOutRoughness * Time.deltaTime)
               );
               Moon.transform.localScale = new Vector3(
                   Moon.transform.localScale.x,
                   Mathf.Lerp(Moon.transform.localScale.y, 19.65329f, ghostLayerZoomOutRoughness*Time.deltaTime),
                   Moon.transform.localScale.z
               );
           }
       }
     }
        
    }
    IEnumerator Coroutine_TimeBetweenShakes()
    {
        if (secNextShakeAllowed < 0) secNextShakeAllowed = 0;
        nextShakeAllowed = false;
        yield return new WaitForSeconds(secNextShakeAllowed);
        nextShakeAllowed = true;
    }
}