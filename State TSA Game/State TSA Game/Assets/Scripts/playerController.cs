using UnityEngine;
using UnityEngine.InputSystem;

public class playerController1 : MonoBehaviour
{
    public Rigidbody rb;
    public Transform raypoint;
    public Transform respawnPoint;


    public float forwardAcc, reverseAcc, maxSpeed, turnStrength, gravityForce, dragOnGround, dragInAir;
    public string horizontal, vertical;
    private float speedInput, turnInput;
    private bool grounded;
    

    public LayerMask whatIsGround, speedbump;
    public float groundRayLength;
    public LayerMask deathGround;

    private Vector2 movementInput = Vector2.zero;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.transform.parent = null;
        respawn();
    }

    // Update is called once per frame
    void Update()
    {   
        speedInput = 0f;

        if(Input.GetAxis(vertical) > 0)
        {
            speedInput = Input.GetAxis(vertical) * forwardAcc  * 100f;
        } else if(Input.GetAxis(vertical) < 0 )
        {
            speedInput = Input.GetAxis(vertical) * reverseAcc * 100f;
        }

        turnInput = Input.GetAxis(horizontal);
        if(grounded)
        {
            if(rb.linearVelocity.magnitude > 1.0f)
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * (rb.linearVelocity.magnitude * 0.3f)));
        }
        transform.position = rb.transform.position;
    }

    void OnTriggerEnter(Collider collided)
    {
        if(collided.gameObject.CompareTag("FloorIsLava"))
        {
            respawn();
        }
    }



    void FixedUpdate()
    {

        grounded = false;
        RaycastHit hit;

        if(Physics.Raycast(raypoint.position, -transform.up, out hit, groundRayLength, deathGround))
        {
            respawn();
        }

        if(Physics.Raycast(raypoint.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            grounded = true;
            if(!(Physics.Raycast(raypoint.position, -transform.up, out hit, groundRayLength, speedbump)))
            {
                transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            }

        }

        if(grounded)
        {
            rb.linearDamping = dragOnGround;
            Vector3 movementForce = transform.forward * speedInput;
            rb.AddForce(movementForce);
            if(rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed);
            }
        } else
        {
            rb.linearDamping = dragInAir;
            rb.AddForce(-gravityForce * Vector3.up * 100f);
        }
    }

    private void respawn(){
        rb.transform.position = respawnPoint.transform.position;
        rb.linearDamping = 100000000;
        rb.linearDamping = dragOnGround;
    }
}

