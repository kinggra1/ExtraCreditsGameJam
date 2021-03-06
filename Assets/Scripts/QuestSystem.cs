﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestSystem : MonoBehaviour {
    public static QuestSystem instance;

    public GameObject postedQuestUIPrefab;
    public GameObject questLayoutGroupUI;
    public GameObject questEntryPanelUI;

    private static readonly uint MAX_QUESTS = 5;
    private List<QuestPanelUIHandler> quests = new List<QuestPanelUIHandler>();

    public struct Quest {
        public InventorySystem.ResourceType resource;
        public uint amount;
        public uint pricePerResource;

        public Quest(InventorySystem.ResourceType resource, uint amount, uint pricePerResource) {
            this.resource = resource;
            this.amount = amount;
            this.pricePerResource = pricePerResource;
        }

        public uint TotalCost() {
            return amount * pricePerResource;
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
        AudioController.instance.PlaySoundForResource(quest.resource);

        // Parenting to the new Quest to layout group allows the UI to dynamically position it.
        GameObject newQuestPanel = Instantiate(postedQuestUIPrefab, questLayoutGroupUI.transform);

        // Add information about the Quest to the script on the Panel itself.
        QuestPanelUIHandler questPanelScript = newQuestPanel.GetComponent<QuestPanelUIHandler>();
        // Add to quest collection.
        quests.Add(questPanelScript);
        int index = quests.Count - 1;
        questPanelScript.questIndex = index;

        int recommendedPrice = (int)InventorySystem.instance.LookupDefaultPrice(quest.resource);
        int purchasePrice = (int)quest.pricePerResource;
        int questDuration = 30 + (recommendedPrice - purchasePrice) * 10;
        questDuration = Mathf.Clamp(questDuration, 10, 50);
        questPanelScript.questDuration = questDuration; // TODO: Set actual duration based on prices.
        questPanelScript.quest = quest;

        // Parenting to the layout group allows the UI to dynamically position this Quest.
        newQuestPanel.transform.parent = questLayoutGroupUI.transform;
        newQuestPanel.transform.SetSiblingIndex(index);

        InventorySystem.instance.SpendMoney(quest.TotalCost());
        StatsController.instance.AddCosts((int)quest.TotalCost());

        ReindexQuests();
    }

    public void RefundQuest(int index) {
        QuestPanelUIHandler questPanel = quests[index];
        Quest quest = questPanel.quest;
        InventorySystem.instance.AddMoney(quest.TotalCost());
        StatsController.instance.AddCosts(-(int)quest.TotalCost());


        // Cleanup.
        quests.RemoveAt(index);
        GameObject.Destroy(questPanel.gameObject);
        ReindexQuests();
    }

    public void CompleteQuest(int index) {
        QuestPanelUIHandler questPanel = quests[index];
        Quest quest = questPanel.quest;
        InventorySystem.instance.AddResource(quest.resource, quest.amount);

        // Cleanup.
        quests.RemoveAt(index);
        GameObject.Destroy(questPanel.gameObject);
        ReindexQuests();
    }

    private void ReindexQuests() {
        for (int i = 0; i < quests.Count; i++) {
            quests[i].questIndex = i;
            quests[i].gameObject.transform.SetSiblingIndex(i);
        }

        // Place QuestEntryPane at the end of all quests, and show if it is currently hidden.
        // questEntryPanelUI.transform.SetSiblingIndex(quests.Count);
        // no JK, set it at the beginning, because then it won't move around as quests complete.
        questEntryPanelUI.transform.SetAsFirstSibling();

        // Hide the quest entry panel if we have max quests.
        // Or show it if we don't and it was previously hidden.
        if (quests.Count == MAX_QUESTS) {
            questEntryPanelUI.SetActive(false);
        } else if (!questEntryPanelUI.activeInHierarchy) {
            questEntryPanelUI.SetActive(true);
        }
    }

    public uint GetRequestedResources(InventorySystem.ResourceType type) {
        throw new System.NotImplementedException();
     }
}
