using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour {
    private bool cursorVisibility;

	void Update () {
        toggleCursor();		
	}

    public void toggleCursor() {
        //getkey to toggle cursor
        if (Input.GetKeyDown(KeyCode.F12)) cursorVisibility = !cursorVisibility;

        //toggle cursor
        if (!cursorVisibility) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
