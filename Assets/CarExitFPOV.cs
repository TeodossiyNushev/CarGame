using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;

public class CarExitFPOV : MonoBehaviour
{
    public GameObject carToExit;
    public GameObject fakeCar;
    public GameObject chFPPOV;

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            fakeCar.gameObject.SetActive(true);
            carToExit.gameObject.SetActive(false);
            chFPPOV.gameObject.SetActive(true);
        }

    }
}
