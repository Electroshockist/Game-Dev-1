using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuffCreator : BuffCreator {
    protected override void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            addBuffToPlayer<SpeedBuff>(other.gameObject, buffName, value, duration);
        }

        base.OnTriggerEnter(other);
    }
}
