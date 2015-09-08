﻿using UnityEngine;
using System.Collections;

public class RadialMovementInput : RadialMovement {

    public float zoomSpeed;
    public float minFOV;
    public float maxFOV;
    public string horizontalButton;
    public string verticalButton;
    public bool inUse;
    public Camera cam;
    [Range(0,1)]
    public float deadZone = 0.2f;
    private float zoomVal;
    protected override void Update() {
        base.Update();
        if (inUse) {
            float i = Input.GetAxis(horizontalButton);
            /*
            if (Input.GetButton(leftButton)) i--;
            if (Input.GetButton(rightButton)) i++;
            */
            if (Mathf.Abs(i) > deadZone)
            {
                if (i > 0) { Move(false); }
                else if (i < 0) { Move(true); }
            }

            i = Input.GetAxis(verticalButton);
            /*
            if (Input.GetButton(zoomOutButton)) i--;
            if (Input.GetButton(zoomInButton)) i++;
            */

            if (Mathf.Abs(i) > deadZone)
            {
                if (i > 0) { Zoom(false); }
                else if (i < 0) { Zoom(true); }
            }
        }
        cam.enabled = inUse;
        cam.fieldOfView = Mathf.Lerp(minFOV, maxFOV, zoomVal);
    }

    public void Zoom(bool zoomOut) {
        zoomVal = Mathf.Clamp(zoomVal + ((zoomOut)?1:-1) * zoomSpeed * Time.deltaTime, 0, 1);

        
    }

}
