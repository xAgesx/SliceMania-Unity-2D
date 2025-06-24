using System.Collections.Generic;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;

public class Slice : MonoBehaviour {

    //public string[] candy = { "GummyBear", "Lollipop", "ChocolateBar", "Jawbreaker", "Cotton Candy" };
    [SerializeField]
    private List<GameObject> candyPrefabs;
    private InputManager inputManager;
    public SwipeDetection swipeDetection;

    public void Awake() {
        inputManager = FindAnyObjectByType<InputManager>().GetComponent<InputManager>();
        
    }
    public List<GameObject> getCandyPrefabs() {
        return candyPrefabs;
    }
    void OnTriggerEnter2D(Collider2D other) {
        //Destroy any item including the last variant 'Lollipop'
        if (other.CompareTag("Item")) {
            GetComponent<CircleCollider2D>().isTrigger = false;
            Destroy(other.gameObject);
            //For every other item , spawn the next smaller children with a small offset up and to the sides
            if (other.gameObject.GetComponent<Item>().itemType != "Lollipop") {

                Vector2 pos = inputManager.primaryPosition();
                int index = candyPrefabs.FindIndex(candyPrefabs => candyPrefabs.GetComponent<Item>().itemType == other.gameObject.GetComponent<Item>().itemType);



                int xOffset = 150;
                for (int i = 0; i < 2; i++) {

                    var spawnedItem = Instantiate(candyPrefabs[index - 1], pos, Quaternion.Euler(0, 0, 0));
                    spawnedItem.GetComponent<Rigidbody2D>().AddTorque(60);
                    spawnedItem.GetComponent<Rigidbody2D>().AddForce(new Vector2(xOffset, 150));
                    Debug.Log(xOffset);
                    xOffset *= -1;
                }

            }
            
        }

        
    }



}
