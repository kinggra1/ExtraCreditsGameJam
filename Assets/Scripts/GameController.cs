using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController instance;

    public GameObject ResultsCanvas;
    public GameObject CustomerPrefab;

    private int timeBetweenCustomers = 5;
    private int timeToSpawnNewCustomer = 5;

    private void Awake() {
        if (instance) {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        int currentTime = TimeSystem.instance.GetCurrentTime();
        //if ()
    }

    public void SpawnNewCustomer()
    {
        Instantiate(CustomerPrefab, new Vector3(4, -2, 0), Quaternion.identity);
        // set next customer spawn time

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
