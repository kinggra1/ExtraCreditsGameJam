using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestSystem : MonoBehaviour {
    public static QuestSystem instance;

    public GameObject postedQuestUIPrefab;
    public GameObject questLayoutGroupUI;
    public GameObject questEntryPanelUI;

    private static readonly uint MAX_QUESTS = 5;
    private List<Quest> quests = new List<Quest>();

    public struct Quest {
        public InventorySystem.ResourceType resource;
        public uint amount;
        public uint pricePerResource;

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
        // Parenting to the new Quest to layout group allows the UI to dynamically position it.
        GameObject newQuestPanel = Instantiate(postedQuestUIPrefab, questLayoutGroupUI.transform);
        // TODO: Add a Script to QuestPanel prefab that allows us to easily set the text and dropdown index.
        newQuestPanel.GetComponentInChildren<Text>().text = quest.amount.ToString();
        // Parenting to the layout group allows the UI to dynamically position this Quest.
        newQuestPanel.transform.parent = questLayoutGroupUI.transform;
        // If this is the 3rd quest, put it in index 2 on the layout group.
        newQuestPanel.transform.SetSiblingIndex(quests.Count - 1);

        // Hide the quest entry panel if we have max quests.
        if (quests.Count == MAX_QUESTS) {
            questEntryPanelUI.SetActive(false);
        }
    }

    public void RemoveQuest() {
        if (!questEntryPanelUI.activeInHierarchy) {
            questEntryPanelUI.SetActive(true);
        }
    }

    public uint GetRequestedResources(InventorySystem.ResourceType type) {
        throw new System.NotImplementedException();
     }
}
