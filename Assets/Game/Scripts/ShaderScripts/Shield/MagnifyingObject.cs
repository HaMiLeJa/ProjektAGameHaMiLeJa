using UnityEngine;
public class MagnifyingObject : MonoBehaviour
{
    Renderer renderer; Camera cam;
    void Start()
    {
        renderer = GetComponent<Renderer>();
        cam = Camera.main;
    }
    void Update()
    {
        transform.forward = cam.transform.position - transform.position; //lookat
        Vector3 screenPoint = cam.WorldToScreenPoint(transform.position);
        screenPoint.x = screenPoint.x / Screen.width;
        screenPoint.y = screenPoint.y / Screen.height;
        renderer.material.SetVector("_ObjScreenPos", screenPoint);
    }
}
