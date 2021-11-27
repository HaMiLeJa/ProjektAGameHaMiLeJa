using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCurves : MonoBehaviour
{
    public bool OnCurve = false;
    [SerializeField] float distanceToCurve;
    [SerializeField] float distance = 3f;

    List<GameObject> CurvesUnderPlayer = new List<GameObject>();

    GameObject currentCurve;
    MovementWay movementWay;

    Vector3 curveDirection;
    
    Rigidbody rb;

    [SerializeField] float speed = 30;


    bool curveFound = false;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Curve")
        {
           if(OnCurve == false)
           {

                OnCurve = true;

                currentCurve = collision.gameObject.transform.parent.gameObject;
                movementWay = currentCurve.GetComponentInChildren<MovementWay>();

                //FindStartpoint (and Direction?)

                //Startpoint reingeben
                if (waypointCoroutine == null)
                    waypointCoroutine = StartCoroutine(MoveToWaypoint(startpoint));
            }
                

            
        }
       
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Curve")
        {
            if (lastWaypointReached == true)
            {
                StartCoroutine(WaitForLeaveCurve());
                rb.AddForce(direction * 50, ForceMode.Impulse);
            }
        }

    }

    IEnumerator WaitForLeaveCurve()
    {
        RaycastHit hit = new RaycastHit();
        while(OnCurve == true)
        {
            if (Physics.Raycast(transform.position, -transform.up, out hit, 20, LayerMask.GetMask("Level")))
            {
                if (hit.collider.tag == "Curve")
                {
                    OnCurve = true;
                }
                else
                    OnCurve = false;
                
            }
            yield return null;
        }

        OnCurve = false;
        waypointCoroutine = null;
        lastWaypointReached = false;
        StopAllCoroutines();
        
    }

    void FixedUpdate()
    {
        /*
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -transform.up, out hit, 20, LayerMask.GetMask("Level"))) //LayerMask.GetMask("Hex")
        {
            if (hit.collider.tag == "Curve")
            {
                distanceToCurve = hit.distance;

                if (distanceToCurve <= distance)
                {
                    OnCurve = true;

                    if(curveFound == false)
                    {
                        curveFound = true;
                        currentCurve = hit.collider.gameObject.transform.parent.gameObject;
                        movementWay = currentCurve.GetComponentInChildren<MovementWay>();


                    }
                    
                }
                else
                {
                    OnCurve = false;
                }
            }
            else
            {
                OnCurve = false;
            }

        }
        else if (lastWaypointReached == true)
        {
            OnCurve = false;
            curveFound = false;

            lastWaypointReached = false;
            waypointCoroutine = null;
        }
        
        if(curveFound == true && waypointCoroutine == null)
        {
            //Find Startpoint
            //mov

            if(waypointCoroutine == null)
                waypointCoroutine = StartCoroutine(MoveToWaypoint(0)); //Startpoint reingeben
        }
        
        */
    }

    int startpoint;
    bool listForward = false;

    void FindStartpoint()
    {
        bool findingStartPoint = true;
        float radius = 10;

        while (findingStartPoint == true)
        {
            Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, myCollider.radius + radius, LayerMask.GetMask("Waypoints"));

            radius = radius - 0.1f;

            if (hitColliders.Length == 1)
            {
                foreach(Collider collider in hitColliders)
                {
                    foreach(GameObject waypoint in movementWay.Waypoints)
                    {
                        if(collider.gameObject == waypoint)
                        {
                            startpoint = movementWay.Waypoints.IndexOf(waypoint);
                            

                            if(startpoint == 0)
                            {
                                listForward = true;
                            }
                            else
                            {
                                listForward = false;
                            }

                        }
                    }
                }

                findingStartPoint = false;

            }
        }
       
    }


    Coroutine waypointCoroutine;
    public bool lastWaypointReached = false;
    [SerializeField] SphereCollider myCollider;
    Vector3 direction;

    IEnumerator MoveToWaypoint(int point)
    {
        //Debug.Log(point);
        bool waypointReached = false;
        

        while (waypointReached == false)
        {
            GameObject waypoint = movementWay.Waypoints[point];
           

            direction = new Vector3(waypoint.transform.position.x - this.transform.position.x, waypoint.transform.position.y - this.transform.position.y, waypoint.transform.position.z - this.transform.position.z).normalized;

            Vector3 movement = direction * Time.deltaTime * speed;
            Vector3 lerpedMovement = new Vector3(Mathf.Lerp(this.transform.position.x, this.transform.position.x + movement.x, 10), Mathf.Lerp(this.transform.position.y, this.transform.position.y + movement.y, 10), Mathf.Lerp(this.transform.position.z, this.transform.position.z + movement.z, 10));

            rb.MovePosition(lerpedMovement);



            Collider[] hitColliders;
            hitColliders = Physics.OverlapSphere(this.transform.position, myCollider.radius + 1, LayerMask.GetMask("Waypoints")); //LayerMask.GetMask("World")
            if(hitColliders != null)
            {
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if ( hitColliders[i].gameObject == movementWay.Waypoints[point])
                    {
                       waypointReached = true;
                       //Debug.Log("waypoint reached");
                    }
                }

            }
            
            yield return null;
        }


        if(listForward == true)
        {
            if (point >= movementWay.Waypoints.Count - 1)
            {
                lastWaypointReached = true;

            }
            else
            {
                StartCoroutine(MoveToWaypoint(point + 1));
            }
        }

        if(listForward)
        
        

        yield return null;
    }



   

}