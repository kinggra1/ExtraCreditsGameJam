using JetBrains.Annotations;
using Microsoft.Unity.VisualStudio.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CustomerController : MonoBehaviour
{
    // where to spawn in the next customer
    public int MinXCustomerPos;
    public int MaxXCustomerPos;
    public int MinYCustomerPos;
    public int MaxYCustomerPos;

    // speed that customer moves towards their standing position
    public float speed;

    // where customer enters the screen
    public Vector3 StartPos;

    // when the customer will get tired of waiting and leave
    public int MaxWaitTime;

    // place on the screen the customer walks to
    private Vector3 standingPos;

    private Customer customer;


    // times when customer's mood will change if not served
    private int neutralTime;
    private int irritatedTime;
    private int angryTime;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.localPosition = StartPos;

        determineCustomerType();
        determineMoodTimes();
        chooseStandingPosition();
    }

    // Update is called once per frame
    void Update()
    {
        bool paused = TimeSystem.instance.GetPaused();
        if (!paused)
        {
            int currentTime = TimeSystem.instance.GetCurrentTime();
            Customer.Mood currentMood = GetMood();
            if (this.gameObject.transform.localPosition != standingPos)
            {
                moveTowardsStandingPos();
            }
            if (currentMood == Customer.Mood.HAPPY && currentTime >= neutralTime)
            {
                SetMood(Customer.Mood.NEUTRAL);
            }
            if (currentMood == Customer.Mood.NEUTRAL && currentTime >= irritatedTime)
            {
                SetMood(Customer.Mood.IRRITATED);
            }
            if (currentMood == Customer.Mood.IRRITATED && currentTime >= angryTime)
            {
                SetMood(Customer.Mood.MAD);
            }
        }
    }

    void determineCustomerType()
    {
        customer = CustomerSystem.instance.GetRandomCustomerFromDistribution();
        UnityEngine.UI.Image image = this.gameObject.GetComponent<UnityEngine.UI.Image>();
        image.sprite = customer.sprite;
    }

    void determineMoodTimes()
    {
        int currentTime = TimeSystem.instance.GetCurrentTime();
        int increment = MaxWaitTime / 3;
        neutralTime = currentTime + increment;
        irritatedTime = currentTime + (2 * increment);
        angryTime = currentTime + MaxWaitTime;
    }

    void chooseStandingPosition()
    {
        System.Random rand = GameController.instance.Rand;
        int xPos = rand.Next(MinXCustomerPos, MaxXCustomerPos);
        int yPos = rand.Next(MinYCustomerPos, MaxYCustomerPos);
        standingPos = new Vector3(xPos, yPos, 0);
    }

    void moveTowardsStandingPos()
    {
        float step = speed * Time.deltaTime;
        this.gameObject.transform.localPosition = Vector3.MoveTowards(this.gameObject.transform.localPosition, standingPos, step);
    }

    Customer.Mood GetMood()
    {
        return customer.mood;
    }

    void SetMood(Customer.Mood mood)
    {
        Debug.Log(mood);
        customer.mood = mood;
    }
}