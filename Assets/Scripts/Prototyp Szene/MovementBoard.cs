using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBoard : MonoBehaviour
{
    [SerializeField] float speed;

    // Update is called once per frame
    void Update()
    {
        //InputKeyboard();

        InputController();
        

    }

    void InputKeyboard()
    {
        if (Input.GetKey(KeyCode.W)) //X
        {
            this.transform.Rotate(new Vector3(1 * speed * Time.deltaTime, 0, 0), Space.World);
        }
        if (Input.GetKey(KeyCode.S)) //Y
        {
            this.transform.Rotate(new Vector3(-1 * speed * Time.deltaTime, 0, 0), Space.World);
        }

        if (Input.GetKey(KeyCode.A)) //Y
        {
            this.transform.Rotate(new Vector3(0, 0, 1 * speed * Time.deltaTime), Space.World);
        }
        if (Input.GetKey(KeyCode.D)) //Y
        {
            this.transform.Rotate(new Vector3(0, 0, -1 * speed * Time.deltaTime), Space.World);
        }
    }


    void InputController()
    {
         float horizontal = Input.GetAxis("Horizontal"); //-> dreht auf dem "Boden"
        float vertical = Input.GetAxis("Vertical");

        this.transform.Rotate(new Vector3(vertical * speed * Time.deltaTime, 0, -horizontal * speed * Time.deltaTime), Space.World);
    }
}
