using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Damager : MonoBehaviour{
    public float damage;

    private void OnTriggerEnter(Collider c) {
        if (c.tag == "Player") {
            c.GetComponent<Character>().Damage(damage);
        }
    }
}
