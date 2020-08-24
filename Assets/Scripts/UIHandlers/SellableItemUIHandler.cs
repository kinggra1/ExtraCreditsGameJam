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
    public GameObject notEnoughFilter;

    private SellableInventoryUIHandler cartSystem;

    public void SetContent(SellableInventoryUIHandler cartSystem, Recipe recipe, uint quanitity) {
        this.cartSystem = cartSystem;
        this.recipe = recipe;
        resourceImage.sprite = IconLookup.instance.ForSellable(recipe.GetResultItem());
        this.quantityText.text = String.Format("{0}", quanitity);

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
