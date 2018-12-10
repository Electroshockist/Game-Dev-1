using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinBall : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            other.GetComponent<Character>().WinScreen.SetActive(true);
            Destroy(gameObject);
        }
    }
}
