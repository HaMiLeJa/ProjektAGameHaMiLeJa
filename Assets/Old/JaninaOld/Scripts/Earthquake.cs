using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earthquake : MonoBehaviour
{
    [SerializeField] float speed;
    float timer;
    public bool earthquakeing = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetButton("LBJanina") && Input.GetButton("RBJanina"))
        {
            earthquakeing = true;

            timer += Time.deltaTime;

            if (timer > 0.1f)
            {
                Vector3 position = new Vector3(Random.Range(0f, 1f), 0, 0);

                this.transform.localPosition = Vector3.Lerp(this.transform.position, position, speed);
                
                timer = 0;
            }
        }
        else
        {
            earthquakeing = false;
        }
    }
}
