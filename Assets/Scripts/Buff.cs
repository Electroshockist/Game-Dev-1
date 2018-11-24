using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff {
    public float value, totalDuration, remainingDuration;

    public string type, name;

    public void Run() {
        if (remainingDuration > 0) remainingDuration -= Time.deltaTime;
    }
}
