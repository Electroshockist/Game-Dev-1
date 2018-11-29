using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour {

    public Animator anim;

    Camera playerCamera;
    GameObject cameraOrbit;
    CharacterController cc;

    public GameObject DeathScreen;

    [SerializeField]
    private Stat health, stamina;

    float waterDamage = 1;

    //player controller variables
    [SerializeField]
    private Meter topSpeed = new Meter();
    [SerializeField]
    private float accel, decel, rotationSpeed, jumpSpeed, horizontalSpeedModifier, gravity;

    private float currentSpeed = 0;

    /*calculates as a percentage of speed.
     A value of 1 would be 200% as fast.*/
    [SerializeField]
    private float sprintSpeedModifier;

    float cameraRotation;

    Vector3 moveDirection = Vector3.zero;

    SpeedBuff WaterSlow, Sprint;

    List<SpeedBuff> speedBuffs;

    //initialize player values
    private void Initialize() {
        //get player's camera
        cameraOrbit = GameObject.Find("Camera Orbit Point");
        playerCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        //get character controller
        cc = GetComponent<CharacterController>();

        if (health.MaxValue <= 0) health.MaxValue = 100;
        if (stamina.MaxValue <= 0) stamina.MaxValue = 100;
        if (accel <= 0) accel = 0.3f;
        if (gravity <= 0) gravity = -1.0f;
        if (topSpeed.baseValue <= 0) topSpeed.baseValue = 0.1f;
        if (rotationSpeed <= 0) rotationSpeed = 3.5f;
        if (jumpSpeed <= 0) jumpSpeed = 0.5f;
        if (horizontalSpeedModifier <= 0) horizontalSpeedModifier = 0.8f;
        if (sprintSpeedModifier <= 0) sprintSpeedModifier = 0.5f;

        health.CurrentValue = health.MaxValue;
        stamina.CurrentValue = stamina.MaxValue;

        topSpeed.value = topSpeed.baseValue;

        WaterSlow = new SpeedBuff("waterSlow", 0.5f, 0);
        Sprint = new SpeedBuff("sprint", sprintSpeedModifier + 1, 0);

        initializeLists();
    }

    // Use this for initialization
    private void Start () {
        Initialize();
    }

    // Update is called once per frame
    private void Update () {
        handleBuffs();
        control();
        animate();
        isDead();
	}

    private void control() {
        rotation();
        motion();
    }

    private void rotation() {
        //body rotation
        transform.Rotate(0, Input.GetAxis("Mouse X") * rotationSpeed, 0);

        //clamped vertical camera rotation
        cameraRotation += Input.GetAxis("Mouse Y") * rotationSpeed;
        cameraRotation = Mathf.Clamp(cameraRotation, -90, 90);
        cameraOrbit.transform.localEulerAngles = new Vector3(-cameraRotation, transform.localEulerAngles.x, transform.localEulerAngles.z);
    }

    private void motion() {
        acceleration();        

        //movement
        moveDirection.z = Input.GetAxis("Vertical") * currentSpeed;
        moveDirection.x = Input.GetAxis("Horizontal") * currentSpeed * horizontalSpeedModifier;

        inWater();        
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
    private void animate() {
        anim.SetFloat("Movement Speed", currentSpeed);
        anim.SetBool("Moving", isMoving());

        //figure out why this doesn't work
        if (isJumping()) {
            anim.SetTrigger("Jump");
            Debug.Log("Jumping");
        }
    }

    //acceleration calculations
    private void acceleration() {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
            //accelerate
            currentSpeed += accel * Time.deltaTime;

            //current speed caps at top speed
            if (currentSpeed > topSpeed.value) currentSpeed = topSpeed.value;
        }
        else {
            currentSpeed = 0;
            ////accelerate
            //currentSpeed -= decel * Time.deltaTime;
            ////current speed caps at 0
            //if (currentSpeed <= 0) currentSpeed = 0;
        }
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

    //sprinting
    private void sprint() {

        //todo: animate camera fov
        if (Input.GetButtonDown("Sprint")) playerCamera.fieldOfView += 15;
        if (Input.GetButtonUp("Sprint")) playerCamera.fieldOfView -= 15;

        if (Input.GetButtonDown("Sprint") && speedBuffs.IndexOf(Sprint) < 0) speedBuffs.Add(Sprint);        

        else if (Input.GetButtonUp("Sprint") && speedBuffs.IndexOf(Sprint) >= 0) speedBuffs.Remove(Sprint);
    }

    //checks if is in water
    private void inWater() {

        if (transform.position.y <= 250) {
            health.CurrentValue -= waterDamage * Time.deltaTime * (250 - transform.position.y);
            if (speedBuffs.IndexOf(WaterSlow) < 0)
                speedBuffs.Add(WaterSlow);
        }
        else if (speedBuffs.IndexOf(WaterSlow) >= 0)
            speedBuffs.Remove(WaterSlow);
    }

    private bool isDead() {
        if (health.CurrentValue <= 0) {
            DeathScreen.SetActive(true);
            return true;
        }
        return false;
    }

    private float totalBuffValue(List<SpeedBuff> buffs) {
        float total = 0;

        for (int i = 0; i < buffs.Count; i++) {
            total += buffs[i].value;
        }

        return total;
    }

    private void handleBuffs() {
        if (speedBuffs.Count == 0) {
            topSpeed.value = topSpeed.baseValue;
            return;
        }
        topSpeed.value = topSpeed.baseValue * totalBuffValue(speedBuffs);
        Debug.Log(totalBuffValue(speedBuffs));
    }

    private void initializeLists() {
        speedBuffs = new List<SpeedBuff>();
    }

    public void Damage(float damage) {
        health.CurrentValue -= damage;
    }
}
