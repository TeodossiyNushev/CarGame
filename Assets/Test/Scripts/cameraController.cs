using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    private GameObject atachedVehicle;
    private GameObject CameraFolder;
    private int locationIndicator = 0 ;
    private carController controllerRef;

    private float bias ;
    [Range(0,1)]public float smoothTime = .5f;
    public float smoothTimeFactor = 100;
    public float smoothTimemin = .5f , max;
    Vector3 newPos;
    public Vector2[] cameraPos;
    public float  dist , height,right, lookUp;
    private Transform target;


    void Start(){
        //cameraPos = new Vector2[3];
        //cameraPos[0] = new Vector2(1,1);
        //cameraPos[1] = new Vector2(dist,height);
        //cameraPos[2] = new Vector2(dist,height);

        atachedVehicle = GameObject.FindGameObjectWithTag("Player");
        CameraFolder = atachedVehicle.transform.Find("CAMERA").gameObject;

        controllerRef = atachedVehicle.GetComponent<carController>();
        target = CameraFolder.transform;


    }

    void FixedUpdate(){
        if(Input.GetKeyDown(KeyCode.Tab)){
            if(locationIndicator >= cameraPos.Length-1 || locationIndicator < 0) locationIndicator = 0;
            else locationIndicator ++;
        }    


		newPos = target.position - (target.forward * cameraPos[locationIndicator].x) + (target.up * cameraPos[locationIndicator].y) ;
		transform.position = newPos * (1 - smoothTime) + transform.position * smoothTime;
        transform.LookAt(target.transform);



        //transform.position = camLocations[locationIndicator].position * (1- smoothTime) + transform.position * smoothTime;
        //transform.LookAt(camLocations[1].transform);

        //smoothTime = (controllerRef.KPH < 100) ? Mathf.Abs((controllerRef.KPH)/ 200 - 0.9f) :smoothTime;
        bias = max - controllerRef.KPH  / smoothTimeFactor  ;
        smoothTime =( bias >=smoothTimemin)? bias :smoothTimemin;

    }
}
