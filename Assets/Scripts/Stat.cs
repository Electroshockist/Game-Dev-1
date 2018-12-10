using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat {
    [SerializeField]
    private UIMeter meter;

    [SerializeField]
    private float maxValue;
    [SerializeField]
    private float currentValue;

    public float MaxValue {
        get {
            return maxValue;
        }

        set {
            meter.MaxValue = value;
            maxValue = meter.MaxValue;
        }
    }
    public float CurrentValue {
        get { return currentValue; }

        set {
            currentValue = value;
            meter.Value = currentValue;
        }
    }

    public void Initilaize() {
        MaxValue = maxValue;
        CurrentValue = maxValue;
    }
}
