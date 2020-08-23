using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour {
    public static InventorySystem instance;
    public enum ResourceType { NONE, WOOD, IRON, WHEAT }
    public enum SellableItem {
        NONE,
        BREAD,

        WOODEN_SHIELD,
        WOODEN_SWORD,

        IRON_SHIELD,
        IRON_SWORD
    }

    public Text moneyText;
    public Text woodText;
    public Text ironText;
    public Text wheatText;

    public Sprite moneyImage;
    public Sprite woodImage;
    public Sprite ironImage;
    public Sprite wheatImage;

    public ForgeUIHandler forgeUI;

    private static readonly uint MAX_RESOURCE = 99;
    private static readonly uint MAX_MONEY = 9999;

    private uint money;
    private Dictionary<ResourceType, uint> resourceCounts = new Dictionary<ResourceType, uint>();
    private Dictionary<SellableItem, uint> sellableItem = new Dictionary<SellableItem, uint>();

    private void Awake() {
        if (instance) {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    private void Start() {
        // Initialize ya bank account.
        money = 100;

        // Initialize default resource values
        resourceCounts.Add(ResourceType.WOOD, 10);
        resourceCounts.Add(ResourceType.IRON, 10);
        resourceCounts.Add(ResourceType.WHEAT, 10);

        // Initialize shop inventory
        sellableItem.Add(SellableItem.BREAD, 5);

        sellableItem.Add(SellableItem.WOODEN_SWORD, 5);
        sellableItem.Add(SellableItem.WOODEN_SHIELD, 5);

        sellableItem.Add(SellableItem.IRON_SWORD, 5);
        sellableItem.Add(SellableItem.IRON_SHIELD, 5);

        RefreshUI();
    }

    public Sprite LookupImage(ResourceType type) {
        switch (type) {
            case ResourceType.NONE:
                return null;
            case ResourceType.WOOD:
                return woodImage;
            case ResourceType.IRON:
                return ironImage;
            case ResourceType.WHEAT:
                return wheatImage;
            default:
                return null;
        } 
        
    }

    public ResourceType StringToResourceType(string name) {
        switch (name) {
            case "Wood":
                return ResourceType.WOOD;
            case "Iron":
                return ResourceType.IRON;
            case "Wheat":
                return ResourceType.WHEAT;
            default:
                return ResourceType.NONE;
        }
    }

    public string ResourceTypeToString(ResourceType type) {
        switch (type) {
            case ResourceType.NONE:
                return "NULL";
            case ResourceType.WOOD:
                return "Wood";
            case ResourceType.IRON:
                return "Iron";
            case ResourceType.WHEAT:
                return "Wheat";
            default:
                return "NULL";
        }
    }

    // Adding may be validating a new quest, so we need to check QuestBoard for all planned quest resources.
    public bool CanAddResource(ResourceType type, uint delta) {
        uint currentCount = resourceCounts[type];
        uint incomingQuestCount = QuestSystem.instance.GetRequestedResources(type);
        if (currentCount + delta > MAX_RESOURCE) {
            return false;
        }
        return true;
    }

    // Subtracting happens at the forge, so we validate against current quanitity.
    public bool CanSpendResource(ResourceType type, uint delta) {
        uint currentCount = resourceCounts[type];
        return delta <= currentCount;
    }

    // Calls to this should always be validated by calling "CanAddResource"
    // which also accounts for QuestBoard orders when adding.
    public void AddResource(ResourceType type, uint delta) {
        resourceCounts[type] = resourceCounts[type] + delta;
        RefreshUI();
    }

    // Calls to this should always be validated by calling "CanSpendResource"
    public void SpendResource(ResourceType type, uint delta) {
        resourceCounts[type] = resourceCounts[type] - delta;
        RefreshUI();
    }

    public uint GetResourceCount(ResourceType resource) {
        return resourceCounts[resource];
    }

    public bool CanAddMoney(uint delta) {
        return money + delta <= MAX_MONEY;
    }

    public bool CanSpendMoney(uint delta) {
        return delta <= money;
    }

    public void AddMoney(uint delta) {
        money += delta;
        RefreshUI();
    }

    public void SpendMoney(uint delta) {
        money -= delta;
        RefreshUI();
    }

    public bool CanSellItem(SellableItem item) {
        uint count = sellableItem[item];
        return count > 0;
    }

    public void SellItem(SellableItem item, uint money) {
        sellableItem[item] = sellableItem[item] - 1;
        AddMoney(money);
        RefreshUI();
    }


    public bool CanCraft(Recipe recipe) {
        foreach (Ingredient ingredient in recipe.GetIngredients()) {
            // If we don't have enough of any ingredient, we can't craft this.
            if (!CanSpendResource(ingredient.resource, ingredient.quantity)) {
                return false;
            }
        }
        return true;
    }

    public void Craft(Recipe recipe) {
        foreach (Ingredient ingredient in recipe.GetIngredients()) {
            // Consume all ingredients required.
            SpendResource(ingredient.resource, ingredient.quantity);
        }
        // TODO: Add new item as defined in recipe to inventory.
        RefreshUI();
    }

    public void RefreshUI() {
        moneyText.text = String.Format("Money: {0}g", money);
        woodText.text = String.Format("Wood: {0}", resourceCounts[ResourceType.WOOD]);
        ironText.text = String.Format("Iron: {0}", resourceCounts[ResourceType.IRON]);
        wheatText.text = String.Format("Wheat: {0}", resourceCounts[ResourceType.WHEAT]);

        forgeUI.RefreshUI();
    }
}
