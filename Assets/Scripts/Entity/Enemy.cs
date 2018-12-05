using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity {

    public float noticeRadius = 10f;

    Transform target;

    NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
		
	}

    // Update is called once per frame
    private void Update () {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= noticeRadius) agent.SetDestination(target.position);
	}

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, noticeRadius);
    }

    protected override Vector3 MotionAxis() {
        throw new System.NotImplementedException();
    }

    protected override void Animate() {
        throw new System.NotImplementedException();
    }
}
