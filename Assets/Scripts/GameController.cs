using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController instance;

    public GameObject ResultsCanvas;
    public GameObject CustomerPrefab;

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
        
    }


    // Maybe some stuff like this would live in GameController?
    private static void SpawnNewCustomer() {

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
