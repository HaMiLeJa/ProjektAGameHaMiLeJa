using System.Collections;
using UnityEngine;
public class HighlightObjects : MonoBehaviour
{
    [SerializeField] private GameObject Lampe;
    [SerializeField] private int headshakes = 4;
    private Hex hex;
    private bool rotationAllowed = true;
    private void Awake()
    {
        hex = transform.parent.transform.parent.GetComponent<Hex>();
        if(Lampe == null) return;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ReferenceLibary.Player)
        {
            hex.startHighlight(true);
            if(Lampe == null) return;
            float playerxzVelocity = Mathf.Abs(ReferenceLibary.RigidbodyPl.velocity.x) +
                                     Mathf.Abs(ReferenceLibary.RigidbodyPl.velocity.z);
            if (playerxzVelocity > 73 && rotationAllowed)
            {
                int randomDirection = Random.Range(0, 1);
                if (randomDirection == 0) StartCoroutine(Rotate(Lampe,headshakes,0.17f, 27+playerxzVelocity/5, Vector3.up));
                else if (randomDirection == 1) StartCoroutine(Rotate(Lampe,headshakes,0.17f, 27+playerxzVelocity/5, Vector3.down));
            }
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == ReferenceLibary.Player) hex.startHighlight(true);
    }
    public IEnumerator Rotate(GameObject rotateMe,int headshakes , float duration, float angle, Vector3 firstDirection)
    {
        rotationAllowed = false;
        if (headshakes == 0)
        {
            rotationAllowed = true; yield break;
        }
        Quaternion startRot = rotateMe.transform.rotation;
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            rotateMe.transform.rotation = startRot * Quaternion.AngleAxis(t / duration * angle, firstDirection);
            yield return null;
        }
        headshakes--;
        yield return Rotate(rotateMe,headshakes,Random.Range(1.2f,1.4f)*duration,
            Random.Range(angle, angle + 5), -firstDirection);
    }
}