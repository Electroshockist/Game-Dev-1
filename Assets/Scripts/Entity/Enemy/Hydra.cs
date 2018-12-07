using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hydra : Enemy {
    [SerializeField]
    GameObject prefabEffect;
    [SerializeField]
    Transform[] prefabLocation;

    public float projectileDamage;

    // Use this for initialization
    protected override void Start() {
        base.Start();
        animManagers.Add(new AnimationHandler("3toxicSpitCombo", anim));
        prefabEffect.GetComponent<Projectile>().damage = projectileDamage;
    }

    // Update is called once per frame
    protected override void Update() {
        if (getDistance() <= attackRadius && !animManagers[0].WaitToPlay()) {
            anim.SetTrigger("Attack");
            StartCoroutine("spit");
        }
        else Follow();

        base.Update();
    }

    private IEnumerator spit() {
        yield return new WaitForSeconds(0.5f);

        foreach (Transform t in prefabLocation) {
            Instantiate(prefabEffect, t.position, t.rotation);
        }
    }
}
