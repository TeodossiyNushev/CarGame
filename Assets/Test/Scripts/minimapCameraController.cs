using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimapCameraController : MonoBehaviour{
    // Start is called before the first frame update

    public GameObject player;

    public Vector3 temp;
    public Vector3 tmp;
    public Vector3 tmp1;

    void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player");
        temp = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = new Vector3(player.transform.position.x , temp.y , player.transform.position.z);
        
        transform.rotation = Quaternion.Euler(90, player.transform.eulerAngles.y,player.transform.eulerAngles.z);

    }
}
