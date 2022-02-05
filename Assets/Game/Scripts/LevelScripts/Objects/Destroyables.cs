using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

public class Destroyables : MonoBehaviour
{
    public DestroyableScriptableObject settings;
    [Space]
    PlayerSuperDash superDash;
    GameObject player;
    Collider col;

    private Rigidbody Rigidbody;
    public AudioSource myAudioSource;
    private GameObject brokenInstance;
    bool TriggerResetted = false;

    Renderer myRenderer;

    List<Material> AllMaterials = new List<Material>();

    int collisionCounter = 0;


    void Start()
    {
        if(myAudioSource == null)
             myAudioSource = this.GetComponent<AudioSource>();

        col = this.GetComponent<Collider>();

        superDash = ReferenceLibary.SuperDash;
        player = ReferenceLibary.Player;

        if (settings.ChangeMaterial == true)
        {
            myRenderer = this.GetComponent<Renderer>();

            AllMaterials.Add(settings.Material01);
            AllMaterials.Add(settings.Material02);
            AllMaterials.Add(settings.Material03);
            AllMaterials.Add(settings.Material04);
        }

        collisionCounter = 0;
    }
    
    private void FixedUpdate()
    {
      
    }

    public void Explode()
    {
        Destroy(Rigidbody);
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;

       if (settings.DestructionClip != null)
       {
            if (myAudioSource.isPlaying == false)
            {
                myAudioSource.clip = settings.DestructionClip;
                myAudioSource.outputAudioMixerGroup = settings.DestroyGroup;
                myAudioSource.pitch = UnityEngine.Random.Range(0.8f, 1.6f);
                myAudioSource.Play();
            }
        }

        GameObject brokenPrefabCopy = settings.BrokenPrefab;
        brokenInstance = Instantiate(brokenPrefabCopy, transform.position, transform.rotation);

        Rigidbody[] rigidbodies = brokenInstance.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody body in rigidbodies)
        {
            if (Rigidbody != null)
            {
                // inherit velocities
                body.velocity = Rigidbody.velocity;
            }
            body.AddExplosionForce(settings.ExplosiveForce, transform.position, settings.ExplosiveRadius);
        }

        StartCoroutine(FadeOutRigidBodies(rigidbodies));
      
    }
    
    private IEnumerator FadeOutRigidBodies(Rigidbody[] Rigidbodies)
    {
        WaitForSeconds Wait = new WaitForSeconds(settings.SleepCheckDelay);
        float activeRigidbodies = Rigidbodies.Length;

        while (activeRigidbodies > 0)
        {
            yield return Wait;

            foreach (Rigidbody rigidbody in Rigidbodies)
            {
                if (rigidbody.IsSleeping())
                {
                    activeRigidbodies--;
                }
            }
       
        }


        yield return new WaitForSeconds(settings.DestroyDelay);

        float time = 0;
        Renderer[] renderers = Array.ConvertAll(Rigidbodies, GetRendererFromRigidbody);
        
        foreach(Rigidbody body in Rigidbodies)
        {
            Destroy(body.GetComponent<Collider>());
            Destroy(body);
        }

        while(time < 1)
        {
            float step = Time.deltaTime * settings.FadeSpeed; 
            foreach (Renderer renderer in renderers)
            {
                renderer.transform.Translate(Vector3.down * (step / renderer.bounds.size.y), Space.World);
            }

            time += step;
            yield return null;
        }

        foreach (Renderer renderer in renderers)
        {
            Destroy(renderer.gameObject);
        }

        if (settings.Respawn)
        {
            StopCoroutine(Coroutine_ResetObject());
            StartCoroutine(Coroutine_ResetObject());
        }
   
     if(!settings.Respawn)  Destroy(gameObject);
    }

    private Renderer GetRendererFromRigidbody(Rigidbody Rigidbody)
    {
        return Rigidbody.GetComponent<Renderer>();
    }

    IEnumerator Coroutine_ResetObject()
    {
        yield return new WaitForSeconds(settings.resetTimer);

        //Change Material
        if(settings.ChangeMaterial == true)
            myRenderer.material = AllMaterials[UnityEngine.Random.Range(0, 4)];

        GetComponent<Collider>().enabled = true;
        GetComponent<Renderer>().enabled = true;
    }

    int DestroyCounter;
    int hitCounter;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != player) return;

        if (settings.AllowAutomatedDestruction == true)
            collisionCounter++;

        if (superDash.isDestroying == true || ReferenceLibary.DownDashPl.isDestroying == true || collisionCounter >= settings.HitAmount)
        {

            

            col.enabled = false;
            
            ReferenceLibary.RigidbodyPl.velocity *= -1;
            Explode();


            if (DestroyCounter >= 15)
            {
                float points15 = settings.DestroyValue / 15;
                if (points15 < 1) points15 = 1;
                ScoreManager.OnScoring?.Invoke(points15);
                
            }
            else
            {
                float scoreValue = ((DestroyCounter * 0.05f)) * settings.DestroyValue;
                float points = settings.DestroyValue - DestroyCounter;
                if (points <= 1) points = 1;
                ScoreManager.OnScoring?.Invoke(points);
            }

            DestroyCounter++;
            collisionCounter = 0;

        }
        else
        {

            if (hitCounter >= 15)
            {
                float points = settings.CollisionValue / 15;
                if (points < 1) points = 1;
                ScoreManager.OnScoring?.Invoke(points);
                
                
            }
            else
            {
                float scoreValue = ((hitCounter * 0.05f)) * settings.CollisionValue;
                float points = settings.CollisionValue - scoreValue;
                if (points < 1) points = 1;

                ScoreManager.OnScoring?.Invoke(points);
            }


            hitCounter++;

            if (myAudioSource.isPlaying == false)
            {
                myAudioSource.clip = settings.CollisionClip;
                myAudioSource.outputAudioMixerGroup = settings.CollisionGroup;
                myAudioSource.pitch = UnityEngine.Random.Range(0.8f, 1.6f);
                myAudioSource.Play();
            }
        }

    }

}
