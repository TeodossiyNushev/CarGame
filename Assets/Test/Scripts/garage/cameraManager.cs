using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraManager : MonoBehaviour{

    public GameObject[] cameraWaypoints;
    public GameObject spawnPos;

    public int cameraPositionIdicator = 0;
    public float lerpSpeed = 1;

    void Start(){
        transform.position = cameraWaypoints[0].transform.position;
    }

    void FixedUpdate(){
        transform.position = Vector3.Lerp(transform.position , cameraWaypoints[cameraPositionIdicator].transform.position, Time.deltaTime * lerpSpeed );
        transform.LookAt(spawnPos.transform);
    }


}
