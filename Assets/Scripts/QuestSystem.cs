using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSystem : MonoBehaviour {
    public static QuestSystem instance;

    public GameObject postedQuestUIPrefab;
    public GameObject questLayoutGroupUI;
    public GameObject questEntryPanelUI;

    private static readonly uint MAX_QUESTS = 5;
    private List<Quest> quests = new List<Quest>();

    public struct Quest {
        InventorySystem.ResourceType resource;
        uint amount;
        uint pricePerResource;

        public Quest(InventorySystem.ResourceType resource, uint amount, uint pricePerResource) {
            this.resource = resource;
            this.amount = amount;
            this.pricePerResource = pricePerResource;
        }
    }

    private void Awake() {
        if (instance) {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start() {

    }

    public void AddNewQuest(Quest quest) {
        quests.Add(quest);
        // Add to UI LayoutGroup.

        // Hide the quest entry panel if we have max quests.
        if (quests.Count == MAX_QUESTS) {
            questEntryPanelUI.active = false;
        }

    }

    public void CompleteQuest() {

    }

    public uint GetRequestedResources(InventorySystem.ResourceType type) {
        throw new System.NotImplementedException();
     }
}
