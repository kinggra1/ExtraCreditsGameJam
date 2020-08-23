using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSystem : MonoBehaviour
{
    public static CustomerSystem instance;

    public Sprite noviceSprite;
    public Sprite warriorSprite;

    public enum CustomerType { NOVICE, WARRIOR }

    private Dictionary<CustomerType, Customer> customerTypeDict = new Dictionary<CustomerType, Customer>();

    private void Awake() {
        if (instance)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        // Initialize customer types
        Customer noviceCustomer = new Customer(noviceSprite);
        customerTypeDict.Add(CustomerType.NOVICE, noviceCustomer);

        Customer warriorCustomer = new Customer(warriorSprite);
        customerTypeDict.Add(CustomerType.WARRIOR, warriorCustomer);
    }

    public Customer GetRandomCustomerFromDistribution()
    {
        System.Random rand = GameController.instance.Rand;
        int generatedNumber = rand.Next(0, 100);
        if (generatedNumber > 80)
        {
            return customerTypeDict[CustomerType.WARRIOR];
        }
        return customerTypeDict[CustomerType.NOVICE];
    }

}

public class Customer
{
    public Sprite sprite;

    public Customer(Sprite sprite)
    {
        this.sprite = sprite;
    }
}