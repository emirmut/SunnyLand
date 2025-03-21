using System;
using UnityEngine;
using UnityEngine.UI;

public class Chronometer : MonoBehaviour
{
    private float currentTimeInSeconds;
    [SerializeField] private Text currentTimeText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        currentTimeInSeconds = 0;
    }

    // Update is called once per frame
    void Update() {
        currentTimeInSeconds += Time.deltaTime;
        TimeSpan time = TimeSpan.FromSeconds(currentTimeInSeconds);
        if (time.Seconds < 10) {
            currentTimeText.text = "Time passed: " + time.Minutes + ":" + "0" + time.Seconds;
        } else {
            currentTimeText.text = "Time passed: "+ time.Minutes + ":" + time.Seconds;
        }
    }
}
