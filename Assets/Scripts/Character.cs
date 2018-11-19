using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour {

    public Animator anim;

    Camera camera1;

    CharacterController cc;

    public Projectile projectilePrefab;
    public Transform projectileSpawnpoint;

    //player controller variables
    public float accel, topSpeed, topSprintSpeed, rotationSpeed, jumpSpeed, horizontalSpeedModifier, gravity;

    public float currentSpeed = 0;

    /*calculates as a percentage of speed.
     A value of 1 would be 200% as fast.*/
    public float sprintSpeedModifier;

    float cameraRotation;

    Vector3 moveDirection = Vector3.zero;

    // Use this for initialization
    void Start () {
        if (accel <= 0) accel = 0.3f;

        if (gravity <= 0) gravity = -1.0f;

        if (topSpeed <= 0) topSpeed = 0.3f;

        if (rotationSpeed <= 0) rotationSpeed = 3.5f;

        if (jumpSpeed <= 0) jumpSpeed = 0.5f;

        if (horizontalSpeedModifier <= 0) horizontalSpeedModifier = 0.8f;

        if (sprintSpeedModifier <= 0) sprintSpeedModifier = 0.5f;

        //get player's camera
        camera1  = transform.Find("Main Camera").GetComponent<Camera>();

        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update () {
        control();
        animate();
	}

    void control() {

        rotation();

        motion();

        //sprinting
        //todo: animate camera fov
        if (Input.GetButtonDown("Sprint")) camera1.fieldOfView += 15;
        if (Input.GetButtonUp("Sprint")) camera1.fieldOfView -= 15;

        //bullets
        if (Input.GetButtonDown("Fire1")) fire();
    }

    void rotation() {
        //body rotation
        transform.Rotate(0, Input.GetAxis("Mouse X") * rotationSpeed, 0);

        //clamped vertical camera rotation
        cameraRotation += Input.GetAxis("Mouse Y") * rotationSpeed;

        cameraRotation = Mathf.Clamp(cameraRotation, -90, 90);

        camera1.transform.localEulerAngles = new Vector3(-cameraRotation, transform.localEulerAngles.x, transform.localEulerAngles.z);
    }

    void motion() {
        //acceleration calculations
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
            //accelerate
            currentSpeed += accel * Time.deltaTime;

            //current speed caps at top speed
            if (currentSpeed > topSpeed) currentSpeed = topSpeed;
        }
        else currentSpeed = 0;

        //moving and sprinting
        //horizontal movement
        moveDirection.x = Input.GetAxis("Horizontal") * currentSpeed * horizontalSpeedModifier;

        //forward/backward movement
        if (Input.GetButton("Sprint")) {
            moveDirection.z = Input.GetAxis("Vertical") * currentSpeed + Input.GetAxis("Vertical") * currentSpeed * sprintSpeedModifier;
        }
        else moveDirection.z = Input.GetAxis("Vertical") * currentSpeed;

        //jumping
        if (Input.GetButtonDown("Jump") && cc.isGrounded) {
            moveDirection.y = 0;
            moveDirection.y = jumpSpeed;
        }

        //gravity
        if (!cc.isGrounded) moveDirection.y += gravity * Time.deltaTime;

        moveDirection = transform.TransformDirection(moveDirection);
        cc.Move(moveDirection);
    }

    void animate() {
        if (isControlling()) {
            anim.SetBool("MOVING", true);
        }
        else  anim.SetBool("MOVING", false);

    }

    bool isControlling() {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) return true;
        else return false;
    }

    void fire() {
        if (projectilePrefab && projectileSpawnpoint) Instantiate(projectilePrefab, projectileSpawnpoint.position, projectileSpawnpoint.rotation);
    }
}
