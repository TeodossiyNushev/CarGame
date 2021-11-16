using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vehicleSpawner : MonoBehaviour{




    public carNode[] waypoints;


    public GameObject[] vehicle;
    public int totalPopulationMax = 20;
    public float maxDistance = 100f;
    public int totalAi = 0 ;
    public List<GameObject> spawnedAi ;

    void Awake(){
        loadArray();
        StartCoroutine(spawner());
        managePopulation();
    }

    void loadArray(){
        waypoints = FindObjectsOfType<carNode>();

    }

    void spawnPrefab(int index){     

        GameObject i = Instantiate(vehicle[Random.Range(0,vehicle.Length)] );
        i.GetComponent<vehicleAiController>().currentNode = waypoints[index].GetComponent<carNode>();
        i.transform.position = waypoints[index].transform.position;
        i.transform.Rotate(0 , waypoints[index].transform.eulerAngles.y , 0);
        spawnedAi.Add(i);
        
    }

    public IEnumerator spawner(){

		while(true){
			yield return new WaitForSeconds(20);
            managePopulation();
		}
	

    }

    void managePopulation(){
        for (int i = 0; i < waypoints.Length; i += 3){
            if(spawnedAi.Count <= totalPopulationMax)
                populationLoop(i);
        }

        for (int i = 0; i < spawnedAi.Count; i++){
            if(Vector3.Distance(transform.position , spawnedAi[i].transform.position) >= maxDistance){
                Destroy(spawnedAi[i]);
                spawnedAi.Remove(spawnedAi[i]);
            }
        }
        totalAi = spawnedAi.Count;

    }

    void populationLoop(int index){
        if(Vector3.Distance(transform.position , waypoints[index].transform.position) <= maxDistance){
            spawnPrefab(index);
        }
    }


}
