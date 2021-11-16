using UnityEngine;

public class carController : MonoBehaviour
{
    internal enum driveType{
        frontWheelDrive,
        rearWheelDrive,
        allWheelDrive,
        sixWheelDrive
    }
    [SerializeField]private driveType drive;

    public Material brakeLights;

    //other classes ->
    private carModifier modifier;
    private inputManager IM;
    [HideInInspector]public bool test; //engine sound boolean

    [Header("Variables")]
    [Range(1,5)]public float finalDrive = 3.4f;
    public float maxRPM , minRPM;
    public float[] gears;
    public AnimationCurve enginePower;
    public Animator wingAnimation;

    [HideInInspector]public int gearNum = 1;
    [HideInInspector]public bool playPauseSmoke = false,hasFinished;
    [HideInInspector]public float KPH;
    [HideInInspector]public float engineRPM;
    [HideInInspector]public bool reverse = false;
    [HideInInspector]public float nitrusValue , totalPower;
    [HideInInspector]public bool nitrusFlag =false;
    [HideInInspector]public float[] wheelSlip;


    [HideInInspector]public WheelCollider[] wheels = new WheelCollider[4];
    private GameObject centerOfMass;
    private Rigidbody rigidbody;
    private audio Audio;

	private WheelFrictionCurve  forwardFriction,sidewaysFriction;
    [HideInInspector]public float engineLerpValue, radius = 4,brakPower = 0, wheelsRPM ,driftFactor, lastValue ,horizontal , acceleration, vertical , velocity = 0 , downforce;
    public float DownForceValue = 10f;
    [HideInInspector]public bool flag=false , lightsFlag , grounded ,engineLerp = false;
    private float startForwardFriction , startSidewaysFriction ;
    private float gearChangeRate ;

    private void Start() {

        //if(SceneManager.GetActiveScene().name == "awakeScene")return;
        getObjects();
        
        

    }

    private void FixedUpdate() {

        //if(SceneManager.GetActiveScene().name == "awakeScene")return;


        horizontal =(IM.horizontal != 0 )? Mathf.SmoothDamp(horizontal, IM.horizontal , ref velocity , Time.deltaTime) : 0;
        vertical = IM.vertical;

        lastValue = engineRPM;

        addDownForce();
        //animateWheels();
        steerVehicle();
        calculateEnginePower();
        activateNitrus();
        driftCar();
        activateLights();

        //animateSpoiler();
    }

    private void activateLights() {
        if (IM.vertical < 0 || KPH <= 1) turnLightsOn();
        else turnLightsOff();
    }

    private void turnLightsOn(){
        if (lightsFlag) return;
        brakeLights.SetColor("_EmissionColor", new Color(255f,35f,35f) * 0.025f);
        lightsFlag = true;
        //lights.SetActive(true);
    }    

    private void turnLightsOff(){
        if (!lightsFlag) return;
        brakeLights.SetColor("_EmissionColor", Color.black);
        lightsFlag = false;
        //lights.SetActive(false);
    }

    private void calculateEnginePower(){
        lerpEngine();
        wheelRPM();

        acceleration = vertical > 0 ?  vertical : wheelsRPM <= 1 ? vertical : 0;

        if(engineRPM >= maxRPM){
            setEngineLerp(maxRPM - 1000);
        }
        if(!engineLerp){
            engineRPM = Mathf.Lerp(engineRPM,1000 + (Mathf.Abs(wheelsRPM) *  finalDrive *  (gears[gearNum])) ,  5 * Time.deltaTime);
            totalPower = (enginePower.Evaluate(engineRPM)  * finalDrive ) * gears[gearNum] * acceleration ;
        }
        

        moveVehicle();
        shifter();
    }

    private void wheelRPM(){
        float sum = 0;
        int R = 0;
        for (int i = 0; i < 4; i++)
        {
            sum += wheels[i].rpm;
            R++;
        }
        wheelsRPM = (R != 0) ? sum / R : 0;
 
        if(wheelsRPM < 0 && !reverse ){
            reverse = true;
            //if (gameObject.tag != "AI") manager.changeGear();
        }
        else if(wheelsRPM > 0 && reverse){
            reverse = false;
            //if (gameObject.tag != "AI") manager.changeGear();
        }
    }

    private void shifter(){

        if(!isGrounded())return;
            //automatic
        if(engineRPM > maxRPM && gearNum < gears.Length-1 && !reverse && Time.time >= gearChangeRate ){
            gearNum ++;
            Audio.DownShift();
            gearChangeRate = Time.time + 1f/0.5f ;
            //if(gameObject.tag != "AI") manager.changeGear();
        }
        if(engineRPM < minRPM && gearNum > 0){
            gearNum --;
            //if (gameObject.tag != "AI") manager.changeGear();
        }

    }
 
    public bool isGrounded(){
        if(wheels[0].isGrounded &&wheels[1].isGrounded &&wheels[2].isGrounded &&wheels[3].isGrounded )
            return true;
        else
            return false;
    }

