using JetBrains.Annotations;
using Microsoft.Unity.VisualStudio.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CustomerController : MonoBehaviour {
    public GameObject unhappyPrompt;

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

    private bool leaving;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.localPosition = StartPos;
        this.animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool paused = TimeSystem.instance.GetPaused();
        if (!paused)
        {
            int currentTime = TimeSystem.instance.GetCurrentTime();
            CustomerParameters.Mood currentMood = GetMood();
            if (customer.hasBeenServed) {
                animator.SetBool("Idle", false);
                moveTowardsStartingPos();
            } else if (customer.mood.Equals(CustomerParameters.Mood.MAD)) {
                animator.SetBool("Idle", false);
                moveTowardsStartingPos();
            } else {
                // Stuff to do if the customer hasn't been helped yet.
                if (Vector3.Distance(this.gameObject.transform.localPosition, standingPos) > 10f) {
                    moveTowardsStandingPos();
                }
                else {
                    animator.SetBool("Idle", true);
                }

                if (currentMood == CustomerParameters.Mood.HAPPY && currentTime >= neutralTime) {
                    SetMood(CustomerParameters.Mood.NEUTRAL);
                }
                if (currentMood == CustomerParameters.Mood.NEUTRAL && currentTime >= irritatedTime) {
                    SetMood(CustomerParameters.Mood.IRRITATED);
                }
                if (currentMood == CustomerParameters.Mood.IRRITATED && currentTime >= angryTime) {
                    SetMood(CustomerParameters.Mood.MAD);
                }
            }
        }
    }

    public void CustomerClickedOn() {
        if (!this.leaving) {
            GameController.instance.ShowSellingMenuForCustomer(customer);
        }
    }

    public void SetCustomerData(Customer customer)
    {
        this.customer = customer;
        determineMoodTimes();
        chooseStandingPosition();
        // customer = CustomerSystem.instance.GetRandomCustomerFromDistribution();
        // UnityEngine.UI.Image image = this.gameObject.GetComponent<UnityEngine.UI.Image>();
        // image.sprite = customer.sprite;
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

    void moveTowardsStartingPos() {
        if (!leaving) {
            if (!customer.hasBeenServed) {
                unhappyPrompt.SetActive(true);
                LeanTween.scale(unhappyPrompt, Vector3.zero, 0.5f).setDelay(2f);
            }
            GameController.instance.HideSellingMenu();
            transform.localScale = new Vector3(-1f, 1f, 1f);
            leaving = true;
        }

        float step = speed * Time.deltaTime;
        this.gameObject.transform.localPosition = Vector3.MoveTowards(this.gameObject.transform.localPosition, StartPos, step);

        if (Vector3.Distance(transform.localPosition, StartPos) < 10f) {
            Destroy(this.gameObject);
        }
    }

    CustomerParameters.Mood GetMood()
    {
        return customer.mood;
    }

    void SetMood(CustomerParameters.Mood mood)
    {
        customer.mood = mood;
    }
}