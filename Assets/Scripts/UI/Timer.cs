using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [HideInInspector] public float currentTimeInSeconds;
    [SerializeField] private int durationInMinutes;
    [SerializeField] private Text currentTimeText;
    private bool isTimerOn;

    [Space]
    [Header("Events")]
    [SerializeField] private UnityEvent OnSceneSwitch;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        isTimerOn = true;
        currentTimeInSeconds = durationInMinutes * 60;
        if (OnSceneSwitch == null) {
            OnSceneSwitch = new UnityEvent();
        }
    }

    // Update is called once per frame
    void Update() {
        if (isTimerOn) {
            currentTimeInSeconds -= Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(currentTimeInSeconds);
            if (currentTimeInSeconds >= 0) {
                if (time.Seconds < 10) {
                    currentTimeText.text = "Time left: " + time.Minutes.ToString() + ":" + "0" + time.Seconds.ToString();
                } else {
                    currentTimeText.text = "Time left: " + time.Minutes.ToString() + ":" + time.Seconds.ToString();
                }
            } else {
                isTimerOn = false;
                currentTimeText.fontSize = 11; // to make it fit in the textbox
                currentTimeText.text = "Time is Over!";
                OnSceneSwitch.Invoke();
            } 
        }
    }
}
