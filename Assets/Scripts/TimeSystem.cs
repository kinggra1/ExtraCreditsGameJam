using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSystem : MonoBehaviour
{
    public static TimeSystem instance;

    public int SecondsPerDay = 360;
    public Text timeDisplay;

    private int currentDay = 0;
    private float currentTime = 0;
    private bool paused = false;

    private void Awake() {
        if (instance) {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            currentTime += Time.deltaTime;
            UpdateTimeDisplay();
            if (currentTime > SecondsPerDay)
            {
                GameController.instance.ShowDailyResults();
            }
        }
    }

    void UpdateTimeDisplay()
    {
        string timeString = $"Day {currentDay}, Time: {(int)currentTime}";
        timeDisplay.text = timeString;
    }

    public void IncrementDay()
    {
        currentDay += 1;
        currentTime = 0;
    }

    public void SetPaused(bool isPaused)
    {
        paused = isPaused;
    }

    public int GetCurrentTime()
    {
        return (int)currentTime;
    }

    public bool IsPastClosingTime()
    {
        return currentTime > SecondsPerDay;
    }
}
