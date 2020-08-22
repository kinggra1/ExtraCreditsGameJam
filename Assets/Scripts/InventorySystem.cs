using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour {
    public static InventorySystem instance;
    public enum ResourceType { WOOD, IRON, WHEAT }

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
    }

    // Calls to this should always be validated by calling "CanSpendResource"
    public void SpendResource(ResourceType type, uint delta) {
        resourceCounts[type] = resourceCounts[type] - delta;
    }

    public bool CanAddMoney(uint delta) {
        return money + delta <= MAX_MONEY;
    }

    public bool CanSpendMoney(uint delta) {
        return delta > money;
    }

    public void AddMoney(uint delta) {
        money += delta;
    }

    public void SpendMoney(uint delta) {
        money -= delta;
    }
}
