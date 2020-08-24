using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CartItemUIHandler : MonoBehaviour {
    public Recipe recipe;
    public Image resourceImage;

    private SellableInventoryUIHandler cartSystem;

    public void SetContent(SellableInventoryUIHandler cartSystem, Recipe recipe) {
        this.cartSystem = cartSystem;
        this.recipe = recipe;
        resourceImage.sprite = IconLookup.instance.ForSellable(recipe.GetResultItem());

    }

    public void RemoveFromCart() {
        cartSystem.RemoveItemFromCart(recipe.GetResultItem());
    }
}
