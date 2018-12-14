using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour {
    [SerializeField]
    private float heal = 10;
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            other.GetComponentInParent<Character>().Heal(heal);
            Destroy(gameObject);
        }
    }
}
