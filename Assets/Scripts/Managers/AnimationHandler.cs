using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler {

    private Animator anim;
    private string animName;

    public AnimationHandler(string animName, Animator anim) {
        this.animName = animName;
        this.anim = anim;
    }

    public bool WaitToPlay() {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(animName)) return true;
        return false;
    }
}
