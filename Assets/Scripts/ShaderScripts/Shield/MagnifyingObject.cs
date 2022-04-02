using UnityEngine;
public class MagnifyingObject : MonoBehaviour
{
    [SerializeField ]private Renderer rend; 
    [SerializeField] private Camera cam;
    [SerializeField] private Transform trans;
    private Vector3 _screenPoint;
    private string objScreenPos = "_ObjScreenPos";
    void Update()
    {
        trans.forward = cam.transform.position - transform.position; //lookat
        _screenPoint = cam.WorldToScreenPoint(trans.position);
        _screenPoint.x /= Screen.width;
        _screenPoint.y /= Screen.height;
       rend.material.SetVector(objScreenPos, _screenPoint);
    }
}
