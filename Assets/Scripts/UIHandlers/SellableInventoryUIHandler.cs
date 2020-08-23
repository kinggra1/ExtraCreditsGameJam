using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellableInventoryUIHandler : MonoBehaviour{
    public GameObject inventoryItemPrefab;
    public GameObject inventoryGridLayout;

    void Start() {
        Dictionary<InventorySystem.SellableItem, uint> items = InventorySystem.instance.GetSellableItems();
        foreach (KeyValuePair<InventorySystem.SellableItem, uint> item in items) {
            GameObject itemPanel = Instantiate(inventoryItemPrefab, inventoryGridLayout.transform);
            IngredientUIHandler ingredientUI = itemPanel.GetComponent<IngredientUIHandler>();

            Sprite sprite = IconLookup.instance.ForSellable(item.Key);
            ingredientUI.SetContent(sprite, item.Value);
        }
    }
}
