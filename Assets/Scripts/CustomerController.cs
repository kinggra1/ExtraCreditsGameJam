using JetBrains.Annotations;
using Microsoft.Unity.VisualStudio.Editor;
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

    // place on the screen the customer walks to
    private Vector3 standingPos;

    private Customer customer;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.localPosition = StartPos;

        determineCustomerType();
        chooseStandingPosition();
    }

    // Update is called once per frame
    void Update()
    {
        bool paused = TimeSystem.instance.GetPaused();
        if (!paused && this.gameObject.transform.localPosition != standingPos)
        {
            moveTowardsStandingPos();
        }
    }

    void determineCustomerType()
    {
        customer = CustomerSystem.instance.GetRandomCustomerFromDistribution();
        UnityEngine.UI.Image image = this.gameObject.GetComponent<UnityEngine.UI.Image>();
        image.sprite = customer.sprite;
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
}