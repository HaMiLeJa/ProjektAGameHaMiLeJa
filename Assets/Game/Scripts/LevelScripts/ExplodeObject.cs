using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeObject : MonoBehaviour
{

    public void Explode()
    {
        Destroy(Rigidbody);
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;

        if (settings.DestructionClip != null)
        {
            if (AudioSource.isPlaying == false)
            {
                AudioSource.clip = settings.DestructionClip;
                AudioSource.pitch = UnityEngine.Random.Range(0.8f, 1.6f);
                AudioSource.Play();
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

        foreach (Rigidbody body in Rigidbodies)
        {
            Destroy(body.GetComponent<Collider>());
            Destroy(body);
        }

        while (time < 1)
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

        if (!settings.Respawn) Destroy(gameObject);
    }

    private Renderer GetRendererFromRigidbody(Rigidbody Rigidbody)
    {
        return Rigidbody.GetComponent<Renderer>();
    }

    IEnumerator Coroutine_ResetObject()
    {
        yield return new WaitForSeconds(settings.resetTimer);

        //Change Material
        if (settings.ChangeMaterial == true)
            myRenderer.material = AllMaterials[UnityEngine.Random.Range(0, 4)];

        GetComponent<Collider>().enabled = true;
        GetComponent<Renderer>().enabled = true;
    }


}
