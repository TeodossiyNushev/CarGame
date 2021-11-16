using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
//using UnityEditor.PackageManager;
using UnityEngine;

public class DistanceChecker : MonoBehaviour
{
    public GameObject RayCastShooter;
    public vehicleAiController VAIC;
    public Rigidbody rb;
    public float xVelocity;



    //public IEnumerator Wait()
    //{
    //    VAIC.totalPower = 120f;
    //    yield return new WaitForSecondsRealtime(2);
    //}

    void FixedUpdate()
    {
        LayerMask Car = LayerMask.GetMask("Car");
        LayerMask Redlight = LayerMask.GetMask("RedLight");
        LayerMask Pedestrian = LayerMask.GetMask("Pedestrian");
        RaycastHit hit;
        xVelocity = rb.velocity.magnitude;

      

        //spira kato doblizhi kola
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 10f, Car))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            VAIC.totalPower = (xVelocity >= 0.15f) ? Mathf.Lerp(0, -550, 1) : -0f;
        }
        else
        {
            //StartCoroutine(Wait());
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            VAIC.totalPower = 25f;
        }

        //spira na cherven svetofar
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 10f, Redlight))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            VAIC.totalPower = (xVelocity >= 0.15f) ? Mathf.Lerp(0, -550f, 1) : -0f;

        }
       

        //spira na peshehodec
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 10f, Pedestrian))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            VAIC.totalPower = (xVelocity >= 0.15f) ? Mathf.Lerp(0, -550f, 1) : 0f;
        }
        
    }
}
