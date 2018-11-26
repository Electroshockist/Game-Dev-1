using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMeter : MonoBehaviour {

    [SerializeField]
    private float fillAmount;

    [SerializeField]
    private Image meter;

    public float MaxValue { get; set; }
    public float Value {
        set {
            fillAmount = Lerp(value, MaxValue);
        }
    }

    // Use this for initialization
    void Start () {
        meter = gameObject.GetComponent<Image>();		
	}
	
	// Update is called once per frame
	void Update () {
        Handle();
	}
    private void Handle() {
        if (fillAmount != meter.fillAmount)  meter.fillAmount = fillAmount;
    }

    private float Lerp(float value, float maxValue) {
        return value / maxValue;
    }
}
