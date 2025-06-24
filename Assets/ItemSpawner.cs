using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemSpawner : MonoBehaviour {

    public Slice sliceScript;
    public List<GameObject> candies; 

    void Start() {
        candies = sliceScript.getCandyPrefabs();
        InvokeRepeating("spawnItem", 0, 2);
    }

    void spawnItem() {
        Debug.Log(candies.Count);
        GameObject itemToSpawn = candies[Random.Range(0, candies.Count )];
        
        Vector2 pos = new Vector2(Random.Range(-2,2),4);
        Instantiate(itemToSpawn, pos, Quaternion.identity);
    }

}
