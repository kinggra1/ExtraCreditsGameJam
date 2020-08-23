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
        // Novice
        InventorySystem.SellableItem[] noviceDesiredItems = new InventorySystem.SellableItem[] {
            InventorySystem.SellableItem.BREAD,
            InventorySystem.SellableItem.WOODEN_SHIELD,
            InventorySystem.SellableItem.WOODEN_SWORD
        };
        Customer noviceCustomer = new Customer(noviceSprite, noviceDesiredItems);
        customerTypeDict.Add(CustomerType.NOVICE, noviceCustomer);

        // Warrior
        InventorySystem.SellableItem[] warriorDesiredItems = new InventorySystem.SellableItem[]
        {
            InventorySystem.SellableItem.BREAD,
            InventorySystem.SellableItem.IRON_SHIELD,
            InventorySystem.SellableItem.IRON_SWORD
        };
        Customer warriorCustomer = new Customer(warriorSprite, warriorDesiredItems);
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
    public InventorySystem.SellableItem[] desiredItems;

    public Customer(Sprite sprite, InventorySystem.SellableItem[] desiredItems)
    {
        this.sprite = sprite;
        this.desiredItems = desiredItems;
    }
}