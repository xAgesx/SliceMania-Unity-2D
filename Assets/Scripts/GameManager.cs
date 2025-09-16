using System.Collections.Generic;
using TMPro;
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
    public int deaths;

    void Awake() {
        HP = 3;
        timer.gameObject.SetActive(false);
        highScore = PlayerPrefs.GetInt("highScore");
        highScoreText.text = Abbreviate(highScore);
        EmptyHpIcon = Resources.Load<Sprite>("EmptyHp");

        //This is basically the volume setting , when the game restarts or player leaves and comes back the volume setting is always saved
        AudioListener.volume = PlayerPrefs.GetFloat("Volume", 1);
        //last minute fix so forgive the unclean code x)
        pauseMenu.transform.GetChild(1).transform.GetChild(1).transform.GetChild(0).gameObject.SetActive((AudioListener.volume == 0) ? true : false);

        
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

        for (int i = 2; i >= HP ; i--) {
            HpIcons[i].sprite = EmptyHpIcon;
        }
        if (HP <= 0) {
            //Save the score before timeScale is set to 0 to avoid any conflicts
            PlayerPrefs.SetInt("Score", score);
            PlayerPrefs.Save();
            continueTransitionMenu();
            HP = 0;

            //Set a number a resets allowed for ads : 1 for now 
            //this is the number of timer the player already died , if it's 0 he can watch an ad otherwise he can't (2 consecutive deaths)
            PlayerPrefs.SetInt("Deaths", deaths);
            PlayerPrefs.Save();
            
            
        }
        if (timerScript.timerFloat <= 0) {
            resetScene();
            PlayerPrefs.SetInt("Deaths", 0);
            PlayerPrefs.Save();
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
        PlayerPrefs.SetFloat("Volume", AudioListener.volume);
        PlayerPrefs.Save();
        
        //Toggle sound indicator
        pauseMenu.transform.GetChild(1).transform.GetChild(1).transform.GetChild(0).gameObject.SetActive((AudioListener.volume == 0) ? true : false);
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
