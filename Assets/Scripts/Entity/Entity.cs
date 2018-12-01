using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour {

    [SerializeField]
    protected Rigidbody rb;

    [SerializeField]
    protected Animator anim;

    [SerializeField]
    protected Stat health;

    protected abstract Vector3 MotionAxis();

    [SerializeField]
    protected float currentSpeed = 0;

    //player controller variables
    [SerializeField]
    protected float accel, decel, rotationSpeed, jumpSpeed, horizontalSpeedModifier;

    [SerializeField]
    protected Meter topSpeed = new Meter();

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
        //forward/backward speed
        moveDirection.z = v.z * currentSpeed;

        //horizontal speed
        moveDirection.x = v.x * currentSpeed * horizontalSpeedModifier;

        //upward/downward speed
        rb.AddForce(0, v.y * jumpSpeed, 0, ForceMode.Impulse);

        rb.velocity =  moveDirection;
    }

    protected abstract void Animate();
}
