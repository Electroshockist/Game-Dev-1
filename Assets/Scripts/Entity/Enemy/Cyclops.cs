using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cyclops : Enemy {

    private bool attacking = false;

    // Use this for initialization
    protected override void Start () {
        base.Start();		
	}

    // Update is called once per frame
    protected override void Update () {
        float distance = Vector3.Distance(targetPos.position, transform.position);
        if (distance <= noticeRadius) agent.SetDestination(targetPos.position);

        //stop animation from retriggering while playing
        if (!attacking) {
            attacking = true;
            if (distance <= attackRadius) anim.SetTrigger("Attack");

        }
        if (attacking && !anim.GetCurrentAnimatorStateInfo(0).IsName("2HitComboAttackForward")) {
            attacking = false;
        }

        base.Update();
    }
}
