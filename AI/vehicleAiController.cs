using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vehicleAiController : MonoBehaviour{

    private carModifier modifier;
    private WheelCollider[] wheels;
    
    public float totalPower;
    public float vertical , horizontal ;

    private float radius = 8 , distance;
    public carNode currentNode;

    private Vector3 velocity ,Destination, lastPosition;

    void Start(){
        modifier = GetComponent<carModifier>();
        wheels = modifier.colliders;

    }

    void FixedUpdate(){
        try{
        checkDistance();
        steerVehicle();
        }
        catch{}
    
    }


    void checkDistance(){

            if(Vector3.Distance(transform.position , currentNode.transform.position) <= 3){
                reachedDestination();
            }

        
    }

        
    private void reachedDestination(){
        if(currentNode.nextWaypoint == null ){
            currentNode = currentNode.previousWaypont;
            return;
        }
        if(currentNode.previousWaypont == null ){
            currentNode = currentNode.nextWaypoint;
            return;
        }

        if(currentNode.link != null && Random.Range(0 , 100) <= 20)
            currentNode = currentNode.link;
        else
            currentNode = currentNode.nextWaypoint;
        
    



    }


    private void steerVehicle(){

        Vector3 relativeVector = transform.InverseTransformPoint(currentNode.transform.position);
        relativeVector /= relativeVector.magnitude;
        float newSteer = (relativeVector.x  / relativeVector.magnitude) * 2;
        horizontal = newSteer;
        foreach (var item in wheels){
            item.motorTorque = totalPower;
        }

        if (horizontal > 0 ) {
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * horizontal;
        } else if (horizontal < 0 ) {                                                          
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * horizontal;
        } else {
            wheels[0].steerAngle =0;
            wheels[1].steerAngle =0;
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if(currentNode != null)
        Gizmos.DrawSphere(currentNode.transform.position ,0.5f);
    }

}
