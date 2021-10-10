using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBoard : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float horizontalRotationSpeed;
    [SerializeField] float maxTilt = 20f;


    // Update is called once per frame
    void Update()
    {
        

        InputTilt();
        InputHorizontalRotation();



    }

    void InputTilt()
    {
        float xValue = Input.GetAxis("Vertical");
        float zValue = Input.GetAxis("Horizontal");

        this.transform.Rotate(new Vector3(xValue * speed * Time.deltaTime, 0, -zValue * speed * Time.deltaTime), Space.World);
    }

    void TiltLimit() // (funktioniert nicht)
    {
        float xRotation = this.transform.rotation.x;
        float zRotation = this.transform.rotation.z;

        if (Mathf.Abs(xRotation) > 0.2f)
        {
            this.transform.Rotate(new Vector3(maxTilt * Mathf.Sign(xRotation), 0, 0), Space.World);
        }
        if (Mathf.Abs(zRotation) > 0.2f)
        {
            this.transform.Rotate(new Vector3(0, 0, maxTilt * Mathf.Sign(zRotation)), Space.World);
        }
    }

    void InputHorizontalRotation()
    {
        float yValue = Input.GetAxis("HorizontalRotation");

        this.transform.Rotate(new Vector3(0, yValue * horizontalRotationSpeed * Time.deltaTime, 0), Space.World);
    }
}
