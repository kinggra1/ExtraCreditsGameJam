using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

public class CustomerSystem : MonoBehaviour
{
    public static CustomerSystem instance;

    public Sprite noviceSprite;
    public Sprite warriorSprite;

    public enum CustomerType { NOVICE, WARRIOR }

    private Dictionary<CustomerType, CustomerParameters> customerTypeDict = new Dictionary<CustomerType, CustomerParameters>();

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
        CustomerParameters noviceCustomer = new CustomerParameters(noviceSprite, noviceDesiredItems, noviceQuests, noviceDialogue);
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
        CustomerParameters warriorCustomer = new CustomerParameters(warriorSprite, warriorDesiredItems, warriorQuests, warriorDialogue);
        customerTypeDict.Add(CustomerType.WARRIOR, warriorCustomer);
    }

    public Customer GetRandomCustomerFromDistribution()
    {
        CustomerType type;
        System.Random rand = GameController.instance.Rand;
        int generatedNumber = rand.Next(0, 100);
        if (generatedNumber > 80) {
            type = CustomerType.WARRIOR;
        }
        else {
            type = CustomerType.NOVICE;
        }

        return customerTypeDict[type].RollNewCustomer();
    }

}

// Ranges of possible characteristics for a given CustomerType.
public class CustomerParameters
{
    public enum Mood { HAPPY, NEUTRAL, IRRITATED, MAD }

    public Sprite sprite;
    public InventorySystem.SellableItem[] desiredItems;
    public Quest[] quests;
    public string[] dialogue;
    public Mood mood;
    public bool hasBeenServed;

    public CustomerParameters(Sprite sprite, InventorySystem.SellableItem[] desiredItems, Quest[] quests, string[] dialogue)
    {
        this.sprite = sprite;
        this.desiredItems = desiredItems;
        this.quests = quests;
        this.dialogue = dialogue;
        this.mood = Mood.HAPPY;
        this.hasBeenServed = false;
    }

    // Initialize and return a single new customer.
    public Customer RollNewCustomer() {
        // Decide whether this customer is just here shopping or if they're on a QUEST!
        // Normal shopping explicitly states [desiredItems], but for a quest they are hidden
        // and must be inferred from Quest-specific dialog.

        // 10% chance of being on a Quest.
        bool isQuesting = Random.value > 0.9f;

        Quest quest = quests[Random.Range(0, quests.Length)];
        string introDialogue = dialogue[Random.Range(0, dialogue.Length)];

        // If they're questing, override their default desired items with hidden Quest goal items.
        desiredItems = isQuesting ? quest.applicableItems : desiredItems;

        return new Customer(sprite, desiredItems, isQuesting, quest, introDialogue);
    }
}

// Information about a single customer. This object can be given to the controller and also passed around to other systems
// so that they can react appropriately to the current state of a single Customer.
public class Customer {
    public Sprite sprite;
    public InventorySystem.SellableItem[] desiredItems;
    public bool isQuesting;
    public Quest quest;
    public string dialogue;

    public CustomerParameters.Mood mood = CustomerParameters.Mood.HAPPY;
    public bool hasBeenServed = false;

    public Customer(Sprite sprite, InventorySystem.SellableItem[] desiredItems, bool isQuesting, Quest quest, string dialogue) {
        this.sprite = sprite;
        this.desiredItems = desiredItems;
        this.isQuesting = isQuesting;
        this.quest = quest;
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