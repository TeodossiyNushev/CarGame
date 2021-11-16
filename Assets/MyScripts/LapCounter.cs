
using UnityEngine;
using UnityEngine.UI;

public class LapCounter : MonoBehaviour
{
    public Text Obikolki;
    private int lap = 0;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Stana!");
            Obikolki.text = "Lap" + lap++;
        }
    }
}