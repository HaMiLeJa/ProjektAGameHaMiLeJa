using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemTrails : MonoBehaviour
{
    [SerializeField] private ParticleSystem superboostWirbel;
    [SerializeField] private GameObject Orb;
    [SerializeField] private Renderer MeshoutSide;
    [SerializeField] private Renderer MeshInside;
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private TrailRenderer _trailRenderer2;
    [SerializeField] private ParticleSystem GlowingOrbs;

    private void Awake()
    {
        _trailRenderer.emitting = false;
        _trailRenderer2.emitting = false;
        Orb.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(PlayNonTrailParticle_Coroutine());
            StartCoroutine(SuperBoost_Coroutine());

        }
        
    }
    
    public void StartSuperDashParticle()
    {

        StartCoroutine(PlayNonTrailParticle_Coroutine());
        StartCoroutine(SuperBoost_Coroutine());
    }
    

    IEnumerator PlayNonTrailParticle_Coroutine()
    {
        var particleDirection = Quaternion.LookRotation(ReferenceLibary.RigidbodyPl.velocity.normalized, Vector3.up);
            
        var go = Instantiate(superboostWirbel, ReferenceLibary.Player.transform.position, particleDirection);
        go.Play();
        
            
        var go2 = Instantiate(GlowingOrbs, ReferenceLibary.Player.transform.position, particleDirection);
        go2.Play();
        go2.transform.parent = transform;
       // yield return new WaitForSeconds(0.1f);
      //  go.transform.parent = transform;
        yield return new WaitForSeconds(10);
        Destroy(go);  //zur sicherheit, wird im partikelsystem zerstört
    }

    IEnumerator SuperBoost_Coroutine()
    {
        
        Orb.SetActive(true);
        MeshoutSide.enabled = false;
        MeshInside.enabled = false;
        _trailRenderer.emitting = true;
        _trailRenderer2.emitting = true;

        yield return new WaitForSeconds(1.4f);
        _trailRenderer.emitting = false;
        _trailRenderer2.emitting = false;
        MeshoutSide.enabled = true;
        MeshInside.enabled = true;
        Orb.SetActive(false);
    }
}
