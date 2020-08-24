﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public static GameController instance;

    public Image menuBackgroundBlurImage;
    public GameObject CustomersCanvas;
    public GameObject ResultsCanvas;
    public GameObject questBoardCanvas;
    public GameObject forgeCanvas;
    public GameObject sellingMenuCanvas;

    public GameObject CustomerPrefab;
    
    // time between customers will be randomly selected between these
    public int MinTimeBetweenCustomers;
    public int MaxTimeBetweenCustomers;

    // when the next customer will be spawned
    private int timeToSpawnNewCustomer = 5;

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

        questBoardCanvas.SetActive(true);
        questBoardCanvas.transform.localScale = Vector3.zero;
        forgeCanvas.SetActive(true);
        forgeCanvas.transform.localScale = Vector3.zero;
        ResultsCanvas.SetActive(true);
        ResultsCanvas.transform.localScale = Vector3.zero;
        sellingMenuCanvas.SetActive(true);
        sellingMenuCanvas.transform.localScale = Vector3.zero;
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
        GameObject newCustomer = Instantiate(CustomerPrefab, CustomersCanvas.transform);
        
        // set next customer spawn time
        int timeUntilNextCustomer = Rand.Next(MinTimeBetweenCustomers, MaxTimeBetweenCustomers);
        timeToSpawnNewCustomer += timeUntilNextCustomer;
    }

    public void ShowDailyResults()
    {
        TimeSystem.instance.SetPaused(true);
        LerpInCanvas(ResultsCanvas);
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

    public void GoToNextDay()
    {
        TimeSystem.instance.IncrementDay();
        HideDailyResults();
        TimeSystem.instance.SetPaused(false);
    }
}
