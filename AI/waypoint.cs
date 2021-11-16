using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waypoint : MonoBehaviour{

    public waypoint previousWaypont;
    public waypoint nextWaypoint;

    [Range(0,10)]public float width = 1;


    public Vector3 getPosition(){
        Vector3 minBound = transform.position + transform.right * width / 2;
        Vector3 MAXBound = transform.position - transform.right * width / 2;
    
        return Vector3.Lerp(minBound,MAXBound,Random.Range(0,1));
    }    

}
