using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlamParentScript : MonoBehaviour
{
   // [SerializeField] GameObject PlayerParticleParent;
    public float livetime;
    float timer;
    [SerializeField] ParticleSystem slamParticle;

    void Update()
    {
        timer += Time.deltaTime;

        if(timer > livetime)
        {
            Destroy(this.gameObject);
        }
    }


    public void PlayParticle()
    {
        slamParticle.Play();
    }

    public void Detach()
    {
        this.transform.parent = null;
        slamParticle.Play();
    }
}
