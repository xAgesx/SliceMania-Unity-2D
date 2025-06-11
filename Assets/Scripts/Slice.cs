using System.Collections.Generic;
using UnityEngine;

public class Slice : MonoBehaviour {

    public string[] candy = { "GummyBear", "Lollipop", "ChocolateBar", "Jawbreaker", "Cotton Candy" };
    public List<GameObject> candyPrefabs;
    private InputManager inputManager;

    public void Awake() {
        inputManager = FindAnyObjectByType<InputManager>().GetComponent<InputManager>();   
    }
    void OnTriggerEnter2D(Collider2D other) {
        if (!other.gameObject.CompareTag("Base")) {
            Destroy(other.gameObject);
            Vector2 pos = inputManager.primaryPosition();
            int index = candyPrefabs.IndexOf(other.gameObject)-1;
            for (int i = 0; i < 2; i++) {
                Instantiate(candyPrefabs[0], pos, Quaternion.Euler(0, 0, 0));
            }

        } else {
            Debug.Log("base candy");
        }
    }



}
