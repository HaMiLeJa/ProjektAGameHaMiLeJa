using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseLei : MonoBehaviour
{
    public int invokeTimeSelfDestruct;

    // Start is called before the first frame update
    void Start()
    {

        Invoke(nameof(SelfDestruction), invokeTimeSelfDestruct);
    }

  
    void SelfDestruction()
    {
        Destroy(gameObject);
    }

}
