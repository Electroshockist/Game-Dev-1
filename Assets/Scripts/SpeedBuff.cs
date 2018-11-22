using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : Buff {

	public SpeedBuff(float value, float duration) {
        type = "speed";
        this.value = value;
        totalDuration = duration;
        remainingDuration = totalDuration;
    }

    public override void Run() {
        remainingDuration -= Time.deltaTime;
    }
}
