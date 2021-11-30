using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.Mathematics;
public class CameraZoomOut : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private float xVelocity, zVelocity, xzVelocity;
    public CinemachineVirtualCamera vcam;
    public static CinemachineVirtualCamera vcamera;
    private float lerpedValue;
    private float cashedFov;
    private Transform lookAtCashed;
    private Transform targetAtCashed;
    
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

   private void Awake()
   {
       vcamera = vcam;
   }

   void Start()
   {
       
        _playerMovement = FindObjectOfType<PlayerMovement>();
        cashedFov = vcam.m_Lens.FieldOfView;
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
        
       // Debug.Log(lerpedValue);
        if(cashedFov+ZoomOutDelay < lerpedValue)
        vcam.m_Lens.FieldOfView = Mathf.Lerp(vcam.m_Lens.FieldOfView, lerpedValue, zoomOutRoughness*Time.deltaTime);
       // Debug.Log(zoomOutRoughness * Time.deltaTime);
     }
    }
    

    
}
