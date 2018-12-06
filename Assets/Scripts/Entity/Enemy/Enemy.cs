using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    public float noticeRadius, attackRadius, damage;

    [SerializeField]
    protected Animator anim;
    protected Character target;
    protected Transform targetPos;
    protected NavMeshAgent agent;

    public Damager[] damagers;

	// Use this for initialization
	protected virtual void Start () {
        if (noticeRadius <= 0) noticeRadius = 10f;
        if (attackRadius <= 0) attackRadius = 2f;
        if (damage <= 0) damage = 15f;

        target = PlayerManager.instance.player.GetComponent<Character>();
        targetPos = target.transform;
        agent = GetComponent<NavMeshAgent>();		
	}

    // Update is called once per frame
    protected virtual void Update () {
        setDamagerDamage();
        Animate();
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, noticeRadius);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    protected virtual void Animate() {
        anim.SetBool("Moving", isMoving());
    }

    private bool isMoving() {
        if (agent.velocity.x + agent.velocity.y + agent.velocity.z == 0) return false;
        return true;
    }

    protected void setDamagerDamage() {
        for(int i = 0; i < damagers.Length; i++) {
            damagers[i].damage = damage;
        }
    }
}
