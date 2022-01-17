using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{

    public ScriptableObject scritpableObjectType;
    private GameObject player;
    public GameObject normalMeshPrefab;
    public GameObject breakablePrefab;
    public AudioSource audioOnBreak;
    public float audioDelay = 0.01f;
    public ParticleSystem particleToSpawn;
    public bool resetAllowed = false;
    private MeshRenderer meshRenderer;
    private Collider thisCollider;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        meshRenderer = GetComponent<MeshRenderer>();
        thisCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            activateNormalPrefab();
            destroyBreakablePrefab();
            
                
        }
    }

    private void OnCollisionEnter(Collision col)
    {
     
        
       // if (col.gameObject == player)
        if(true)
        {
            deactivateNormalPrefab();
            instantiateBreakablePrefab();
            
            
            playSoundOnBreak();
            playBreakableVFX();

         
        }
    }

    
    private void destroyBreakablePrefab()
    { 
        Destroy(prefabToDestroyLater); 
    }
    private void deactivateNormalPrefab()
    {
        meshRenderer.enabled = false;
        thisCollider.enabled = false;
    }

    private void activateNormalPrefab()
    {
        meshRenderer.enabled = true;
        thisCollider.enabled = true;
    }

    private void subscribeToBrokenDic()
    {

        // Subscribe to a dictonary that 
        //this.gameObject 
    }

    private void playSoundOnBreak()
    {
        audioOnBreak.PlayDelayed(audioDelay);
    }
    GameObject prefabToDestroyLater = null;
    private void instantiateBreakablePrefab()
    {
        
        prefabToDestroyLater = Instantiate(breakablePrefab,
            normalMeshPrefab.gameObject.transform.position,
            normalMeshPrefab.gameObject.transform.rotation);
    }

    private void playBreakableVFX()
    {
        particleToSpawn.Play();
    }

}

