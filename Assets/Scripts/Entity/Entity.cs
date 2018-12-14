using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour {

    [SerializeField]
    protected Rigidbody rb;

    [SerializeField]
    protected Animator anim;

    protected abstract Vector3 MotionAxis();

    [SerializeField]
    protected float currentSpeed = 0;

    //player controller variables
    [SerializeField]
    protected float accel, decel, rotationSpeed, jumpSpeed, horizontalSpeedModifier;

    [SerializeField]
    protected Meter topSpeed = new Meter();

    [SerializeField]
    protected bool isGrounded;

    [SerializeField]
    protected Vector3 moveDirection = Vector3.zero;

    // Use this for initialization
    void Start () {
        Initialize();
    }

    // Update is called once per frame
    protected virtual void Update () {
        Animate();
        Move(MotionAxis());
	}

    //initialize player values
    protected virtual void Initialize() {
        //get rigidbody
        rb = GetComponent<Rigidbody>();
    }

    protected void Move(Vector3 v) {
        //accelerate/deceleate
        Acceleration();

        //forward/backward speed
        moveDirection.z = v.z * currentSpeed;

        //horizontal speed
        moveDirection.x = v.x * currentSpeed * horizontalSpeedModifier;

        //upward/downward speed
        if (jumpSpeed != 0) rb.AddForce(0, rb.velocity.y + v.y * jumpSpeed, 0, ForceMode.Impulse);

        moveDirection = transform.TransformDirection(moveDirection);

        rb.velocity =  moveDirection;
    }

    //acceleration calculations
    private void Acceleration() {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
            //accelerate
            currentSpeed += accel * Time.deltaTime;

            //current speed caps at top speed
            if (currentSpeed > topSpeed.Value) currentSpeed = topSpeed.Value;
        }
        else {
            currentSpeed = 0;
            ////accelerate
            //currentSpeed -= decel * Time.deltaTime;
            ////current speed caps at 0
            //if (currentSpeed <= 0) currentSpeed = 0;
        }
    }

    //ground checkers

    private void OnTriggerStay(Collider collision) {
        isGrounded = true;
    }

    private void OnTriggerExit(Collider collision) {
        isGrounded = false;
    }

    protected abstract void Animate();
}
