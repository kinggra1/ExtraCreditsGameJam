using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController instance;

    public GameObject ResultsCanvas;
    public GameObject CustomerPrefab;
    
    // time between customers will be randomly selected between these
    public int MinTimeBetweenCustomers;
    public int MaxTimeBetweenCustomers;

    // when the next customer will be spawned
    private int timeToSpawnNewCustomer = 5;

    private System.Random rand;

    private void Awake() {
        if (instance) {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start() {
        rand = new System.Random();
    }

    // Update is called once per frame
    void Update() {
        int currentTime = TimeSystem.instance.GetCurrentTime();
        bool isPastClosingTime = TimeSystem.instance.IsPastClosingTime();
        // don't spawn a new customer if the shop is past closing time
        if (!isPastClosingTime && currentTime >= timeToSpawnNewCustomer)
        {
            SpawnNewCustomer();
        }
    }

    public void SpawnNewCustomer()
    {
        Instantiate(CustomerPrefab, new Vector3(4, -2, 0), Quaternion.identity);
        // set next customer spawn time
        int timeUntilNextCustomer = rand.Next(MinTimeBetweenCustomers, MaxTimeBetweenCustomers);
        Debug.Log(timeUntilNextCustomer);
        timeToSpawnNewCustomer += timeUntilNextCustomer;
    }

    public void ShowDailyResults()
    {
        TimeSystem.instance.SetPaused(true);
        ResultsCanvas.SetActive(true);
    }

    public void HideDailyResults()
    {
        ResultsCanvas.SetActive(false);
    }

    public void GoToNextDay()
    {
        TimeSystem.instance.IncrementDay();
        HideDailyResults();
        TimeSystem.instance.SetPaused(false);
    }
}
