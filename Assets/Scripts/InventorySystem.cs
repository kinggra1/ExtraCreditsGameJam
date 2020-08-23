using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour {
    public static InventorySystem instance;
    public enum ResourceType { NONE, WOOD, IRON, WHEAT }

    public Text moneyText;
    public Text woodText;
    public Text ironText;
    public Text wheatText;

    private static readonly uint MAX_RESOURCE = 99;
    private static readonly uint MAX_MONEY = 9999;

    private uint money;
    private Dictionary<ResourceType, uint> resourceCounts = new Dictionary<ResourceType, uint>();

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
        if (currentCount - delta < 0) {
            return false;
        }
        return true;
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

    public void RefreshUI() {
        moneyText.text = String.Format("Money: {0}g", money);
        woodText.text = String.Format("Wood: {0}", resourceCounts[ResourceType.WOOD]);
        ironText.text = String.Format("Iron: {0}", resourceCounts[ResourceType.IRON]);
        wheatText.text = String.Format("Wheat: {0}", resourceCounts[ResourceType.WHEAT]);
    }
}
