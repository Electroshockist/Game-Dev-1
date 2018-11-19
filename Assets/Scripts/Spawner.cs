using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject[] spawnableEntities;

	// Use this for initialization
	void Start () {
        Spawn(60);
	}

    void Spawn(float percentChance) {

        int greatestRandom = 0;
        float[] random = new float[spawnableEntities.Length];

        for (int i = 0; i < spawnableEntities.Length; i++) {

            //checks if object exists
            if (GameObject.Find(spawnableEntities[i].name)) {
                //assigns random value if not already
                random[i] = Random.value;
            }

        }

        for (int i = 0; i < spawnableEntities.Length; i++) {

            //checks if object exists
            if (GameObject.Find(spawnableEntities[i].name)) {

                for (int j = i; j < random.Length; j++) {

                    //checks range
                    if (random[j] < percentChance / 100) {

                        //checks greater random and if it is in the chance range
                        if (random[j] > random[i]) greatestRandom = j;
                        else greatestRandom = i;
                    }
                    //else Debug.Log("Value " + random[j] * 100 + "% greater than " + percentChance + "%. Ignoring.");
                }
            }
        }

        //checks range
        if (random[greatestRandom] < percentChance / 100) {
            //spawns object
            Instantiate(spawnableEntities[greatestRandom], transform.position, transform.rotation);
            //Debug.Log("Spawned object: " + spawnableEntities[greatestRandom]);
        }
    }
}
