using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;

public class RayCastFPPOV : MonoBehaviour
{
    public GameObject RayCastShooter;
    public Rigidbody rb;

    public GameObject carToEntre;
    public GameObject fakeCar;
    public GameObject chFPPOV;

    // Update is called once per frame
    void FixedUpdate()
    {
        LayerMask CarToEnter = LayerMask.GetMask("CarToEnter");
        RaycastHit hit;
        

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 10f, CarToEnter))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            if (Input.GetKeyDown(KeyCode.F))
            {
                fakeCar.gameObject.SetActive(false);
                carToEntre.gameObject.SetActive(true); 
                chFPPOV.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            
        }

    }
}
