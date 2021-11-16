using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class carsoundScript : MonoBehaviour
{
    public float revs;
    public Text revsText;
    public Text gearsText;
    public Text kphText;
    public AudioSource revsSound;
    public AudioSource idleSound;
    public AudioSource wineSound;
    public AudioSource flutterSound;
    public float secs;
    public int gear;
    public float revsMultipler;
    public float revsD;
    public float kphMultipler;
    public float kph;
    public float kphD;
    public int maxGear;
    public float maxRevs;
    public float maxKph;
    public float engineVol;
    public float idleVol;
    public float wineVol;

    public carController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<carController>();
        revsMultipler = 3000;
        gear = 1;
        kphMultipler = 0f;
        kphD = 9;
        revsD = .75f;
    }

    // Update is called once per frame
    void Update()
    {
        secs += 1 * Time.deltaTime;
        //revsText.text = "RPM: " + revs.ToString("F0");
        //gearsText.text = "GEAR: " + gear.ToString("F0");
        //kphText.text = "KPH: " + kph.ToString("F0");
        revsSound.pitch = revs / 7400f + (gear / 50f);
        if(secs < 5)
        {   
            revs = controller.engineRPM;
            kphMultipler += 1 * Time.deltaTime * revs / kphD / gear;
        }
        if(secs > 5)
        {
            
            if (revs < maxRevs + 24)
            {
                revs = controller.engineRPM;
            }     
            if (kph < maxKph)
            {
                kph += 1 * Time.deltaTime * kphMultipler;
            }
        }
        if(revs > maxRevs)
        {
            if(gear < maxGear)
            {
                flutterSound.Play();
                revs = controller.engineRPM;
                revsD += 0.02f;
                revsMultipler *= .5f;
                kphMultipler *= .7f;
                gear += 1;
                kphD += Random.Range(5, 8);
            }  
        }
        idleVol = Mathf.Clamp01(idleVol);
        engineVol = Mathf.Clamp01(engineVol);
        wineVol = Mathf.Clamp01(wineVol);

        engineVol = kph / 70;
        revsSound.volume = engineVol;
        wineVol = kph / 200;
        wineSound.volume = wineVol;
        wineSound.pitch = kph / 190;
    }
}
