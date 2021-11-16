using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightsSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
   

    public GameObject redLight;
    public GameObject yellowLight;
    public GameObject greenLight;
    public GameObject redLightStopper;

    void Start()
    {
        redLight.gameObject.SetActive(true);
        redLightStopper.gameObject.SetActive(true);
        yellowLight.gameObject.SetActive(false);
        greenLight.gameObject.SetActive(false);
        StartCoroutine(TrafficLightSystem());
    }

    // Update is called once per frame
    // void Update()
    //{
        
    //}

    IEnumerator TrafficLightSystem()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(10);

            redLight.gameObject.SetActive(false);
            yellowLight.gameObject.SetActive(true);

            yield return new WaitForSecondsRealtime(2);

            yellowLight.gameObject.SetActive(false);
            greenLight.gameObject.SetActive(true);

            redLightStopper.gameObject.SetActive(false);

            yield return new  WaitForSecondsRealtime(20);

            greenLight.gameObject.SetActive(false);
            yellowLight.gameObject.SetActive(true);

            redLightStopper.gameObject.SetActive(true);

            yield return new WaitForSecondsRealtime(3);

            yellowLight.gameObject.SetActive(false);
            redLight.gameObject.SetActive(true);



        }
    }
}
