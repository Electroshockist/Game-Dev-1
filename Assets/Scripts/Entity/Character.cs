using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : Entity {

    Camera playerCamera;
    GameObject cameraOrbit;

    [SerializeField]
    public GameObject WinScreen;

    [SerializeField]
    public GameObject DeathScreen;

    [SerializeField]
    private Stat health;

    [SerializeField]
    private Stat stamina;

    float lerpedSpeed;

    /*calculates as a percentage of speed.
    A value of 1 would be 200% as fast.*/
    private float sprintSpeedModifier, waterSlow;

    //handle animations
    private List<AnimationHandler> animManagers = new List<AnimationHandler>();

    float cameraRotation;

    //internal speed buffs
    SpeedBuff Sprint, WaterSlow;

    //initialize player values
    protected override void Initialize() {
        //get parent's variables
        base.Initialize();

        animManagers.Add(new AnimationHandler("Stable Sword Outward Slash", anim));

        //get player's camera
        cameraOrbit = GameObject.Find("Camera Orbit Point");
        playerCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        if (health.MaxValue <= 0) health.MaxValue = 100;
        if (stamina.MaxValue <= 0) stamina.MaxValue = 100;
        if (accel <= 0) accel = 10f;
        if (topSpeed.baseValue <= 0) topSpeed.baseValue = 5f;
        if (rotationSpeed <= 0) rotationSpeed = 3.5f;
        if (jumpSpeed <= 0) jumpSpeed = 2.5f;
        if (horizontalSpeedModifier <= 0) horizontalSpeedModifier = 0.8f;
        if (sprintSpeedModifier <= 0) sprintSpeedModifier = 1.5f;
        if (waterSlow <= 0) waterSlow = 0.5f;

        //sprintSpeedModifier will aslways be greater or equal to 1(100%)
        sprintSpeedModifier++;

        health.CurrentValue = health.MaxValue;
        stamina.CurrentValue = stamina.MaxValue;

        topSpeed.Value = topSpeed.baseValue;
    }
    private void Awake() {
        health.Initilaize();
    }
    // Use this for initialization
    private void Start() {
        Initialize();
    }

    // Update is called once per frame
    protected override void Update() {
        if (!Attack()) {
            base.Update();
        }
        handleBuffs(topSpeed, getBuffs<SpeedBuff>());

        
            control();
        DeathScreen.SetActive(isDead());
    }

    private bool Attack() {
        if (Input.GetButtonDown("Fire1") && !animManagers[0].WaitToPlay()) {
            anim.SetTrigger("Attack");
            return true;
        }
        else if (animManagers[0].WaitToPlay()) return true;
        return false;

    }

    private void control() {
        rotation();
        motion();

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
        inWater();
        sprint();

        //jumping
        if (isJumping()) {
            moveDirection.y = jumpSpeed;
        }
        else moveDirection.y = 0;
    }

    //animates player
    protected override void Animate() {
        lerpedSpeed = currentSpeed / topSpeed.baseValue;
        anim.SetFloat("Movement Speed", lerpedSpeed);
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
        if (Input.GetButtonDown("Sprint")) {
            playerCamera.fieldOfView += 15;
            if(!buffExists(Sprint)) Sprint = createBuff<SpeedBuff>("Sprint", sprintSpeedModifier);
        }
        if (Input.GetButtonUp("Sprint")) {
            playerCamera.fieldOfView -= 15;
            removeBuff(Sprint);
        }
    }

    //checks if is in water
    private void inWater() {

        if (transform.position.y <= 250) {
            //damage while in water based off of deapth
            health.CurrentValue -= Time.deltaTime * (250 - transform.position.y);

            if (!buffExists(WaterSlow)) WaterSlow = createBuff<SpeedBuff>("Water Slow", waterSlow);
        }
        else removeBuff(WaterSlow);
    }

    private void handleBuffs<T,U>(T value, List<U> list) where T : Meter where U : Buff {
        if (list.Count == 0) {
            value.Value = value.baseValue;
            return;
        }
        value.Value = value.baseValue * TotalBuffValue(list);
    }

    //get total buff value
    private float TotalBuffValue<T>(List<T> buffs) where T : Buff{
        //total wil be multiplied by each buff value in the list
        float total = 1;

        for (int i = 0; i < buffs.Count; i++) {
            total *= buffs[i].Value;
        }

        return total;
    }

    //get buffs of a generic type Buff
    private List<T> getBuffs<T>() where T : Buff{
        var buffs = FindObjectsOfType<T>();
        return new List<T>(buffs);
    }

    public T createBuff<T>(string name, float value) where T : Buff{
        T buff = gameObject.AddComponent<T>();
        buff.Create(name, value);
        return buff;
    }

    public T createBuff<T>(string name, float value, float duration) where T : Buff {
        T buff = gameObject.AddComponent<T>();
        buff.Create(name, value, duration);
        return buff;
    }

    private bool buffExists<T>(T buff) where T : Buff {
        if(getBuffs<T>().IndexOf(buff) < 0) return false;
        return true;
    }

    private void removeBuff<T>(T buff) where T : Buff {
        if (buffExists(buff)) Destroy(buff);
    }

    public void Heal(float heal) {
        health.CurrentValue += heal;
    }

    public void Damage(float damage) {
        health.CurrentValue -= damage;
    }

    private bool isDead() {
        if (health.CurrentValue <= 0) {
            return true;
        }
        return false;
    }
}
