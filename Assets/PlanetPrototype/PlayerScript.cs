using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public GameObject Planet;

    public float speed = 150f;
    //[SerializeField] float lerpSpeed = 0.2f;

    public float JumpHeight = 1.2f;

    [SerializeField] float forceJump;
    [SerializeField] float jumpDuration;
    float timer;

    float gravity = 100;
    [SerializeField] bool OnGround = false;

    float distanceToGround;
    Vector3 Groundnormal;

    Rigidbody rb;

    public GameObject explosionPrefab;


    //Leis Code
    public GameObject explosionTwo;
    int lives = 3;
    public Text livesDisplay;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

    }

    void Update()
    {

        
        Movement();
        GroundCheck();
        //Jump();
        Gravity();
       
         
    }


    void Movement()
    {
        

        Vector3 strafeMovement = transform.right * Input.GetAxis("Horizontal");
        Vector3 forwardMovement = transform.forward * Input.GetAxis("Vertical");

        Vector3 movement = forwardMovement + strafeMovement;
        movement = movement.normalized * Time.deltaTime * speed;

        // Bewegung
        rb.MovePosition(transform.position + movement);

        //rb.MovePosition(Vector3.Lerp(transform.position, transform.position + movement, lerpSpeed));

        //rb.MovePosition(transform.position + movement);



        /*float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        
        transform.Translate(x, 0, z); */


        
    }

    void GroundCheck()
    {
        //GroundControl
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -transform.up, out hit, 10))
        {
            distanceToGround = hit.distance;
            Groundnormal = hit.normal; //verwendet bei Gravity

            if (distanceToGround <= 0.2f)
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
        if (Input.GetButton("JumpJanina") && OnGround == true && timer < jumpDuration)
        {
            timer += Time.deltaTime;
            rb.AddForce(Vector2.up * forceJump);
        }
        else
        {
            timer = 0;
        }


        
    }

    void Gravity()
    {
        //Gravity and rotation
        Vector3 gravDirection = (transform.position - Planet.transform.position).normalized;

        if (OnGround == false)
        {
            rb.AddForce(gravDirection * -gravity);

        }

        // Quat
        Quaternion toRotation = Quaternion.FromToRotation(transform.up, Groundnormal) * transform.rotation;
        transform.rotation = toRotation;
    }
    
    /// Code von Lei
    /// </summary>
    /// <param name="coll"></param>
    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("Enemy"))
        {
            Vector3 targetPos = coll.gameObject.transform.position;
            Destroy(coll.gameObject);
            Instantiate(explosionPrefab, targetPos, Quaternion.identity);

           

        }
        else if(coll.gameObject.CompareTag("Defense"))
        {
            Destroy(coll.gameObject);
            Instantiate(explosionTwo, transform.position, Quaternion.identity);

            gameObject.SetActive(false);
            lives--;
            livesDisplay.text = "Lives: " + lives;

            if (lives > 0)
                Invoke(nameof(NextLife), 3);

        }


    }

    void NextLife()
    {
        gameObject.SetActive(true);
        transform.position += new Vector3(0, 50,0);
    }
}
