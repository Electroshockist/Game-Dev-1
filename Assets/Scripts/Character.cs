using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour {

    public Animator anim;

    Camera playerCamera;

    GameObject cameraOrbit;

    CharacterController cc;

    public Projectile projectilePrefab;
    public Transform projectileSpawnpoint;

    //player controller variables
    public float accel, decel, baseTopSpeed, rotationSpeed, jumpSpeed, horizontalSpeedModifier, gravity;

    public float currentSpeed = 0;
    public float topSpeed;

    /*calculates as a percentage of speed.
     A value of 1 would be 200% as fast.*/
    public float sprintSpeedModifier;

    float cameraRotation;

    Vector3 moveDirection = Vector3.zero;

    List<SpeedBuff> speedBuffs;

    // Use this for initialization
    void Start () {
        if (accel <= 0) accel = 0.3f;

        if (gravity <= 0) gravity = -1.0f;

        if (baseTopSpeed <= 0) baseTopSpeed = 0.1f;

        if (rotationSpeed <= 0) rotationSpeed = 3.5f;

        if (jumpSpeed <= 0) jumpSpeed = 0.5f;

        if (horizontalSpeedModifier <= 0) horizontalSpeedModifier = 0.8f;

        if (sprintSpeedModifier <= 0) sprintSpeedModifier = 0.5f;

        topSpeed = baseTopSpeed;

        //get player's camera

        cameraOrbit = GameObject.Find("Camera Orbit Point");

        playerCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        //get character controller
        cc = GetComponent<CharacterController>();

        initializeLists();
    }

    // Update is called once per frame
    void Update () {
        control();
        animate();
	}

    void control() {

        rotation();

        motion();

        //bullets
        if (Input.GetButtonDown("Fire1")) fire();
    }

    void rotation() {
        //body rotation
        transform.Rotate(0, Input.GetAxis("Mouse X") * rotationSpeed, 0);

        //clamped vertical camera rotation
        cameraRotation += Input.GetAxis("Mouse Y") * rotationSpeed;

        cameraRotation = Mathf.Clamp(cameraRotation, -90, 90);

        cameraOrbit.transform.localEulerAngles = new Vector3(-cameraRotation, transform.localEulerAngles.x, transform.localEulerAngles.z);
    }

    void motion() {
        acceleration();        

        //movement
        moveDirection.z = Input.GetAxis("Vertical") * currentSpeed;
        moveDirection.x = Input.GetAxis("Horizontal") * currentSpeed * horizontalSpeedModifier;

        isInWater();
        
        sprint();        

        //jumping
        if (isJumping()) {
            moveDirection.y = 0;
            moveDirection.y = jumpSpeed;
            Debug.Log("Jumping");
        }

        //gravity
        if (!cc.isGrounded) moveDirection.y += gravity * Time.deltaTime;

        moveDirection = transform.TransformDirection(moveDirection);
        cc.Move(moveDirection);
    }

    //animates player
    void animate() {
        anim.SetFloat("Movement Speed", currentSpeed);
        anim.SetBool("Moving", isMoving());

        //figure out why this doesn't work
        if (isJumping()) {
            anim.SetTrigger("Jump");
            Debug.Log("Jumping");
        }
    }

    //acceleration calculations
    void acceleration() {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
            //accelerate
            currentSpeed += accel * Time.deltaTime;

            //current speed caps at top speed
            if (currentSpeed > topSpeed) currentSpeed = topSpeed;
        }
        else {
            currentSpeed = 0;
            ////accelerate
            //currentSpeed -= decel * Time.deltaTime;
            ////current speed caps at 0
            //if (currentSpeed <= 0) currentSpeed = 0;
        }
    }

    //sprinting
    private void sprint() {
        //todo: animate camera fov
        if (Input.GetButtonDown("Sprint")) playerCamera.fieldOfView += 15;
        if (Input.GetButtonUp("Sprint")) playerCamera.fieldOfView -= 15;

        if (Input.GetButton("Sprint")) topSpeed = baseTopSpeed + baseTopSpeed * sprintSpeedModifier;
        else topSpeed = baseTopSpeed;
    }

    void fire() {
        if (projectilePrefab && projectileSpawnpoint) Instantiate(projectilePrefab, projectileSpawnpoint.position, projectileSpawnpoint.rotation);
    }

    private bool isInWater() {
        if (speedBuffs.Contains(SpeedBuff s()))
        if (transform.position.y <= 250) {
            topSpeed /= 2;
            Debug.Log(topSpeed);
            return true;
        }
        topSpeed = baseTopSpeed;
        return false;
    }

    //checks if moving
    private bool isMoving() {
        if (Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.y) + Mathf.Abs(moveDirection.z) == 0) return true;
        return false;
    }

    //checks if jumping
    private bool isJumping() {
        if (Input.GetButtonDown("Jump") && cc.isGrounded) return true;
        return false;
    }

    private float totalBuffValue(List<Buff> buffs) {
        float total = 0;
        for (int i = 0; i < buffs.Count; i++) {
            Debug.Log(buffs[i].value);
            total += buffs[i].value;
        }
        return total;
    }

    private void handleBuffs() {

    }

    private void initializeLists() {
        speedBuffs = new List<SpeedBuff>();
    }
}
