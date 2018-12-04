using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {
    float DamageValue = 10;

    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.tag == "Player") collision.gameObject.GetComponent<Character>().Damage(DamageValue);
    }
}
