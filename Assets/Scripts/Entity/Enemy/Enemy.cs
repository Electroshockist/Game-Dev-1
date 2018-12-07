using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour {

    public float noticeRadius, attackRadius, damage;    

    [SerializeField]
    protected Animator anim;
    protected Character target;
    protected Transform targetPos;
    protected NavMeshAgent agent;

    //handle animations
    protected List<AnimationHandler> animManagers = new List<AnimationHandler>();

	// Use this for initialization
	protected virtual void Start () {
        target = PlayerManager.instance.player.GetComponent<Character>();
        targetPos = target.transform;
        agent = GetComponent<NavMeshAgent>();

        if (noticeRadius <= 0) noticeRadius = 10f;
        if (attackRadius <= 0) attackRadius = 2f;
        if (damage <= 0) damage = 15f;

        agent.stoppingDistance = attackRadius;	
	}

    // Update is called once per frame
    protected virtual void Update () {
        if (getDistance() <= noticeRadius) agent.SetDestination(targetPos.position);
        Animate();
    }

    protected void RotateTowards() {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * agent.angularSpeed);
    }

    protected float getDistance() {
        return Vector3.Distance(targetPos.position, transform.position);
    }

    protected virtual void Animate() {
        anim.SetBool("Moving", isMoving());
    }

    protected bool isMoving() {
        if (agent.velocity.x + agent.velocity.y + agent.velocity.z == 0) return false;
        return true;
    }

    protected void Follow() {        
        if (getDistance() <= noticeRadius) agent.SetDestination(targetPos.position);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, noticeRadius);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
