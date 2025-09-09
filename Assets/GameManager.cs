using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public Slice sliceScript;
    public TimerScript timerScript;
    public int score;
    public int HP;
    [SerializeField] Canvas Timer;

    void Awake() {
        HP = 3;
        Timer.enabled = false;
    }

    // Update is called once per frame
    void Update() {
        score = getScore();

        if (HP <= 0) {

            continueTransitionMenu();
        }
        if (timerScript.timerFloat <= 0) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }


    }
    void continueTransitionMenu() {

        //First Remove all The candies 
        GameObject[] allItems = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in allItems) {
            DestroyImmediate(item);
        }

        //Stop the spawning script
        GetComponent<ItemSpawner>().enabled = false;
        GetComponent<ItemSpawner>().CancelInvoke();

        //then display the menu
        displayContinue();
    }
    void displayContinue() {
        Timer.enabled = true;
        timerScript.enabled = true;
        
    }

    int getScore() {
        return sliceScript.score;
    }
}
