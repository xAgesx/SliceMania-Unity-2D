
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour {

    [SerializeField]private int timerValue;
    [SerializeField] private float timerCurrentValue; 
    [SerializeField] private TextMeshProUGUI timerTxt;
    [SerializeField] private Image TimerImage;

    public int timerInt;
    public float timerFloat;

    IEnumerator coroutine;

    void Start() {
        timerInt = timerValue;
        timerFloat = (float)timerValue;
        timerTxt.text = timerValue.ToString();

        coroutine = countdown();
        startCountdown();
    }
    public void startCountdown() {
        StartCoroutine(coroutine);
        
    }
    public void stopCountdown() {
        StopCoroutine(coroutine);
        
    }

    IEnumerator countdown() {
        while (timerFloat > 0) {
            timerFloat -= Time.deltaTime;
            TimerImage.fillAmount = timerFloat / timerValue;

            if (timerInt > (int)timerFloat) {
                timerTxt.text = timerInt.ToString();
                timerInt--;
            }
            yield return null;
        }
        timerTxt.text = "0";
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.Save();
        stopCountdown();
        
    }
    void Update() {
        countdown();
    }
}
