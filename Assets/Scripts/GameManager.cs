using System.Collections.Generic;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public Slice sliceScript;
    public TimerScript timerScript;
    public int score;
    public int highScore;
    public int HP;
    [SerializeField] Canvas timer;
    public GameObject pauseMenu;
    public TextMeshProUGUI highScoreText;
    public Button pauseBtn;
    public Image[] HpIcons;
    public Sprite EmptyHpIcon;

    void Awake() {
        HP = 3;
        timer.gameObject.SetActive(false);
        highScore = PlayerPrefs.GetInt("highScore");
        highScoreText.text = Abbreviate(highScore);
        EmptyHpIcon = Resources.Load<Sprite>("EmptyHp");
    }

    // Update is called once per frame
    void Update() {
        score = getScore();
        if (score > highScore) {
            highScore = score;
            highScoreText.text = Abbreviate(highScore);
            PlayerPrefs.SetInt("highScore", highScore);
            PlayerPrefs.Save();
        }

        for (int i = 2; i >= HP; i--) {
            HpIcons[i].sprite = EmptyHpIcon;
        }
        if (HP <= 0) {
            //Save the score before timeScale is set to 0 to avoid any conflicts
            PlayerPrefs.SetInt("Score",score);
            PlayerPrefs.Save();
            continueTransitionMenu();
            HP = 0;
            
        }
        if (timerScript.timerFloat <= 0) {
            resetScene();
        }


    }
    public void resetScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void continueTransitionMenu() {

        //First Remove all The candies 
        destroyAllItems();

        //Stop the spawning script
        
        GetComponent<ItemSpawner>().CancelInvoke();
        GetComponent<ItemSpawner>().enabled = false;

        //then display the menu
        displayContinue();
        //disable the Pause btn menu to avoid conflict between the 2 menus
        pauseBtn.gameObject.SetActive(false);
    }
    void displayContinue() {
        timer.gameObject.SetActive(true);
        timerScript.enabled = true;


    }
    public void destroyAllItems() {
        GameObject[] allItems = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in allItems) {
            DestroyImmediate(item);
        }
    }

    int getScore() {
        return sliceScript.score;
    }

    //UI MANAGER
    public void EnablePause() {
        Time.timeScale = 0;
        pauseBtn.gameObject.SetActive(false);
        pauseMenu.SetActive(true);
        GetComponent<SwipeDetection>().enabled = false;

    }
    public void Resume() {
        Time.timeScale = 1;
        pauseBtn.gameObject.SetActive(true);
        pauseMenu.SetActive(false);
        GetComponent<SwipeDetection>().enabled = true;
    }
    public void backToMenu() {
        Application.Quit();
    }
    public void ToggleSound() {
        AudioListener.volume = (AudioListener.volume == 0) ? 1 : 0;

        //Toggle sound indicator
        pauseMenu.transform.GetChild(1).transform.GetChild(1).transform.GetChild(0).gameObject.SetActive((AudioListener.volume == 0) ? false : true);
    }
    public string Abbreviate(float number)
    {
        // Define the suffixes for different magnitudes.
        string[] suffixes = { "", "k", "m", "b", "t" };
        int magnitude = 0;
        float abbreviatedNumber = number;

        while (abbreviatedNumber >= 1000 && magnitude < suffixes.Length - 1)
        {
            magnitude++;
            abbreviatedNumber /= 1000;
        }

        string formattedNumber = abbreviatedNumber.ToString("0.#");

        return $"{formattedNumber}{suffixes[magnitude]}";
    }
}
