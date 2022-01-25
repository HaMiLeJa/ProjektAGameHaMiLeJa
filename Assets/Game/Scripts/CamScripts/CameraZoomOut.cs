using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine.Rendering.Universal;
public class CameraZoomOut : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    private float xVelocity, zVelocity, xzVelocity;
    public CinemachineVirtualCamera vcam;
    public static CinemachineVirtualCamera vcamera;
    private float lerpedValue;
    private float cashedFov;
    private Transform lookAtCashed;
    private Transform targetAtCashed;
    public GameObject ghostLayer;
    private float lerpedValueGhostLayerX;
    private float lerpedValueGhostLayerZ;
    private float cashedXScale;
    private float cashedZScale;

    [SerializeField] private float addXScaleGhostlayer;
    [SerializeField] private float addZScaleGhostlayer;
    

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


 [Range(0.01f, 20)]
   [Tooltip("Wie Smooth soll die Kamera zwischen den Werten Lerpen")] [SerializeField] private float moonZoomOutRoughness = 2;
   private void Awake()
   {
       vcamera = vcam;
   }

   void Start()
   {
       cashedFov = vcam.m_Lens.FieldOfView;
        cashedXScale = ghostLayer.transform.localScale.x;
        cashedZScale = ghostLayer.transform.localScale.z;

       // _playerMovement = ReferenceLibary.PlayerMov;
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
        
            lerpedValueGhostLayerX = MathLibary.RemapClamped( StartZoomingValue, StopZoomingValue, cashedXScale, cashedXScale+ addXScaleGhostlayer, xzVelocity);
            lerpedValueGhostLayerZ = MathLibary.RemapClamped( StartZoomingValue, StopZoomingValue, cashedZScale, cashedZScale+ addZScaleGhostlayer, xzVelocity);
       // Debug.Log(lerpedValue);
       
       
       if (cashedFov + ZoomOutDelay < lerpedValue)
       {
           vcam.m_Lens.FieldOfView = Mathf.Lerp(vcam.m_Lens.FieldOfView, lerpedValue, zoomOutRoughness*Time.deltaTime);
           ghostLayer.transform.localScale = new Vector3(
                   Mathf.Lerp(ghostLayer.transform.localScale.x, lerpedValueGhostLayerX, moonZoomOutRoughness*Time.deltaTime),
                   ghostLayer.transform.localScale.y ,
                   Mathf.Lerp(ghostLayer.transform.localScale.z, lerpedValueGhostLayerZ, moonZoomOutRoughness*Time.deltaTime)
               )
               ;
           // Debug.Log(zoomOutRoughness * Time.deltaTime);
       }
      
     }
    }
    

    
}
