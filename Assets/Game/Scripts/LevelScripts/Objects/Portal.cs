using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cinemachine.Utility;
using JetBrains.Annotations;
using NaughtyAttributes;


public class Portal : MonoBehaviour //Portal in zwei Richtungen, Frei untereinander verlinkbar
{
    [Tooltip ("Link the Goal Portal here")]
    [SerializeField] GameObject Goal;
    GameObject player;
    private CinemachineVirtualCamera cam = default;
    private bool DelayActive = true;
    [HideInInspector] public bool StartPortal = false;
    [HideInInspector] public bool GoalPortal = false;
    
   
    private float cashedlerpValue;
    private PlayerMovement _playerMovement;
    private float cashedXVelocity, cashedZVelocity, xzVelocity;
    [SerializeField] AudioSource myAudioSource;
    private Vector3 cashedCamHelperPos;
    private float cashedVelocity;
    private GameManager _gameManager;
    private float cashedFovTemp;
    private float cashedFovNewCam;
    private float distanceBetweenCamHelperAndPlayerCashed;
    [Range(0,70)]
    [SerializeField] float zoomOutDuringTeleport = 18;
    void Start()
    {
        cam = CameraZoomOut.vcamera;
        _playerMovement = ReferenceLibary.PlayerMov;
        _gameManager = ReferenceLibary.GameMng;
        cashedlerpValue = _gameManager.followRoughness;
        if (cam != CameraZoomOut.vcamera)
            cashedFovNewCam = cam.m_Lens.FieldOfView;

        //myAudioSource = this.GetComponent<AudioSource>();

        player = ReferenceLibary.Player;
        cashedCamHelperPos = GameManager.CameraHelper.transform.position;
    }

    private void SetBackFollowSpeed()
    {
        if (!GameManager.CameraTeleportActive)
            _gameManager.followRoughness = cashedlerpValue;
    }

    
    private void FixedUpdate()
    {


        if (GameManager.CameraTeleportActive)
        {
         
           
           
           //Calculate Helper Distance to player
          
           float distanceCamHelperPlayer =  MathLibary.CalculateDistance(ReferenceLibary.Player, GameManager.CameraHelper);
           //modifiy fov
           if( distanceCamHelperPlayer > distanceBetweenCamHelperAndPlayerCashed/2)
               cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, cashedFovTemp+zoomOutDuringTeleport, 1*Time.deltaTime);
           else if (distanceCamHelperPlayer< distanceBetweenCamHelperAndPlayerCashed/2)
               cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, cashedFovTemp, 1*Time.deltaTime);
            //Zero out speed 
           if(distanceCamHelperPlayer > Mathf.Abs(8))
               _playerMovement.rb.velocity = Vector3.zero;

           if (distanceCamHelperPlayer < Mathf.Abs(8) && GameManager.StopGiveVelocityBack)
           {
               GameManager.StopGiveVelocityBack = !GameManager.StopGiveVelocityBack;
           }

           if (distanceCamHelperPlayer < Mathf.Abs(8))
           {
               float horizontalInput = Input.GetAxis("Horizontal");
               float verticalInput = Input.GetAxis("Vertical");
               if (horizontalInput == 0)
                   horizontalInput = UnityEngine.Random.Range(-1f, 1f);
               if (verticalInput == 0)
                   verticalInput = UnityEngine.Random.Range(-1f, 1f);
               
               _playerMovement.rb.velocity = new Vector3(
                   horizontalInput*(_gameManager.SpeedAfterTeleport + (cashedVelocity/_gameManager.ReduceSpeedInfluenceBeforeTeleport*_gameManager.IncreaseSpeedInfluenceBeforeTeleport)),
                   0,
                   verticalInput*(_gameManager.SpeedAfterTeleport+ (cashedVelocity/_gameManager.ReduceSpeedInfluenceBeforeTeleport*_gameManager.IncreaseSpeedInfluenceBeforeTeleport))
                   );
               
               cam.LookAt = player.transform;
               cam.Follow =  player.transform;
               _gameManager.followRoughness = cashedlerpValue;
               distanceCamHelperPlayer = 0;
               GameManager.CameraHelper.transform.position = cashedCamHelperPos;
               cam.m_Lens.FieldOfView = cashedFovNewCam;
               GameManager.CameraTeleportActive = false;
               SetBackFollowSpeed();
           }
         
            if (distanceCamHelperPlayer < _gameManager.lastDistanceTreshhold && distanceCamHelperPlayer >= 5f)
            {
                _gameManager.followRoughness = _gameManager.followRoughness * (_gameManager.lastDistanceSpeedIncreasePercentPerFrame/100+1);
           }
            if (distanceCamHelperPlayer < 10f)
           {
               _gameManager.followRoughness = _gameManager.followRoughness * (_gameManager.lastDistanceSpeedIncreasePercentPerFrame/20+1);
            }
        
              GameManager.CameraHelper.transform.position = Vector3.Lerp( 
               GameManager.CameraHelper.transform.position,
               player.transform.position, 
               _gameManager.followRoughness);
         
       }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == player.tag)
        {
            
           
            if (GoalPortal == false) //Wenn diese Portal das startPoral ist fortfahren
            {
                cashedVelocity = Mathf.Abs(_playerMovement.rb.velocity.x + _playerMovement.rb.velocity.z);
                
                //not used for now
                cashedXVelocity = _playerMovement.rb.velocity.x;
                cashedZVelocity = _playerMovement.rb.velocity.z;
                
                StartPortal = true;
                GoalPortal = false;
                
                Goal.GetComponent<Portal>().GoalPortal = true;
/*
                if (!DelayActive)
                {
                    int numVcams = CinemachineCore.Instance.VirtualCameraCount;
                    for (int i = 0; i < numVcams; ++i)
                        CinemachineCore.Instance.GetVirtualCamera(i).OnTargetObjectWarped(
                            player.transform, -Goal.transform.position);
                      player.transform.position = Goal.transform.position;
                }
                */
                if (DelayActive && !GameManager.CameraTeleportActive)
                {
                    cashedFovTemp = cam.m_Lens.FieldOfView;
                  
                    GameManager.StopGiveVelocityBack = false;
                        GameManager.CameraHelper.transform.position = player.transform.position;
                        cam.LookAt =  GameManager.CameraHelper.transform; 
                        cam.Follow =  GameManager.CameraHelper.transform;
                        player.transform.position = Goal.transform.position;
                        distanceBetweenCamHelperAndPlayerCashed = MathLibary.CalculateDistance(GameManager.CameraHelper, player);
                        GameManager.CameraTeleportActive = true;
                    
                }
            }
            
            if(GoalPortal)
            {
                StartPortal = false;

                if (myAudioSource.isPlaying == false)
                    myAudioSource.Play();
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == player.tag && GoalPortal == true)
        {
            StartPortal = false;
            GoalPortal = false;
        }
    }

}
