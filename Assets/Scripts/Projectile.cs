using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float damage;

    void Start() {
        var tm = GetComponentInChildren<RFX4_TransformMotion>(true);
        if (tm != null) tm.CollisionEnter += Tm_CollisionEnter;
        if (damage == 0) damage = 10;
    }

    private void Tm_CollisionEnter(object sender, RFX4_TransformMotion.RFX4_CollisionInfo e) {
        if(e.Hit.transform.tag == "Player") {
            e.Hit.transform.gameObject.GetComponent<Character>().Damage(damage);
        }
    }
}
