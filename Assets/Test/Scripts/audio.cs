using UnityEngine;
using Random = UnityEngine.Random;

public class audio : MonoBehaviour{


    public AudioClip lowAccelClip;                                              
    public AudioClip lowDecelClip;                                              
    public AudioClip highAccelClip;                                             
    public AudioClip highDecelClip; 
    [Range(0,2)]public float turboVolume;  
    public AudioClip downShift;
    [Range(0,1)]public float downShiftVolume;  
    public AudioClip Turbo;   

    private float accFade = 0;
                                      
    [Range(0,1)]public float pitchMultiplier = 1f;                              
    private float acceleration;
    public float lowPitchMin = 1f;                                              
    public float lowPitchMax = 6f;                                              
    [Range(0,1)]public float highPitchMultiplier = 0.25f;                       
    private float maxRolloffDistance = 500;                                      
    private AudioSource m_LowAccel; 
    private AudioSource m_LowDecel; 
    private AudioSource m_HighAccel;
    private AudioSource m_HighDecel;
    private AudioSource m_Turbo;
    private AudioSource m_downShift;
    private bool m_StartedSound; 
    private carController m_CarController;
    private inputManager InputManager;
    
    private void StartSound(){
        m_CarController = GetComponent<carController>();
        InputManager = GetComponent<inputManager>();

        m_HighAccel = SetUpEngineAudioSource(highAccelClip);
        m_LowAccel = SetUpEngineAudioSource(lowAccelClip);
        m_LowDecel = SetUpEngineAudioSource(lowDecelClip);
        m_HighDecel = SetUpEngineAudioSource(highDecelClip);
        m_Turbo = SetUpEngineAudioSource(Turbo);
        m_downShift = SetUpEngineAudioSource(downShift);
        m_downShift.loop = false;
        m_downShift.volume = downShiftVolume;

        
        m_StartedSound = true;
    }
   
    private void StopSound()
    {
        foreach (var source in GetComponents<AudioSource>())
        {
            Destroy(source);
        }
        m_StartedSound = false;
    }

    private void FixedUpdate(){

        float camDist = (Camera.main.transform.position - transform.position).sqrMagnitude;
                    
        accFade = Mathf.Lerp(accFade,Mathf.Abs( acceleration ), 15 * Time.deltaTime );
        if (m_StartedSound && camDist > maxRolloffDistance*maxRolloffDistance)
        {
            StopSound();
        }
        if (!m_StartedSound && camDist < maxRolloffDistance*maxRolloffDistance)
        {
            StartSound();
        }
        if (m_StartedSound){
            if(m_CarController.totalPower > 0 && !m_CarController.engineLerp)
                acceleration = 1;
            else acceleration = 0;

            float pitch = ULerp(lowPitchMin, lowPitchMax, m_CarController.engineRPM / m_CarController.maxRPM);
            pitch = Mathf.Min(lowPitchMax, pitch);
            m_Turbo.pitch = 0.76f +( m_CarController.engineRPM /  m_CarController.maxRPM) /2;
            m_LowAccel.pitch = pitch*pitchMultiplier;
            m_LowDecel.pitch = pitch*pitchMultiplier;
            m_HighAccel.pitch = pitch*highPitchMultiplier*pitchMultiplier;
            m_HighDecel.pitch = pitch*highPitchMultiplier*pitchMultiplier;

            float decFade = 1 - accFade;
            float highFade = Mathf.InverseLerp(0.2f, 0.8f,  m_CarController.engineRPM / 10000);
            float lowFade = 1 - highFade;
            
            highFade = 1 - ((1 - highFade)*(1 - highFade));
            lowFade = 1 - ((1 - lowFade)*(1 - lowFade));
            //accFade = 1 - ((1 - accFade)*(1 - accFade));
            decFade = 1 - ((1 - decFade)*(1 - decFade));
            m_LowAccel.volume = lowFade*accFade;
            m_LowDecel.volume = lowFade*decFade;
            m_HighAccel.volume = highFade*accFade;
            m_HighDecel.volume = highFade*decFade;
            m_Turbo.volume = highFade * accFade  * turboVolume ;
                
            }

        }

    private AudioSource SetUpEngineAudioSource(AudioClip clip){
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = 0;
        source.spatialBlend = 1;
        source.loop = true;
        source.dopplerLevel = 0;
        source.time = Random.Range(0f, clip.length);
        source.Play();
        source.minDistance = 5;
        source.maxDistance = maxRolloffDistance;
        return source;
    }

    public void DownShift(){

        m_downShift.Play();

    }
   
    private  float ULerp(float from, float to, float value){
        return (1.0f - value)*from + value*to;
    }


}
