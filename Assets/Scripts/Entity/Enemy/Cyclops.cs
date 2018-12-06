﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cyclops : Enemy {

    // Use this for initialization
    protected override void Start () {
        base.Start();
        animManagers.Add(new AnimationHandler("2HitComboAttackForward", anim));
    }

    // Update is called once per frame
    protected override void Update () {
        if (getDistance() <= noticeRadius) agent.SetDestination(targetPos.position);

        if (getDistance() <= attackRadius && !animManagers[0].WaitToPlay()) anim.SetTrigger("Attack");

        base.Update();
    }
}
