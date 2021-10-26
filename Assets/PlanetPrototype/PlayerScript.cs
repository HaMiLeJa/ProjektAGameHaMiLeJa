using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public GameObject Planet;

    public float movementSpeed = 150f;
    [SerializeField] float lerpSpeed = 0.2f;

    //public float JumpHeight = 1.2f;

    [SerializeField] float forceJump;
    [SerializeField] float jumpDuration;
    [SerializeField] bool jumpButtonReleased = true;
    [SerializeField] bool jumping = false;
    float timer;

    bool jumpBottonPressedInLastFrame = false;
    bool allowJump = false;

    [SerializeField] float gravity = 9.8f;
    [SerializeField] bool OnGround = false;

    float distanceToGround;
    Vector3 Groundnormal;

    Rigidbody rb;

    Vector3 movement;

    //public GameObject collisionPrefabOne;


    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.freezeRotation = true;

    }

    void FixedUpdate()
    {

        
        Movement();
        GroundCheck();
        Jump();
        Gravity();
       
         
    }

    void Movement()
    {
        //Variante 1 

        Vector3 strafeMovement = transform.right * Input.GetAxis("Horizontal");
        Vector3 forwardMovement = transform.forward * Input.GetAxis("Vertical");

        movement = forwardMovement + strafeMovement;
        movement = movement.normalized * Time.deltaTime * movementSpeed;

        rb.velocity = rb.velocity + movement;

        //Abhname der Velocity über zeit:



        // Bewegung
        //rb.MovePosition(transform.position + movement); //Funktioniert ganz gut

        //rb.position = Vector3.Lerp(this.transform.position, this.transform.position + movement, lerpSpeed); //funktioniert auch ganz gut

        // rb.MovePosition(Vector3.Lerp(transform.position, transform.position + movement, lerpSpeed)); //funktiioniert auch



       // transform.Translate(movement);


        //Variante 2: Spieler bewegt sich irgendwie nur auf einer Halbkugel, buggt sich oft in die Welt rein
        /*

        float x = Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed;

        Vector3 direction = new Vector3(x, 0, z);

        //rb.MovePosition(rb.transform.position + direction);

        //rb.position = Vector3.Lerp(this.transform.position, this.transform.position + direction, lerpSpeed); // movementSpeed auf ca 150

        //this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + direction, 0.1f); //keine Collision;

        transform.Translate(direction); //keine Collision -> movement speed auf 10 setzen

        */


    }

    void MovementOld()
    {
        
        

        /*
        Vector3 strafeMovement = transform.right * Input.GetAxis("Horizontal");
        Vector3 forwardMovement = transform.forward * Input.GetAxis("Vertical");

        Vector3 movement = forwardMovement + strafeMovement;
        movement = movement.normalized * Time.deltaTime * movementSpeed;

        // Bewegung
        rb.MovePosition(transform.position + movement);

        //rb.MovePosition(Vector3.Lerp(transform.position, transform.position + movement, lerpSpeed));

        //rb.MovePosition(transform.position + movement);

        */


        /*
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed;

        transform.Translate(x, 0, z);
        */

    }

    void GroundCheck()
    {
        //GroundControl
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -transform.up, out hit, 10, LayerMask.GetMask("World")))
        {
            distanceToGround = hit.distance;
            Groundnormal = hit.normal; //verwendet bei Gravity

            if (distanceToGround <= 0.5f)
            {
                OnGround = true;
            }
            else
            {
                OnGround = false;
            }
        }
    }

    void Jump()
    {
        //Jump
        /*
         if (Input.GetKeyDown(KeyCode.Space))
         {
            rb.AddForce(transform.up* 40000 * JumpHeight * Time.deltaTime);
         }
        */

        /*if (Input.GetButton("JumpJanina") && OnGround == true && timer < jumpDuration && jumpButtonReleased == true) 
        {
            jumping = true;

            timer += Time.deltaTime;

            rb.AddForce(this.transform.up * forceJump, ForceMode.Impulse);
        }
        else
        {
            jumping = false;

            jumpButtonReleased = false;

        }

        if (Input.GetButton("JumpJanina") == false) //Check if Button was released, before allowing new jump
        {
            jumpButtonReleased = true;
            timer = 0;
        }*/


        // Wenn Button Gedrückt ist, vorher nicht gedrückt war und onGround -> Sprung starten
        // solange der Timer nicht abgelaufen ist force adden, auch wenn on Ground=false
        //wenn timer abgelaufen:


        if(Input.GetButton("JumpJanina"))
        {
            

            if(OnGround == true && jumpBottonPressedInLastFrame == false)
            {
                allowJump = true;
            }

            jumpBottonPressedInLastFrame = true;


            if (allowJump == true && timer < jumpDuration)
            {
                timer += Time.deltaTime;
                rb.AddForce(this.transform.up * forceJump); //15, 0.1
            }

        }
        else
        {
            timer = 0;
            jumpBottonPressedInLastFrame = false;
            allowJump = false ;
        }



    }

    void Gravity()
    {

        if (jumping == true) return; //eig unnötig

        //Gravity and rotation
        //Vector3 gravDirection = (transform.position - Planet.transform.position).normalized; //Original Direction

        //Idea: Add current movement direction to it?.....
        Vector3 gravDirection = (transform.position - Planet.transform.position).normalized;

        if (OnGround == false)
        {
            rb.AddForce((gravDirection * -gravity));

        }

        // Quat
        Quaternion toRotation = Quaternion.FromToRotation(transform.up, Groundnormal) * transform.rotation;
        transform.rotation = toRotation;
    }
    

}
