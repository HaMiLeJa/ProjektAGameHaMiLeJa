using UnityEngine;
public class DissolveHeight : MonoBehaviour
{
    Renderer rend;
    void Start() => rend = GetComponent<Renderer>();
    void Update() => rend.material.SetFloat("_DissolveStartHeight", transform.position.y);
}