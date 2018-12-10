using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffCreator : MonoBehaviour {
    [SerializeField]
    protected string buffName;

    [SerializeField]
    protected float value;

    [SerializeField]
    protected float duration;

    protected void addBuffToPlayer<T>(GameObject g, string name, float value, float duration) where T : Buff{
        g.GetComponent<Character>().createBuff<T>(name, value, duration);
    }

    protected virtual void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            Destroy(gameObject);
        }
    }
}
