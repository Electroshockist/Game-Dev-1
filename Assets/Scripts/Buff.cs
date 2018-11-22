using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff : MonoBehaviour {
    public float value, totalDuration, remainingDuration;

    public string type, name;

    public abstract void Run();
}
