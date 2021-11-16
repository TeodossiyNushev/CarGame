using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputManager : MonoBehaviour
{
   
    [HideInInspector] public float vertical;
    [HideInInspector] public float horizontal;
    [HideInInspector] public bool handbrake;
    [HideInInspector] public bool boosting;


    private void FixedUpdate () {
        
            keyboard ();


    }

    private void keyboard () {
        vertical = Input.GetAxis ("Vertical");
        horizontal = Input.GetAxis ("Horizontal");
        handbrake = (Input.GetAxis ("Jump") != 0) ? true : false;
        if (Input.GetKey (KeyCode.LeftShift)) boosting = true;
        else boosting = false;

    }


}
