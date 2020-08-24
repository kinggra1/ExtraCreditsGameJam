using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public static GameController instance;

    private static readonly int DEFAULT_TIME_TO_FIRST_CUSTOMER = 5;

    public Image menuBackgroundBlurImage;
    public GameObject gameIntroCanvas;
    public GameObject ResultsCanvas;
    public GameObject questBoardCanvas;
    public GameObject forgeCanvas;
    public GameObject sellingMenuCanvas;
    public GameObject winningCanvas;
    public GameObject losingCanvas;

    public GameObject CustomerPrefab;
    
    // time between customers will be randomly selected between these
    public int MinTimeBetweenCustomers;
    public int MaxTimeBetweenCustomers;

    // when the next customer will be spawned
    private int timeToSpawnNewCustomer = DEFAULT_TIME_TO_FIRST_CUSTOMER;

    public System.Random Rand;

    private void Awake() {
        if (instance) {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start() {
        Rand = new System.Random();

        // Special subcanvas that we delete on button press.
        gameIntroCanvas.SetActive(true);
        TimeSystem.instance.SetPaused(true);

        questBoardCanvas.SetActive(true);
        forgeCanvas.SetActive(true);
        ResultsCanvas.SetActive(true);
        sellingMenuCanvas.SetActive(true);
        winningCanvas.SetActive(true);
        losingCanvas.SetActive(true);
        CollapseMenusInstantly();
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
        AudioController.instance.PlayCustomerArrivedBell();
        GameObject newCustomer = CustomerSystem.instance.CreateNewCustomer();
        
        // set next customer spawn time
        int timeUntilNextCustomer = Rand.Next(MinTimeBetweenCustomers, MaxTimeBetweenCustomers);
        timeToSpawnNewCustomer += timeUntilNextCustomer;
    }

    private void CollapseMenusInstantly() {
        questBoardCanvas.transform.localScale = Vector3.zero;
        forgeCanvas.transform.localScale = Vector3.zero;
        ResultsCanvas.transform.localScale = Vector3.zero;
        sellingMenuCanvas.transform.localScale = Vector3.zero;
        winningCanvas.transform.localScale = Vector3.zero;
        losingCanvas.transform.localScale = Vector3.zero;
    }

    private void DestroyOtherMenus() {
        Destroy(questBoardCanvas.gameObject);
        Destroy(forgeCanvas.gameObject);
        Destroy(ResultsCanvas.gameObject);
        Destroy(sellingMenuCanvas.gameObject);
        Destroy(winningCanvas.gameObject);
    }

    public void ShowDailyResults() {
        foreach (CustomerController customer in GameObject.FindObjectsOfType<CustomerController>()) {
            Destroy(customer.gameObject);
        }
        CollapseMenusInstantly();
        TimeSystem.instance.SetPaused(true);
        LerpInCanvas(ResultsCanvas);
        ResultsCanvas.GetComponent<ResultsCanvasUIHandler>().ShowToday();
    }

    public void HideDailyResults()
    {
        LerpOutCanvas(ResultsCanvas);
    }

    public void ShowQuestBoard() {
        LerpInCanvas(questBoardCanvas);
    }

    public void HideQuestBoard() {
        LerpOutCanvas(questBoardCanvas);
    }

    public void ShowForge() {
        LerpInCanvas(forgeCanvas);
    }

    public void HideForge() {
        LerpOutCanvas(forgeCanvas);
    }

    public void ShowSellingMenuForCustomer(Customer customer) {
        // Set the canvas pivot so that it springs forth from the customer. Need to normalize to 0-1 though... TODO
        // sellingMenuCanvas.GetComponent<RectTransform>().pivot = customer.transform.position;
        LerpInCanvas(sellingMenuCanvas);
        sellingMenuCanvas.GetComponent<SellableInventoryUIHandler>().SetCustomerData(customer);
    }

    public void HideSellingMenu() {
        LerpOutCanvas(sellingMenuCanvas);
    }

    private void LerpInCanvas(GameObject canvas) {
        LeanTween.scale(canvas, Vector3.one, 0.15f);
        LeanTween.alpha(menuBackgroundBlurImage.rectTransform, 0.6f, 0.15f);
    }

    private void LerpOutCanvas(GameObject canvas) {
        LeanTween.scale(canvas, Vector3.zero, 0.2f);
        LeanTween.alpha(menuBackgroundBlurImage.rectTransform, 0f, 0.2f);
    }

    public void WinGame() {
        foreach (CustomerController customer in GameObject.FindObjectsOfType<CustomerController>()) {
            Destroy(customer.gameObject);
        }
        CollapseMenusInstantly();
        TimeSystem.instance.SetPaused(true);
        LerpInCanvas(winningCanvas);
    }

    public void LoseGame() {
        foreach (CustomerController customer in GameObject.FindObjectsOfType<CustomerController>()) {
            Destroy(customer.gameObject);
        }
        DestroyOtherMenus();
        TimeSystem.instance.SetPaused(true);
        LerpInCanvas(losingCanvas);
    }

    public void StartGame() {
        AudioController.instance.PlayCustomerArrivedBell();
        TimeSystem.instance.SetPaused(false);
        Destroy(gameIntroCanvas.gameObject);
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToNextDay()
    {
        TimeSystem.instance.IncrementDay();
        StatsController.instance.ResetDailyCounts();
        HideDailyResults();
        timeToSpawnNewCustomer = DEFAULT_TIME_TO_FIRST_CUSTOMER;
        TimeSystem.instance.SetPaused(false);
    }
}
