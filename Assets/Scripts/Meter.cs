using System;
using UnityEngine;

[Serializable]
public class Meter {

    [SerializeField]
    public float baseValue;
    [SerializeField]
    private float currentValue;

    public float Value {
        get { return currentValue; }

        set {
            currentValue = value;
        }
    }

    public void Initilaize() {
        Value = baseValue;
    }
}
