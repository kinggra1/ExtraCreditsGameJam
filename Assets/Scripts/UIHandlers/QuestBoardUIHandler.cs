using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestBoardUIHandler : MonoBehaviour {
    // For enabling/disabling when the quest board is full.
    public GameObject questEntryPanel;
    // Pass to QuestSystem to modify the UI when adding a new quest.
    public GameObject questLayoutGroup;

    private static readonly uint DEFAULT_QUANTITY = 5;
    private static readonly uint MAX_QUANTITY = 100;
    // default price per item is determined by what's in the dropdown.
    private static readonly uint MAX_PRICE_PER_ITEM = 100;

    InventorySystem.ResourceType resource;
    uint quantity;
    uint pricePerItem;

    public Text quantityText;
    public Dropdown resourceDropdown1;
    public Dropdown resourceDropdown2;
    public Dropdown resourceDropdown3;

    public Text pricePerItemText;

    // Start is called before the first frame update
    void Start() {
        resourceDropdown1.onValueChanged.AddListener(delegate {
            SetResource(resourceDropdown1);
        });
        SetResource(resourceDropdown1);

        resourceDropdown2.onValueChanged.AddListener(delegate {
            SetResource(resourceDropdown2);
        });
        resourceDropdown3.onValueChanged.AddListener(delegate {
            SetResource(resourceDropdown3);
        });

        ResetOrderPanel();
    }

    // Update is called once per frame
    void Update() {

    }

    private void ResetOrderPanel() {
        quantity = DEFAULT_QUANTITY;
        pricePerItem = InventorySystem.instance.LookupDefaultPrice(resource);
        RefreshUI();
    }

    public void UpdateResourcesAvailable() {
        foreach (ResourceDropdownToggler toggler in GetComponentsInChildren<ResourceDropdownToggler>()) {
            toggler.UpdateState();
        }
    }

    public void IncrementQuanitity() {
        if (quantity < MAX_QUANTITY) {
            quantity++;
        }
        RefreshUI();
    }

    public void DecrementQuantity() {
        if (quantity > 1) {
            quantity--;
        }
        RefreshUI();
    }

    public void IncrementPricePerItem() {
        if (pricePerItem < MAX_QUANTITY) {
            pricePerItem++;
        }
        RefreshUI();
    }

    public void DecrementPricePerItem() {
        if (pricePerItem > 1) {
            pricePerItem--;
        }
        RefreshUI();
    }

    public void SetResource(Dropdown dropdown) {
        resource = InventorySystem.instance.StringToResourceType(dropdown.options[dropdown.value].text);
        pricePerItem = InventorySystem.instance.LookupDefaultPrice(resource);
        RefreshUI();
    }

    private uint NextQuestTotalCost() {
        return quantity * pricePerItem;
    }

    public void AddQuest() {
        if (InventorySystem.instance.CanSpendMoney(NextQuestTotalCost())) {
            QuestSystem.instance.AddNewQuest(
                new QuestSystem.Quest(resource, quantity, pricePerItem));
            ResetOrderPanel();
        } else {
            // TODO: Feedback for failed to post quest.
        }
    }

    public void RefreshUI() {
        quantityText.text = quantity.ToString();
        pricePerItemText.text = String.Format("{0}", pricePerItem);

        resourceDropdown1.gameObject.SetActive(false);
        resourceDropdown2.gameObject.SetActive(false);
        resourceDropdown3.gameObject.SetActive(false);
        switch (LevelManager.instance.CurrentLevel()) {
            case 0:
                resourceDropdown1.gameObject.SetActive(true);
                break;
            case 1:
                resourceDropdown2.gameObject.SetActive(true);
                break;
            case 2:
                resourceDropdown3.gameObject.SetActive(true);
                break;
            default:
                resourceDropdown3.gameObject.SetActive(true);
                break;
        }


        UpdateResourcesAvailable();
    }
}
