using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellableItemUIHandler : MonoBehaviour
{
    public Recipe recipe;

    public Image resourceImage;
    public Text quantityText;
    public Text itemName;
    public Text goldValue;
    public GameObject notEnoughFilter;

    private SellableInventoryUIHandler cartSystem;

    public void SetContent(SellableInventoryUIHandler cartSystem, Recipe recipe, uint quanitity) {
        this.cartSystem = cartSystem;
        this.recipe = recipe;
        resourceImage.sprite = IconLookup.instance.ForSellable(recipe.GetResultItem());
        this.quantityText.text = String.Format("{0}", quanitity);
        this.itemName.text = InventorySystem.instance.SellableTypeToString(recipe.GetResultItem());
        this.goldValue.text = String.Format("{0}", recipe.GetValue());

        SetFaded(quanitity == 0);
    }

    public void AddItemToCart() {
        if (!cartSystem.CartFull()) {
            cartSystem.AddItemToCart(recipe);
        }
    }

    private void SetFaded(bool fade) {
        notEnoughFilter.SetActive(fade);
    }
}
