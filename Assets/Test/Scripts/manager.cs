using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class manager : MonoBehaviour
{

    public carController controller;

    public GameObject neeedle ;
    public Text gears , kph;

    public float desiredPosition;

    public float startPosiziton = 32f, endPosition = -211f;


    void Start(){

        controller = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<carController>();        
    }

    // Update is called once per frame
    void Update()
    {
        updateNeedle();
    }
    public void updateNeedle () {
        desiredPosition = startPosiziton - endPosition;
        float temp = controller.engineRPM / 10000;
        neeedle.transform.eulerAngles = new Vector3 (0, 0, (startPosiziton - temp * desiredPosition));

        gears.text = (controller.gearNum + 1).ToString();
        kph.text = controller.KPH.ToString("0");

    }
}
