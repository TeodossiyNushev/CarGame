using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pedestrianSpawner : MonoBehaviour{




    public waypoint[] waypoints;


    public GameObject[] pedestrian;
    public int totalPopulationMax = 20;
    public float maxDistance = 100f;

    public List<GameObject> spawnedAi ;

    void Awake(){
        loadArray();
        StartCoroutine(managePedestrian());
        managePopulation();
    }

    void loadArray(){
        waypoints = FindObjectsOfType<waypoint>();

    }

    void spawnPrefab(int index){     

        GameObject i = Instantiate(pedestrian[Random.Range(0,pedestrian.Length)] );
        i.GetComponent<ThirdPersonUserControl>().currentWaypoint = waypoints[index].GetComponent<waypoint>();
        i.transform.position = waypoints[index].transform.position;
        spawnedAi.Add(i);
        
    }

    public IEnumerator managePedestrian(){

		while(true){
			yield return new WaitForSeconds(20);
            managePopulation();
		}
	

    }

    void managePopulation(){
        for (int i = 0; i < waypoints.Length; i++){
            if(spawnedAi.Count <= totalPopulationMax)
                populationLoop(i);
        }

        for (int i = 0; i < spawnedAi.Count; i++){
            if(Vector3.Distance(transform.position , spawnedAi[i].transform.position) >= maxDistance){
                Destroy(spawnedAi[i]);
                spawnedAi.Remove(spawnedAi[i]);
            }
        }

    }

    void populationLoop(int index){
        if(Vector3.Distance(transform.position , waypoints[index].transform.position) <= maxDistance){
            spawnPrefab(index);
        }
    }


}
