using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour {
    public static InventorySystem instance;
    public enum ResourceType { WOOD, IRON, WHEAT }

    private static readonly int MAX_RESOURCE = 99;

    private int woodCount = 10;
    private int ironCount = 10;
    private int wheatCount = 10;

    private Dictionary<ResourceType, int> resourceCounts = new Dictionary<ResourceType, int>();

    private void Awake() {
        if (instance) {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    private void Start() {
        // Initialize default resource values
        resourceCounts.Add(ResourceType.WOOD, 10);
        resourceCounts.Add(ResourceType.IRON, 10);
        resourceCounts.Add(ResourceType.WHEAT, 10);
    }

    // Adding may be validating a new quest, so we need to check QuestBoard for all planned quest resources.
    public bool CanAddResource(ResourceType type, uint delta) {
        int currentCount = resourceCounts[type];
        int incomingQuestCount = QuestSystem.instance.GetRequestedResources(type);
        if (currentCount + delta > MAX_RESOURCE) {
            return false;
        }
        return true;
    }

    // Subtracting happens at the forge, so we validate against current quanitity.
    public bool CanSpendResource(ResourceType type, uint delta) {
        int currentCount = resourceCounts[type];
        if (currentCount - delta < 0) {
            return false;
        }
        return true;
    }

    // Calls to this should always be validated by calling "CanChangeWood(int amount)"
    // which also accounts for QuestBoard orders when adding.
    public void ChangeWood() {

    }
}
