using UnityEngine;
public class MagnifyingObject : MonoBehaviour
{
    [SerializeField]Renderer renderer; [SerializeField]Camera cam;
    private Vector3 screenPoint;
    private string objScreenPos = "_ObjScreenPos";
    void Update()
    {
        transform.forward = cam.transform.position - transform.position; //lookat
        screenPoint = cam.WorldToScreenPoint(transform.position);
        screenPoint.x = screenPoint.x / Screen.width;
        screenPoint.y = screenPoint.y / Screen.height;
        renderer.material.SetVector(objScreenPos, screenPoint);
    }
}
