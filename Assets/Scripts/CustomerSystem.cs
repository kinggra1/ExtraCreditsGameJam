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

    private enum QuestType { BASIC_FIGHT, ADVANCED_FIGHT, FOOD }
    private Dictionary<QuestType, Quest> questTypeDict = new Dictionary<QuestType, Quest>();

    private void Awake() {
        if (instance)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        initializeQuestTypes();
        initializeCustomerTypes();   
    }
    
    private void initializeQuestTypes()
    {
        string basicFightDialogue = "I'm going out to fight some Level 1 Slimes.";
        InventorySystem.SellableItem[] basicFightItems = new InventorySystem.SellableItem[] {
            InventorySystem.SellableItem.WOODEN_SHIELD,
            InventorySystem.SellableItem.WOODEN_SWORD
        };
        Quest basicFightQuest = new Quest(basicFightDialogue, basicFightItems);
        questTypeDict.Add(QuestType.BASIC_FIGHT, basicFightQuest);

        string advancedFightDialogue = "I'm fighting a dragon today. Wish me luck!";
        InventorySystem.SellableItem[] advancedFightItems = new InventorySystem.SellableItem[] {
            InventorySystem.SellableItem.IRON_SHIELD,
            InventorySystem.SellableItem.IRON_SWORD
        };
        Quest advancedFightQuest = new Quest(advancedFightDialogue, advancedFightItems);
        questTypeDict.Add(QuestType.ADVANCED_FIGHT, advancedFightQuest);

        string foodDialogue = "I'm so hungry. Got anything to eat around here?";
        InventorySystem.SellableItem[] foodItems = new InventorySystem.SellableItem[] {
            InventorySystem.SellableItem.BREAD
        };
        Quest foodQuest = new Quest(foodDialogue, foodItems);
        questTypeDict.Add(QuestType.FOOD, foodQuest);
    }

    private void initializeCustomerTypes()
    {
        // Novice
        InventorySystem.SellableItem[] noviceDesiredItems = new InventorySystem.SellableItem[] {
            InventorySystem.SellableItem.BREAD,
            InventorySystem.SellableItem.WOODEN_SHIELD,
            InventorySystem.SellableItem.WOODEN_SWORD
        };
        Quest[] noviceQuests = new Quest[]
        {
            questTypeDict[QuestType.BASIC_FIGHT],
            questTypeDict[QuestType.FOOD]
        };
        string[] noviceDialogue = new string[]
        {
            "It's my first day as an adventurer! I'm not scared at all... nope, not even a little.",
            "I tried beating up monsters with a stick, but that didn't work out so well. Guess I'll buy some equipment now!",
            "I'm going to be the best adventurer EVER. Right after I learn how to use a sword, that is."
        };
        Customer noviceCustomer = new Customer(noviceSprite, noviceDesiredItems, noviceQuests, noviceDialogue);
        customerTypeDict.Add(CustomerType.NOVICE, noviceCustomer);

        // Warrior
        InventorySystem.SellableItem[] warriorDesiredItems = new InventorySystem.SellableItem[]
        {
            InventorySystem.SellableItem.BREAD,
            InventorySystem.SellableItem.IRON_SHIELD,
            InventorySystem.SellableItem.IRON_SWORD
        };
        Quest[] warriorQuests = new Quest[]
        {
            questTypeDict[QuestType.ADVANCED_FIGHT],
            questTypeDict[QuestType.FOOD]
        };
        string[] warriorDialogue = new string[]
        {
            "One of these days, I'll steal a dragon's horde and retire on the gold. Maybe I'll run a shop like you! Har, har!",
            "About time I get some replacement gear. I've bashed in enough goblin heads that my old weapons are getting dents in 'em.",
            "Wooden swords? Pfft. I've used toothpicks stronger than those."
        };
        Customer warriorCustomer = new Customer(warriorSprite, warriorDesiredItems, warriorQuests, warriorDialogue);
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
    public Quest[] quests;
    public string[] dialogue;

    public Customer(Sprite sprite, InventorySystem.SellableItem[] desiredItems, Quest[] quests, string[] dialogue)
    {
        this.sprite = sprite;
        this.desiredItems = desiredItems;
        this.quests = quests;
        this.dialogue = dialogue;
    }
}

public class Quest
{
    public string description;
    public InventorySystem.SellableItem[] applicableItems;

    public Quest(string description, InventorySystem.SellableItem[] applicableItems)
    {
        this.description = description;
        this.applicableItems = applicableItems;
    }
}