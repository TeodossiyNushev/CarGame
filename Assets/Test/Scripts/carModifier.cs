using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carModifier : MonoBehaviour{
    
    internal enum type{
        player,
        AI,

    }
    [SerializeField]private type playerType;



    [Header("wheels")]
    [Range(0.2f,0.7f)]public float wheelRadius = 0.36f;
    [Range(0.05f,0.2f)]public float suspensionDistance = 0.1f;
    [Range(0,0.1f)]public float suspensionoffset = 0.03f;
    [Range(.4f,1)]public float sidewaysFriction ;
    [Range(.5f,1)]public float forwardFriction ;

    private carController controller;
    private GameObject wheelsFolder;
    private GameObject[] wheels;
    [HideInInspector]public WheelCollider[] colliders;

    public ParticleSystem smoke;
    [HideInInspector]public ParticleSystem[] smokeArray;

    public AudioClip skid;
    [Range(0,1)]public float skidVolume = 0.6f;
    [HideInInspector]public AudioSource[] skidArr;

    public TrailRenderer tireMarks;
    [HideInInspector]public TrailRenderer[] tireMarksarr;

    private Vector3 wheelPosition;
	private Quaternion wheelRotation ;



    void Awake(){
        controller = gameObject.GetComponent<carController>();
        wheelsFolder = gameObject.transform.Find("wheels").gameObject;   

        wheels = new GameObject[wheelsFolder.transform.childCount-1];
        colliders = new WheelCollider[wheels.Length];

        for (int i = 0; i < wheels.Length; i++){
            wheels[i] = wheelsFolder.transform.GetChild(i+1).gameObject;
        }

        GameObject wheelObject = wheelsFolder.transform.GetChild(0).gameObject;
        spawnWheelColliders();
        spawnWheels(wheelObject);
        if(playerType == type.player)
            spawnSmoke();

    }

    void FixedUpdate(){
        if(playerType == type.player)
            activateSmoke();
        animateWheels();   

    }
   
    private void activateSmoke(){

        for (int i = 0; i < wheels.Length; i++){
            if(controller.wheelSlip[i] > 0.70f  && controller.isGrounded()) {
                if(!skidArr[i].isPlaying)
                    smokeArray[i].Play();
                    skidArr[i].Play();
                    tireMarksarr[i].emitting = true;
            }
            else{
                smokeArray[i].Stop();
                skidArr[i].Stop();
                tireMarksarr[i].emitting = false;
            }
        }


    }

    public void spawnWheels(GameObject wheel ){
        for (int i = 0; i < wheels.Length; i++){
            GameObject newWheel = Instantiate(wheel);
            newWheel.transform.parent = wheels[i].transform ;
            newWheel.transform.position = wheels[i].transform.position;
            newWheel.transform.localScale = new Vector3(1,1,1);
            newWheel.transform.localRotation = new Quaternion(0,0,0,0);
            if(i % 2 != 0)
            newWheel.transform.Rotate(new Vector3(0,180,0));
        }
        wheel.SetActive(false);
    }

    public void spawnWheelColliders(){

        GameObject wheelColliderFolder = new GameObject();
        wheelColliderFolder.transform.parent = gameObject.transform;
        wheelColliderFolder.name = "wheelCollidersFolder";

        for (int i = 0; i < wheels.Length; i++){
            GameObject wheel = new GameObject();
            wheel.name = "wheel_" + i;
            wheel.transform.parent = wheelColliderFolder.transform;
            wheel.transform.position = wheels[i].transform.position;
            setupWheelColldier(wheel, i);
        }
        
    }

    private void spawnSmoke(){
        smokeArray = new ParticleSystem[wheels.Length];
        tireMarksarr = new TrailRenderer[wheels.Length];
        skidArr = new AudioSource[wheels.Length];
        GameObject wheelColliderFolder = new GameObject();
        wheelColliderFolder.transform.parent = gameObject.transform;
        wheelColliderFolder.name = "smoke";

        for (int i = 0; i < wheels.Length; i++){
            GameObject wheel = new GameObject();
            wheel.name = "wheel_" + i;
            wheel.transform.parent = wheelColliderFolder.transform;
            wheel.transform.position = new Vector3(colliders[i].transform.position.x , colliders[i].transform.position.y - colliders[i].radius / 1.7f ,colliders[i].transform.position.z);
            setupWheelSmoke(wheel, i);
        }
    }
    
    private void setupWheelSmoke(GameObject element , int i){
        AudioSource skidAudio = element.AddComponent<AudioSource>();
        skidArr[i] = skidAudio;

        skidAudio.clip = skid;
        skidAudio.loop = true;
        skidAudio.volume = skidVolume ;
        skidAudio.Stop();
        skidAudio.dopplerLevel = 0;
        skidAudio.reverbZoneMix = 0;
        skidAudio.spatialBlend = 1;
        skidAudio.maxDistance = 20;

        ParticleSystem partices = Instantiate(smoke);

        partices.transform.parent = element.transform ;
        partices.transform.position = element.transform.position;
        partices.transform.localScale = new Vector3(1,1,1);
        partices.transform.localRotation = new Quaternion(0,0,0,0);

        smokeArray[i] = partices;
        
        TrailRenderer trail = Instantiate(tireMarks);

        trail.emitting = false;

        trail.transform.parent = element.transform ;
        trail.transform.position = element.transform.position;
        trail.transform.localScale = new Vector3(1,1,1);
        trail.transform.localRotation = new Quaternion(0,0,0,0);

        tireMarksarr[i] = trail.GetComponent<TrailRenderer>();

    }

    private void animateWheels (){

		for (int i = 0; i < wheels.Length; i++) {
			colliders[i].GetWorldPose(out wheelPosition, out wheelRotation);
            wheels[i].transform.rotation = wheelRotation;
            wheels[i].transform.position = wheelPosition;

        }
    }

    public void setupWheelColldier(GameObject element , int i){
        WheelCollider collider = element.AddComponent<WheelCollider>();
        WheelFrictionCurve curve = collider.forwardFriction;
        JointSpring s = collider.suspensionSpring;

        colliders[i] = collider;

        curve.asymptoteValue = forwardFriction;
        collider.forwardFriction = curve;
        curve.asymptoteValue = sidewaysFriction;
        collider.sidewaysFriction = curve;

        curve.extremumSlip = 0.15f;
        collider.sidewaysFriction = curve;

        s.spring = 75000;
        collider.suspensionSpring = s;

        collider.radius = wheelRadius;
        collider.suspensionDistance = suspensionDistance;
        collider.forceAppPointDistance = 0.05f;
        collider.center = new Vector3(0,suspensionoffset,0);

    }
    
}