    private void moveVehicle(){



        if (drive == driveType.allWheelDrive){
            for (int i = 0; i < wheels.Length; i++){
                wheels[i].motorTorque = totalPower / 4;
                wheels[i].brakeTorque = brakPower;
            }
        }else if(drive == driveType.rearWheelDrive){
            wheels[2].motorTorque = totalPower / 2;
            wheels[3].motorTorque = totalPower / 2;

            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].brakeTorque = brakPower;
            }
        }
        else if(drive == driveType.sixWheelDrive){
            for (int i = 0; i < wheels.Length; i++){
                wheels[i].motorTorque = totalPower / 6;
                wheels[i].brakeTorque = brakPower;
            }
        }
        
        else{
            wheels[0].motorTorque = totalPower / 2;
            wheels[1].motorTorque = totalPower / 2;

            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].brakeTorque = brakPower;
            }
        }

        KPH = rigidbody.velocity.magnitude * 3.6f;

        brakeVehicle();
    }

    private void brakeVehicle(){
        brakPower = (vertical == -1 && wheelsRPM > 1)? 1205 : 0 ;
        wheels[2].brakeTorque = wheels[3].brakeTorque = (IM.handbrake)? 100000 : 0f;

    }
  
    private void steerVehicle(){


        //acerman steering formula
		//steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * horizontalInput;

        if (horizontal > 0 ) {
				//rear tracks size is set to 1.5f       wheel base has been set to 2.55f
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * horizontal;
        } else if (horizontal < 0 ) {                                                          
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * horizontal;
			//transform.Rotate(Vector3.up * steerHelping);

        } else {
            wheels[0].steerAngle =0;
            wheels[1].steerAngle =0;
        }

    }
   
    private void getObjects(){

        IM = GetComponent<inputManager>();
        modifier = GetComponent<carModifier>();
        rigidbody = GetComponent<Rigidbody>();
        Audio = GetComponent<audio>();
        wheels = modifier.colliders;
        wheelSlip = new float[wheels.Length];
        centerOfMass = gameObject.transform.Find("mass").gameObject;
        rigidbody.centerOfMass = centerOfMass.transform.localPosition;   


    }

    private void addDownForce(){
        downforce = Mathf.Abs( DownForceValue * rigidbody.velocity.magnitude);
        rigidbody.AddForce(-transform.up * downforce );

    }

    private void driftCar(){

        WheelHit hit;
        for (int i = 2; i < wheels.Length ; i++){

            if(wheels[i].GetGroundHit(out hit)){
                

                radius = 4 + (-Mathf.Abs(hit.sidewaysSlip) * 2) + rigidbody.velocity.magnitude / 10 ;
                wheelSlip[i] = Mathf.Abs( hit.forwardSlip ) + Mathf.Abs(hit.sidewaysSlip) ;

                forwardFriction = wheels[i].forwardFriction;
                forwardFriction.stiffness = (IM.handbrake)? Mathf.SmoothDamp(forwardFriction.stiffness , .3f , ref velocity , Time.deltaTime ): 1;
                wheels[i].forwardFriction = forwardFriction;

                sidewaysFriction = wheels[i].sidewaysFriction;
                sidewaysFriction.stiffness = (IM.handbrake)? Mathf.SmoothDamp(sidewaysFriction.stiffness , .3f  , ref velocity , Time.deltaTime ): 1;
                wheels[i].sidewaysFriction = sidewaysFriction;
                
                grounded = true;
            }
            else grounded = false;

        }


    }

    public void activateNitrus(){
        if (!IM.boosting && nitrusValue <= 10 ){
            nitrusValue += Time.deltaTime / 2;
        }
        else{
            nitrusValue -= (nitrusValue <= 0 ) ? 0 : Time.deltaTime;
        }

        if (IM.boosting){
            //if (nitrusValue > 0) { 
                //CarEffects.startNitrusEmitter();
                rigidbody.AddForce(transform.forward * 10000);
            //} 
            //else CarEffects.stopNitrusEmitter();
        }
        //else CarEffects.stopNitrusEmitter();

    }
    
    private void setEngineLerp(float num){
        engineLerp = true;
        engineLerpValue = num;
    }

    public void lerpEngine(){
        if(engineLerp){
            totalPower = 0;
            engineRPM = Mathf.Lerp(engineRPM,engineLerpValue,20 * Time.deltaTime );
            engineLerp = engineRPM <= engineLerpValue + 100 ? false : true;
        }
    }   

    private string s , w ;
/*
    void OnGUI(){

        s = "";

        foreach (float item in wheelSlip){
            s +=  item.ToString("0.0") + " ";
        }
        w = "";

        foreach (var item in wheels){
            w += item.sidewaysFriction.stiffness.ToString("0.0") + " ";
            //s +=   item.ToString("0.0") + " ";
        }

        //GUI.Label(new Rect(10, 10, 100, 20),"value: " + Mathf.Abs(200 + DownForceValue * rigidbody.velocity.magnitude));
        GUI.Label(new Rect(20, 30, 200, 20),"radius: " + radius.ToString("0"));
        GUI.Label(new Rect(20, 40, 200, 20),"downforce: " + downforce.ToString("0"));
        GUI.Label(new Rect(20, 50, 200, 20),"contact: " + grounded);
        GUI.Label(new Rect(20, 60, 200, 20),"wheel sip: " + s);
        GUI.Label(new Rect(20, 70, 200, 20),"wheel grip: " + w);
        GUI.Label(new Rect(20, 80, 200, 20),"speed: " + KPH.ToString("0.0"));
        GUI.Label(new Rect(20, 90, 200, 20),"brake: " + brakPower.ToString("0.0"));
        GUI.Label(new Rect(20, 100, 200, 20),"wheels RPM: " + wheelsRPM.ToString("0.0"));
        GUI.Label(new Rect(20, 110, 200, 20),"acceleration: " + acceleration.ToString("0.0"));

    }
*/
}