using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Damager : MonoBehaviour{
    public float damage;

    private void OnTriggerEnter(Collider c) {
        //if on player, don't hurt player
        if(tag == "Friendly") {
            if(c.tag == "Enemy") c.GetComponentInParent<Enemy>().Damage(damage);
        } 
        else if (c.tag == "Player") {
            c.GetComponentInParent<Character>().Damage(damage);
        }
    }
}
