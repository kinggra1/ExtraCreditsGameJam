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

    public ForgeUIHandler forgeUI;
    public SellableInventoryUIHandler sellScreneUI;

    private static readonly uint MAX_RESOURCE = 99;
    private static readonly uint MAX_MONEY = 9999;

    private uint money;
    private Dictionary<ResourceType, uint> resourceCounts = new Dictionary<ResourceType, uint>();
    private Dictionary<SellableItem, uint> sellableItemCounts = new Dictionary<SellableItem, uint>();

    private void Awake() {
        if (instance) {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        // Initialize ya bank account.
        money = 100;

        // Initialize default resource values
        resourceCounts.Add(ResourceType.WOOD, 10);
        resourceCounts.Add(ResourceType.IRON, 10);
        resourceCounts.Add(ResourceType.WHEAT, 10);

        // Initialize shop inventory
        sellableItemCounts.Add(SellableItem.BREAD, 5);

        sellableItemCounts.Add(SellableItem.WOODEN_SWORD, 2);
        sellableItemCounts.Add(SellableItem.WOODEN_SHIELD, 5);

        sellableItemCounts.Add(SellableItem.IRON_SWORD, 5);
        sellableItemCounts.Add(SellableItem.IRON_SHIELD, 5);
    }

    private void Start() {
        RefreshUI();
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
        uint count = sellableItemCounts[item];
        return count > 0;
    }

    public void SellItem(SellableItem item, uint money) {
        sellableItemCounts[item] = sellableItemCounts[item] - 1;
        AddMoney(money);
        RefreshUI();
    }

    public uint GetSellableItemCount(SellableItem item) {
        return sellableItemCounts[item];
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

    public void StartCrafting(Recipe recipe) {
        foreach (Ingredient ingredient in recipe.GetIngredients()) {
            // Consume all ingredients required.
            SpendResource(ingredient.resource, ingredient.quantity);
        }
        RefreshUI();
    }

    public void FinishCrafting(Recipe recipe) {
        SellableItem itemType = recipe.GetResultItem();
        sellableItemCounts[itemType] = sellableItemCounts[itemType] + 1;
        RefreshUI();
    }

    public Dictionary<SellableItem, uint> GetSellableItems() {
        return sellableItemCounts;
    }

    public void RefreshUI() {
        moneyText.text = String.Format("{0}", money);
        woodText.text = String.Format("{0}", resourceCounts[ResourceType.WOOD]);
        ironText.text = String.Format("{0}", resourceCounts[ResourceType.IRON]);
        wheatText.text = String.Format("{0}", resourceCounts[ResourceType.WHEAT]);

        forgeUI.RefreshUI();
        sellScreneUI.RefreshUI();
    }
}
