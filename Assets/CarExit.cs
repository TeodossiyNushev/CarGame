using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;

public class CarExit : MonoBehaviour
{
    // Start is called before the first frame update
    //public GameObject RayCastShooter;
    //public Rigidbody rb;

    public GameObject carToExit;
    public GameObject fakeCar;
    public GameObject chFPPOV;
    public vehicleAiController VAIC;



    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            fakeCar.gameObject.SetActive(true);
            chFPPOV.gameObject.SetActive(true);
            carToExit.gameObject.SetActive(false);

        }
    }


}
