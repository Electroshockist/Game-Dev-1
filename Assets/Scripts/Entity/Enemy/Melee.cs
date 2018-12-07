using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Melee : Enemy {
    //damage areas that are not neccesarily connected to entitiy directly
    [SerializeField]
    protected Damager[] damagers;

    // Use this for initialization
    protected override void Start() {
        base.Start();
	}

    // Update is called once per frame
    protected override void Update () {
        setDamagerDamage();
        base.Update();
	}

    protected void setDamagerDamage() {
        for (int i = 0; i < damagers.Length; i++) {
            damagers[i].damage = damage;
        }
    }
}
