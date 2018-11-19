using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float projectileLifetime, projectileForce;
    Rigidbody rb;
    CharacterController rootCc;

	// Use this for initialization
	void Start () {
        rootCc = GameObject.Find("Character").GetComponent<CharacterController>();

        if (projectileLifetime <= 0) projectileLifetime = 5.0f;
        if (projectileForce <= 0) projectileForce = 10.0f;

        rb = GetComponent<Rigidbody>();
        if (!rb) rb = gameObject.AddComponent<Rigidbody>();

        rb.AddForce(transform.forward * projectileForce + rootCc.velocity, ForceMode.Impulse);

        Destroy(gameObject, projectileLifetime);		
	}
}
