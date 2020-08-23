using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestPanelUIHandler : MonoBehaviour
{
    public QuestSystem.Quest quest;
    public int questIndex;
    public float questDuration;

    public Image resourceImage;
    public Text amount;
    public Text type;
    public Text totalPrice;
    public Image progressBar;

    private float timer;

    private void Start() {
        Sprite iconImage = IconLookup.instance.ForResource(quest.resource);
        resourceImage.sprite = iconImage;

        amount.text = String.Format("x{0}", quest.amount);
        type.text = InventorySystem.instance.ResourceTypeToString(quest.resource);
        totalPrice.text = String.Format("{0}g", quest.TotalCost());
    }

    private void Update() {
        timer += Time.deltaTime;
        if (timer > questDuration) {
            QuestSystem.instance.CompleteQuest(questIndex);
        }

        progressBar.fillAmount = timer / questDuration;
    }

    public void RemoveQuestClicked() {
        QuestSystem.instance.RefundQuest(questIndex);
    }
}
