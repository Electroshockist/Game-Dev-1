using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : Buff {

    public SpeedBuff(string name, float value) {
        type = "speed";
        this.name = name;
        this.value = value;
    }
    public SpeedBuff(string name, float value, float duration) {
        type = "speed";
        this.name = name;
        this.value = value;
        totalDuration = duration;
        remainingDuration = totalDuration;
    }
}
