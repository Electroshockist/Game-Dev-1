using UnityEngine;

//abstract parent buff class
public abstract class Buff : MonoBehaviour{
    [SerializeField]
    protected string buffName, type;

    [SerializeField]
    protected float value, duration;

    public string Name {
        get {
            return buffName;
        }
    }

    public string Type {
        get {
            return type;
        }
    }

    public float Value {
        get {
            return value;
        }
    }

    public float Duration {
        get {
            return duration;
        }
    }

    public virtual void Create(string name, float value) {
        Create(name, value, 0);
    }

    public virtual void Create(string name, float value, float duration) {
        buffName = name;
        this.value = value;
        this.duration = duration;

        if (duration > 0) {
            Destroy(this, duration);
        }
    }
}
