using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : Entity {

    Camera playerCamera;
    GameObject cameraOrbit;

    public GameObject DeathScreen;

    [SerializeField]
    private Stat stamina;

    const float waterDamage = 1;

    /*calculates as a percentage of speed.
     A value of 1 would be 200% as fast.*/
    [SerializeField]
    private float sprintSpeedModifier;

    float cameraRotation;

    SpeedBuff WaterSlow, Sprint;

    List<SpeedBuff> speedBuffs;

    //initialize player values
    protected override void Initialize() {
        //get parent's variables
        base.Initialize();

        //get player's camera
        cameraOrbit = GameObject.Find("Camera Orbit Point");
        playerCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        if (health.MaxValue <= 0) health.MaxValue = 100;
        if (stamina.MaxValue <= 0) stamina.MaxValue = 100;
        if (accel <= 0) accel = 3f;
        if (topSpeed.baseValue <= 0) topSpeed.baseValue = 5f;
        if (rotationSpeed <= 0) rotationSpeed = 3.5f;
        if (jumpSpeed <= 0) jumpSpeed = 10f;
        if (horizontalSpeedModifier <= 0) horizontalSpeedModifier = 0.8f;
        if (sprintSpeedModifier <= 0) sprintSpeedModifier = 1.5f;

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
    protected override void Update () {
        base.Update();
        handleBuffs();
        control();
        DeathScreen.SetActive(isDead());
        
	}

    private void control() {
        rotation();
        //motion();
        if (Input.GetKeyDown(KeyCode.T)) {
            transform.position += new Vector3(0, 100, 0);
        }
    }

    private void rotation() {
        //body rotation
        transform.Rotate(0, Input.GetAxis("Mouse X") * rotationSpeed, 0);

        //clamped vertical camera rotation
        cameraRotation += Input.GetAxis("Mouse Y") * rotationSpeed;
        cameraRotation = Mathf.Clamp(cameraRotation, -90, 90);
        cameraOrbit.transform.localEulerAngles = new Vector3(-cameraRotation, transform.localEulerAngles.x, transform.localEulerAngles.z);
    }

    protected override Vector3 MotionAxis() {
        Vector3 inputs = new Vector3(Input.GetAxis("Horizontal"), rb.velocity.y, Input.GetAxis("Vertical"));

        if (isJumping()) inputs.y = 1;
        else inputs.y = 0;

        return inputs;
    }

    private void motion() {
        //acceleration();

        inWater();        
        sprint();        

        //jumping
        if (isJumping()) {
            moveDirection.y = 0;
            moveDirection.y = jumpSpeed;
            Debug.Log("Jumping");
        }
    }

    //animates player
    protected override void Animate() {
        anim.SetFloat("Movement Speed", currentSpeed);
        anim.SetBool("Moving", isMoving());

        if (isJumping()) {
            anim.SetTrigger("Jump");
        }
    }

    //checks if moving
    private bool isMoving() {
        if (Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.y) + Mathf.Abs(moveDirection.z) != 0) return true;
        return false;
    }

    //checks if jumping
    private bool isJumping() {
        if (Input.GetButtonDown("Jump") && isGrounded) return true;
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
            return true;
        }
        return false;
    }

    private float totalBuffValue(List<SpeedBuff> buffs) {
        float total = 0;

        for (int i = 0; i < buffs.Count; i++) {
            Debug.Log(buffs[i].name);
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
