using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cinemachine.Utility;
using JetBrains.Annotations;

public class Portal : MonoBehaviour //Portal in zwei Richtungen, Frei untereinander verlinkbar
{
    [Tooltip ("Link the Goal Portal here")]
    [SerializeField] GameObject Goal;
    GameObject player;
    private CinemachineVirtualCamera cam = default;
    private bool DelayActive = true;
    [HideInInspector] public bool StartPortal = false;
    [HideInInspector] public bool GoalPortal = false;
    [Range(0.0001f,0.01f)]
    public float followRoughness = 0.005f;
    [Header("Increase Cam speed at")]
    [Range(0f,200f)]
    public float lastDistanceTreshhold = 60f;
    [Range(0f,10f)]
    public float lastDistanceSpeedIncreasePercentPerFrame = 1f;
    private float cashedlerpValue;
    private float snapBackTreshold = 0.0001f;
    private PlayerMovement _playerMovement;
    private float cashedXVelocity, cashedZVelocity, xzVelocity;
    AudioSource myAudioSource;
    

    void Start()
    {
        cam = CameraZoomOut.vcamera;
        _playerMovement = FindObjectOfType<PlayerMovement>();
        cashedlerpValue = followRoughness;
        myAudioSource = this.GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        
        if (!GameManager.CameraTeleportActive)
        {
            followRoughness = cashedlerpValue;
       
        }
        
       if (GameManager.CameraTeleportActive)
       {
<<<<<<< Updated upstream
           if(GameManager.ZeroOutAllowed)
                _playerMovement.rb.velocity = Vector3.zero;
=======
           
>>>>>>> Stashed changes
           
           float  distanceCamHelperPlayer =
               (Mathf.Abs(GameManager.CameraHelper.transform.position.x) - Mathf.Abs(player.transform.position.x))
               + (Mathf.Abs(GameManager.CameraHelper.transform.position.z) - Mathf.Abs(player.transform.position.z));

           if(distanceCamHelperPlayer > 10)
               _playerMovement.rb.velocity = Vector3.zero;
           if(distanceCamHelperPlayer < 10)
               _playerMovement.rb.velocity = new Vector3(cashedXVelocity, 0, cashedZVelocity);
           
           if (distanceCamHelperPlayer < 20 && GameManager.GiveVelocityBack)
           {
              
               GameManager.GiveVelocityBack = !GameManager.GiveVelocityBack;
           }
               
           
           if (distanceCamHelperPlayer < snapBackTreshold)
           {
               cam.LookAt = player.transform;
               cam.Follow =  player.transform;
               followRoughness = cashedlerpValue;
             
               GameManager.CameraHelper.transform.position = new Vector3(
                       GameManager.CameraHelper.transform.position.x,
                   GameManager.CameraHelper.transform.position.y+200, 
                       GameManager.CameraHelper.transform.position.z);
               
               GameManager.CameraTeleportActive = false;
               GameManager.ZeroOutAllowed = false;
           }
           
           if (distanceCamHelperPlayer < lastDistanceTreshhold && distanceCamHelperPlayer >= 1f)
           {
               followRoughness = followRoughness * (lastDistanceSpeedIncreasePercentPerFrame/100+1);
           }
           if (distanceCamHelperPlayer < 1f && distanceCamHelperPlayer >= snapBackTreshold)
           {
               followRoughness = followRoughness * (lastDistanceSpeedIncreasePercentPerFrame/20+1);
           }
        
           GameManager.CameraHelper.transform.position = Vector3.Lerp( 
               GameManager.CameraHelper.transform.position,
               player.transform.position, 
               followRoughness);
       }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == player.tag)
        {
            
           
            if (GoalPortal == false) //Wenn diese Portal das startPoral ist fortfahren
            {
                cashedXVelocity = _playerMovement.rb.velocity.x;
                cashedZVelocity = _playerMovement.rb.velocity.z;
                
                StartPortal = true;
                GoalPortal = false;
                
                Goal.GetComponent<Portal>().GoalPortal = true;

                if (!DelayActive)
                {
                    int numVcams = CinemachineCore.Instance.VirtualCameraCount;
                    for (int i = 0; i < numVcams; ++i)
                        CinemachineCore.Instance.GetVirtualCamera(i).OnTargetObjectWarped(
                            player.transform, -Goal.transform.position);
                      player.transform.position = Goal.transform.position;
                }
                if (DelayActive)
                {
                    if(!GameManager.CameraTeleportActive)
                    {

                        GameManager.ZeroOutAllowed = true;
                        GameManager.GiveVelocityBack = !GameManager.GiveVelocityBack;
                        GameManager.CameraHelper.transform.position = player.transform.position;
                        cam.LookAt =  GameManager.CameraHelper.transform; 
                        cam.Follow =  GameManager.CameraHelper.transform;
                        player.transform.position = Goal.transform.position;
                        GameManager.CameraTeleportActive = true;
                    }
                }
            }
            
            if(GoalPortal)
            {
                StartPortal = false;

                if (myAudioSource.isPlaying == false && AudioManager.Instance.allowAudio == true)
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
