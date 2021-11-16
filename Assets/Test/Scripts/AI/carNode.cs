using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carNode : MonoBehaviour{

    public carNode previousWaypont;
    public carNode nextWaypoint;

    public carNode link;


    public Vector3 getPosition(){
        Vector3 minBound = transform.position ;
        Vector3 MAXBound = transform.position ;
    
        return Vector3.Lerp(minBound,MAXBound,Random.Range(0,1));
    }    

}
