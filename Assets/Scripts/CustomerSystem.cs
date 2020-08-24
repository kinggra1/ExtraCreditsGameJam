using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

public class CustomerSystem : MonoBehaviour
{
    public static CustomerSystem instance;

    public Transform customerParentTransform;
    public Sprite noviceSprite;
    public Sprite warriorSprite;
    public GameObject novicePrefab;
    public GameObject warriorPrefab;

    public enum CustomerType { PEASANT, NOVICE, WARRIOR }

    private Dictionary<CustomerType, CustomerParameters> customerTypeDict = new Dictionary<CustomerType, CustomerParameters>();

    private enum QuestType { BASIC_FIGHT, ADVANCED_FIGHT, FOOD, SWORD_COLLECTION }
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

        string swordCollectionDialogue = "I'm trying to find every type of sword in the land.";
        InventorySystem.SellableItem[] swordCollectionItems = new InventorySystem.SellableItem[] {
            InventorySystem.SellableItem.WOODEN_SWORD,
            InventorySystem.SellableItem.IRON_SWORD
        };
        Quest swordCollectionQuest = new Quest(swordCollectionDialogue, swordCollectionItems);
        questTypeDict.Add(QuestType.SWORD_COLLECTION, swordCollectionQuest);

        string foodDialogue = "I'm so hungry. Got anything to eat around here?";
        InventorySystem.SellableItem[] foodItems = new InventorySystem.SellableItem[] {
            InventorySystem.SellableItem.BREAD
        };
        Quest foodQuest = new Quest(foodDialogue, foodItems);
        questTypeDict.Add(QuestType.FOOD, foodQuest);
    }

    private void initializeCustomerTypes()
    {
        // Peasant
        InventorySystem.SellableItem[] peasantDesiredItems = new InventorySystem.SellableItem[] {
            InventorySystem.SellableItem.BREAD,
        };
        Quest[] peasantQuests = new Quest[]
        {
            questTypeDict[QuestType.FOOD]
        };
        string[] peasantDialogue = new string[]
        {
            "Just stopping by on some errands.",
            "I wish I could head out adventuring some day.",
            "You've got the best bread in town!"
        };
        CustomerParameters peasantCustomer = new CustomerParameters(noviceSprite, peasantDesiredItems, peasantQuests, peasantDialogue);
        customerTypeDict.Add(CustomerType.PEASANT, peasantCustomer);

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
            questTypeDict[QuestType.SWORD_COLLECTION]
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

    public CustomerType GetRandomCustomerTypeFromDistribution() {

        System.Random rand = GameController.instance.Rand;
        int generatedNumber = rand.Next(0, 100);

        int currentLevel = LevelManager.instance.CurrentLevel();
        switch (currentLevel) {
            case 0:
                // Always return PEASANT for 1st shop level.
                return CustomerType.PEASANT;
            case 1:
                // Second shop level is 80% NOVICE 20% PEASANT
                if (generatedNumber > 80) {
                    return CustomerType.PEASANT;
                }
                else {
                    return CustomerType.NOVICE;
                }
            case 2:
                // Third shop level is 20% WARRIOR, 70% NOVICE, 10% PEASANT
                if (generatedNumber > 80) {
                    return CustomerType.WARRIOR;
                } else if (generatedNumber > 20) {
                    return CustomerType.NOVICE;
                } else {
                    return CustomerType.PEASANT;
                }
            default:
                Debug.Log("OH NO");
                return CustomerType.PEASANT;
        }
    }

    public GameObject CreateNewCustomer() {
        CustomerType customerType = GetRandomCustomerTypeFromDistribution();
        GameObject customerObject;
        switch (customerType) {
            case CustomerType.PEASANT:
                customerObject = Instantiate(novicePrefab, customerParentTransform);
                break;
            case CustomerType.NOVICE:
                customerObject = Instantiate(novicePrefab, customerParentTransform);
                break;
            case CustomerType.WARRIOR:
                customerObject = Instantiate(warriorPrefab, customerParentTransform);
                break;
            default:
                customerObject = null;
                break;
        }

        Customer custmer = customerTypeDict[customerType].RollNewCustomer();
        customerObject.GetComponent<CustomerController>().SetCustomerData(custmer);
        return customerObject;
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
        bool isQuesting = UnityEngine.Random.value > 0.9f;

        Quest quest = quests[UnityEngine.Random.Range(0, quests.Length)];
        string introDialogue = dialogue[UnityEngine.Random.Range(0, dialogue.Length)];

        // If they're questing, override their default desired items with hidden Quest goal items.
        desiredItems = isQuesting ? quest.applicableItems : desiredItems;

        return new Customer(sprite, desiredItems, isQuesting, quest, introDialogue);
    }
}

// Information about a single customer. This object can be given to the controller and also passed around to other systems
// so that they can react appropriately to the current state of a single Customer.
public class Customer {
    private static readonly string[] INCORRECT_SALE_RESPONSES = new string[] {
        "Mmm, not really what I'm looking for.",
        "This doesn't all seem necessary.",
        "Why are you trying to sell me this?",
        "Yeaaah, I don't think I want that.",
        "Is this some kind of joke to you?",
        "Were you even listening to me?",
        "Why? Why would I want that?",
        "I think you have me confused with someone else.",
        "Is it free? I’m not paying for something I don’t need.",
        "This is not how you get a return customer.",
        "What? No! How many heroes have you sent to their death?",
        "I want the good stuff. Not whatever this is…"
    };

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

    public void IncorrectSaleAttempt() {
        dialogue = INCORRECT_SALE_RESPONSES[UnityEngine.Random.Range(0, INCORRECT_SALE_RESPONSES.Length)];
    }

    public string CustomerIntroText() {
        string text = this.dialogue;
        if (this.isQuesting) {
            text += "\n\n" + this.quest.description;
        }
        else {
            text += "\n\n" + CustomerItemsAsSentence();
        }
        return text;
    }

    private string Specifier(string item) { 
        if (item.StartsWith("Iron")) {
            return "an";
        } if (item.StartsWith("Bread")) {
            return "some";
        } else {
            return "a";
        }
    }

    private string CustomerItemsAsSentence() {
        string item1, item2, item3;
        string specifier;
        switch (desiredItems.Length) {
            case 1:
                item1 = InventorySystem.instance.SellableTypeToString(desiredItems[0]);
                specifier = Specifier(item1);
                return String.Format("Can I get {0} {1}?", specifier, item1);
            case 2:
                item1 = InventorySystem.instance.SellableTypeToString(desiredItems[0]);
                item2 = InventorySystem.instance.SellableTypeToString(desiredItems[1]);
                string specifier1 = Specifier(item1);
                string specifier2 = Specifier(item2);
                return String.Format("I want {0} {1} and {2} {3}.", specifier1, item1, specifier2, item2);
            case 3:
                item1 = InventorySystem.instance.SellableTypeToString(desiredItems[0]);
                item2 = InventorySystem.instance.SellableTypeToString(desiredItems[1]);
                item3 = InventorySystem.instance.SellableTypeToString(desiredItems[2]);
                specifier = Specifier(item3);
                return String.Format("Looking for {0}, {1}, and {2} {3}.", item1, item2, specifier, item3);
            default:
                return "I have no idea what I'm doing here.";
        }
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