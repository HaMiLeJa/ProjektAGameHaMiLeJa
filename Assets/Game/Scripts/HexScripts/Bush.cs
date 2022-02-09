using System.Collections;
using UnityEngine;
public class Bush : MonoBehaviour

{
    [SerializeField] private int headshakes = 4;
    private bool rotationAllowed = true;
    private int maxHeadshakes;
    [SerializeField]
    private float rotationAngle = 80;
    private float angles;
    [SerializeField] private float rotDuration = 0.3f;
    [Space]
    [SerializeField] float force = 5;
    private void Awake()
    {
        maxHeadshakes = headshakes;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ReferenceLibary.Player)
        {

            Vector3 posOther = other.transform.position;
            angles = Vector3.Angle(posOther, this.transform.position)*100;
            //Debug.Log(angles);
            this.gameObject.transform.Rotate(0,angles,0,Space.Self);
            float playerxzVelocity = Mathf.Abs(ReferenceLibary.RigidbodyPl.velocity.x) +
                                     Mathf.Abs(ReferenceLibary.RigidbodyPl.velocity.z);
            if (rotationAllowed)
            {
                StartCoroutine(Rotate(this.gameObject,headshakes,rotDuration, rotationAngle , Vector3.down));
            }
        }
    }
    
    public IEnumerator Rotate(GameObject rotateMe,int headshakes , float duration, float angle, Vector3 firstDirection)
    {
        Quaternion startRot = rotateMe.transform.rotation;
        rotationAllowed = false;
        if (headshakes == 0)
        {
            rotationAllowed = true;
            yield break;
        }

        if (headshakes == maxHeadshakes)
            startRot = Quaternion.Euler(rotateMe.transform.rotation.x,angles,
                rotateMe.transform.rotation.z);
    
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            rotateMe.transform.rotation = startRot * Quaternion.AngleAxis(t / duration * angle, firstDirection);
            yield return null;
        }
        headshakes--;
        yield return Rotate(rotateMe,headshakes,UnityEngine.Random.Range(1.2f,1.4f)*duration,
            UnityEngine.Random.Range(angle, angle + 5), -firstDirection);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == ReferenceLibary.Player)
        {

            Rigidbody rb = ReferenceLibary.RigidbodyPl;

            Vector3 movementDirection = rb.velocity.normalized;

            float timer = 0;
            while (timer <= 0.3f)
            {
                Debug.Log("Schubs");
                rb.AddForce(movementDirection * force * Time.deltaTime, ForceMode.Force);
                timer+= Time.deltaTime;
            }

            rb.AddForce(movementDirection * force * 100 *Time.deltaTime, ForceMode.Force);

        }
    }
}
