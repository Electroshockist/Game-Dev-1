using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourChanger : MonoBehaviour {
    Renderer ren;
    Color c;

    void Start() {
        ren = gameObject.GetComponent<Renderer>();
        c = ren.material.color;
        c.g += 10.0f;
        ren.material.color = c;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("Fire2")) StartCoroutine("ColourChange",0.01f);		
	}

    IEnumerator ColourChange(float time) {
        for (float f = 10.0f; f >= 0; f -= 0.01f) {            
            c.b = f;
            c.g = 10.0f - f;
            ren.material.color = c;
            yield return new WaitForSeconds(time);
        }
    }
}
