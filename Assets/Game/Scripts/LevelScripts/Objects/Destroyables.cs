using System.Collections;
using UnityEngine;
using System;

public class Destroyables : MonoBehaviour
{
    public DestroyableScriptableObject settings;
    [Space]
    PlayerSuperDash superDash;
    [SerializeField] GameObject player;
    Collider col;

    private Rigidbody Rigidbody;
    public AudioSource AudioSource;
    private GameObject brokenInstance;
    bool TriggerResetted = false;

    void Start()
    {
        if(AudioSource == null)
             AudioSource = this.GetComponent<AudioSource>();
        col = this.GetComponent<Collider>();

        superDash = ReferenceLibary.SuperDash;
        player = ReferenceLibary.Player;
    }
    
    private void FixedUpdate()
    {
        /*
        if (superDash.isDestroying == true)
        {
            col.isTrigger = true;
            TriggerResetted = true;
        }
        else if (TriggerResetted == false)
        {
            TriggerResetted = true;
            col.isTrigger = false;
        }
        */
    }
    public void Explode()
    {
        Destroy(Rigidbody);
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;

       if (settings.DestructionClip != null)
       {
            if(AudioSource.isPlaying == false)
                 AudioSource.PlayOneShot(settings.DestructionClip);
       }

        GameObject brokenPrefabCopy = settings.BrokenPrefab;
        brokenInstance = Instantiate( brokenPrefabCopy, transform.position, transform.rotation);

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
        GetComponent<Collider>().enabled = true;
        GetComponent<Renderer>().enabled = true;
    }


    int hitCounter;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != player) return;
        
        if (superDash.isDestroying == true || ReferenceLibary.DownDashPl.isDestroying == true)
        {
            col.enabled = false;
            
            ReferenceLibary.RigidbodyPl.velocity *= -1;
            Explode();

          
            ScoreManager.OnScoring?.Invoke(settings.DestroyValue);

        }
        else
        {

            if (hitCounter >= 20)
            {
                ScoreManager.OnScoring?.Invoke(settings.CollisionValue / 20);
                Debug.Log("20 approached");
                return;
            }

          

            if (AudioSource.isPlaying == false)
            {
                AudioSource.clip = settings.CollisionClip;
                AudioSource.Play();
            }

            float scoreValue = ((hitCounter * 0.05f)) * settings.CollisionValue;
            hitCounter++;
            ScoreManager.OnScoring?.Invoke(settings.CollisionValue - scoreValue);
            
        }

    }
}
