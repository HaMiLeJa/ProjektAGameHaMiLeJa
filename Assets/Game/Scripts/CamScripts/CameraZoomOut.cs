using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class CameraZoomOut : MonoBehaviour
{
    [Header("GameObjects")]
    [Space]
    [SerializeField]private PlayerMovement _playerMovement;
    public CinemachineVirtualCamera vcam;
    public static CinemachineVirtualCamera vcamera;
    
    private Transform lookAtCashed;
    private Transform targetAtCashed;
    public GameObject Moon;
    public GameObject GhostLayer;

    private float 
        xVelocity, zVelocity, xzVelocity,
        lerpedValue, cashedFov,
        
        lerpedValueMoonX, lerpedValueMoonZ, lerpedValueGhostLayerX, lerpedValueGhostLayerZ,
        cashedXScaleMoon, cashedZScaleMoon, cashedXScaleGhostLayer, cashedZScaleGhostLayer;
    

    [Header("Camera Zoomout")]
    [Space]
    
   [Tooltip("Wie weit soll es rauszoomen")] [SerializeField] private float maxFov = 110;
   [Tooltip("Ab wann wird gezoomed abhängig vom movement")] [SerializeField] private float  StartZoomingValue = 0.01f;
   [Tooltip("Bis wann wird gezoomed abhängig vom movement")] [SerializeField] private float StopZoomingValue = 100;
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

  [Space] [Space] private CameraShake _cameraShake;
 private float shakeAngle, shakeStrength, shakeSpeed, shakeDuration, shakeNoisePercent, shakeDampingPercet, shakeRotationPercent;

 [Header("Camera Shake Management")] [Space]
 [Tooltip("Für das optionsmenü")] [SerializeField] private bool deactivateShaking = false;
 [Tooltip("Ab wann started das shaken")] [Range(0f, 300)] [SerializeField] private float  StartShaking = 90f;
 [Tooltip("Je höher die zahl, desto weniger wirken alle effekte")] [Range(0.02f, 1200)] [SerializeField] private float SpeedInflunceDampeningForAll = 300;
 [Space]
 [Space]
 [Tooltip("Dampening nach dem overall Dampening zum finetunen")][Range(0f, 1)] [SerializeField] private float minShakeDamping = 0.21f;
 [Tooltip("Dampening nach dem overall Dampening zum finetunen")][Range(0f, 1)] [SerializeField] private float maxShakeDamping = 0.53f;
 
 [Space]
 [Range(0f, 1)] [SerializeField] private float minShakeNoise = 0.23f;
 [Range(0f, 1)] [SerializeField] private float maxShakeNoise = 0.56f;
 
 [Space]
 [Range(0f, 1)] [SerializeField] private float minShakeRotation = 0.26f;
 [Range(0f, 1)] [SerializeField] private float maxShakeRotation = 0.84f;
 
 [Space]
 [Range(0f, 2)] [SerializeField] private float minShakeStrength =0.07f;
 [Range(0f, 2)] [SerializeField] private float maxShakeStrength = 0.56f;

 [Space]
 [Range(0f, 5)] [SerializeField] private float minShakeDuration = 0.61f;
 [Range(0f, 5)] [SerializeField] private float maxShakeDuration = 1.32f;
 
 [Space]
 [Range(0f, 8)] [SerializeField] private float minShakeSpeed = 1.8f;
 [Range(0f, 8)] [SerializeField] private float maxShakeSpeed = 0.53f;
 
 [SerializeField] private float secNextShakeAllowed;
 [SerializeField] private bool nextShakeAllowed = true;
 private CameraShakeCollision _cameraShakeCollision;
 private void Awake()
 {
   _cameraShakeCollision = FindObjectOfType<CameraShakeCollision>();
     vcamera = vcam;
 }
 
 
 void Start()
   {
       cashedFov = vcam.m_Lens.FieldOfView;
        cashedXScaleMoon = Moon.transform.localScale.x;
        cashedZScaleMoon = Moon.transform.localScale.z;
        cashedXScaleGhostLayer = GhostLayer.transform.localScale.x;
        cashedZScaleGhostLayer = GhostLayer.transform.localScale.z;
        _cameraShake = FindObjectOfType<CameraShake>();
   }
 void FixedUpdate()
    {
     if(!GameManager.CameraTeleportActive)
     {
        xVelocity = math.abs(_playerMovement.rb.velocity.x);
      //  Debug.Log("x Velocity"+ xVelocity);
        zVelocity = math.abs(_playerMovement.rb.velocity.z);
      //  Debug.Log("z Velocity"+ zVelocity);
        xzVelocity = xVelocity + zVelocity;
        if (xzVelocity < zVelocity + HorizontalVerticalStartZoom)
            xzVelocity = zVelocity*2;
        if (xzVelocity < xVelocity + HorizontalVerticalStartZoom)
            xzVelocity = xVelocity*2;

            lerpedValue = MathLibary.RemapClamped( StartZoomingValue, StopZoomingValue, cashedFov, maxFov, xzVelocity);
        
            lerpedValueMoonX = MathLibary.RemapClamped( StartZoomingValue, StopZoomingValue, cashedXScaleMoon, cashedXScaleMoon+ addXScaleMoon, xzVelocity);
            lerpedValueMoonZ = MathLibary.RemapClamped( StartZoomingValue, StopZoomingValue, cashedZScaleMoon, cashedZScaleMoon+ addZScaleMoon, xzVelocity);
            
            lerpedValueGhostLayerX = MathLibary.RemapClamped( StartZoomingValue, StopZoomingValue, cashedXScaleGhostLayer, cashedXScaleGhostLayer+ addXScaleGhostLayer, xzVelocity);
            lerpedValueGhostLayerZ = MathLibary.RemapClamped( StartZoomingValue, StopZoomingValue, cashedZScaleGhostLayer, cashedZScaleGhostLayer+ addZScaleGhostLayer, xzVelocity);
       // Debug.Log(lerpedValue);
       
       if (cashedFov + ZoomOutDelay < lerpedValue)
       {
           vcam.m_Lens.FieldOfView = Mathf.Lerp(vcam.m_Lens.FieldOfView, lerpedValue, zoomOutRoughness*Time.deltaTime);
           
           Moon.transform.localScale = new Vector3(
                   Mathf.Lerp(Moon.transform.localScale.x, lerpedValueMoonX, moonZoomOutRoughness*Time.deltaTime),
                   Moon.transform.localScale.y ,
                   Mathf.Lerp(Moon.transform.localScale.z, lerpedValueMoonZ, moonZoomOutRoughness*Time.deltaTime)
               )  ;
               
               GhostLayer.transform.localScale = new Vector3(
                                  Mathf.Lerp(GhostLayer.transform.localScale.x, lerpedValueGhostLayerX, ghostLayerZoomOutRoughness*Time.deltaTime),
                                  GhostLayer.transform.localScale.y ,
                                  Mathf.Lerp(GhostLayer.transform.localScale.z, lerpedValueGhostLayerZ, ghostLayerZoomOutRoughness*Time.deltaTime)
                              );
       }
     }
    }
    void Update() 
    {
        xVelocity = math.abs(_playerMovement.rb.velocity.x);
            zVelocity = math.abs(_playerMovement.rb.velocity.z);
            xzVelocity = xVelocity + zVelocity;
            if (xzVelocity < zVelocity + HorizontalVerticalStartZoom)
                xzVelocity = zVelocity * 2;
            if (xzVelocity < xVelocity + HorizontalVerticalStartZoom)
                xzVelocity = xVelocity * 2;
            
            shakeAngle = Random.Range(1,9);
            shakeStrength = MathLibary.RemapClamped(StartZoomingValue, SpeedInflunceDampeningForAll, minShakeStrength,
                maxShakeStrength, xzVelocity);
            shakeDuration = MathLibary.RemapClamped(StartZoomingValue, SpeedInflunceDampeningForAll, minShakeDuration,
                maxShakeDuration, xzVelocity);

            shakeSpeed = MathLibary.RemapClamped(StartZoomingValue, SpeedInflunceDampeningForAll, minShakeSpeed, maxShakeSpeed,
                xzVelocity);
            shakeNoisePercent = MathLibary.RemapClamped(StartZoomingValue, SpeedInflunceDampeningForAll, minShakeNoise,
                maxShakeNoise, xzVelocity);

            shakeDampingPercet = MathLibary.RemapClamped(StartZoomingValue, SpeedInflunceDampeningForAll, minShakeDamping,
                maxShakeDamping, xzVelocity);
            shakeRotationPercent = MathLibary.RemapClamped(StartZoomingValue, SpeedInflunceDampeningForAll, minShakeRotation,
                maxShakeRotation, xzVelocity);
            
          if(_cameraShakeCollision.camShakeActivated && ! deactivateShaking && xzVelocity > StartShaking && nextShakeAllowed)
            {
            _cameraShake.StartShake(new CameraShake.Einstellungen(shakeAngle, shakeStrength, shakeSpeed, 
                shakeDuration, shakeNoisePercent, shakeDampingPercet, shakeRotationPercent));
            StartCoroutine(Coroutine_TimeBetweenShakes());
            }
          _cameraShakeCollision.camShakeActivated = false;
    }
    IEnumerator Coroutine_TimeBetweenShakes()

    {
        if (secNextShakeAllowed < 0)
            secNextShakeAllowed = 0;
        nextShakeAllowed = false;
        yield return new WaitForSeconds(secNextShakeAllowed);
        nextShakeAllowed = true;
    }
}

    
    

