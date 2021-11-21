
/*
 Script geht noch nicht und braucht mehr arbeit
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Camera))]
public class FloatingPointOrigin : MonoBehaviour
{
 
    [SerializeField] private float threshold;
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Start;
    void LateUpdate()
    {
        Vector3 cameraPosition = gameObject.transform.position;
        cameraPosition.y = 0f;

        if (Player.transform.position.z + threshold < Start.transform.position.z)

            for (int z = 0; z < SceneManager.sceneCount; z++)
            {
                foreach (GameObject g in SceneManager.GetSceneAt(z).GetRootGameObjects())
                {
                    g.transform.position -= cameraPosition;
                }
            }

            Vector3 originDelta = Vector3.zero - cameraPosition;
            Debug.Log("recentering, origin delta = " + originDelta);
        }

    }
*/
