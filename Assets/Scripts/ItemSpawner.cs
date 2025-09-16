using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemSpawner : MonoBehaviour {

    public Slice sliceScript;
    public List<GameObject> candies;

    void Start() {
        candies = sliceScript.getCandyPrefabs();
        startSpawning();
    }

    public void startSpawning() {
        InvokeRepeating("spawnItem", 0, 3);
    }
    void spawnItem() {
        //Debug.Log(candies.Count);
        //Choosing a random candy based on it's rarety 
        int indexToSpawn = 0;
        int i = Random.Range(0, 100);
        switch (i) {
            case int n when (n<40) : indexToSpawn = 0;break;
            case int n when (n<70) : indexToSpawn = 1;break;
            case int n when (n<90) : indexToSpawn = 2;break;
            case int n when (n<=100) : indexToSpawn = 3;break;
        }
        Debug.Log(indexToSpawn);
        GameObject itemToSpawn = candies[indexToSpawn];

        Vector2 pos = new Vector2(Random.Range(-2, 2), 4);
        Instantiate(itemToSpawn, pos, Quaternion.identity);
    }
    void cancelInvoke() {
        CancelInvoke();
    }

}
