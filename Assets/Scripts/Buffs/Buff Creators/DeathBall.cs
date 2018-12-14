using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBall : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            other.GetComponentInParent<Character>().Damage(1000);
            Destroy(gameObject);
        }
    }
}
