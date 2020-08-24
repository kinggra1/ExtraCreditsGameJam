using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellableInventoryUIHandler : MonoBehaviour{
    private static readonly uint CART_SIZE = 3;

    public GameObject inventoryItemPrefab;
    public GameObject inventoryGridLayout;

    public GameObject cartPanelLayout;
    public GameObject cartItemPrefab;
    public Text sellButtonText;

    public Image customerImage;
    public Text customerDialogText;
    public Text customerPurchaseHintText;

    public GameObject coinPrefab;
    public GameObject walletUI;

    private Customer customer;

    private List<CartItemUIHandler> cartItems = new List<CartItemUIHandler>();

    void Start() {
        foreach (Transform child in inventoryGridLayout.transform) {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in cartPanelLayout.transform) {
            GameObject.Destroy(child.gameObject);
        }
        RefreshUI();
    }

    // Using this as new initialization because it is only called when reopening the shopping window.
    public void SetCustomerData(Customer customer) {
        this.customer = customer;

        // Clear out anything that was previously the cart and refresh UI.
        foreach (CartItemUIHandler cartItem in cartItems) {
            Destroy(cartItem.gameObject);
        }
        cartItems.Clear();
        RefreshUI();
    }

    public bool WillCustomerBuy() {
        // TODO: Implement logic to match against customer preferences.
        HashSet<InventorySystem.SellableItem> customerDesiredItems = new HashSet<InventorySystem.SellableItem>();
        foreach (InventorySystem.SellableItem item in customer.desiredItems) {
            customerDesiredItems.Add(item);
        }

        HashSet<InventorySystem.SellableItem> suppliedItems = new HashSet<InventorySystem.SellableItem>();
        foreach (CartItemUIHandler cartItem in cartItems) {
            // If the cart has something that the customer doesn't want, the purchase fails immediately.
            if (!customerDesiredItems.Contains(cartItem.recipe.GetResultItem())) {
                return false;
            }
            suppliedItems.Add(cartItem.recipe.GetResultItem());
        }

        return suppliedItems.SetEquals(customerDesiredItems);
    }

    public bool CartFull() {
        return cartItems.Count == CART_SIZE;
    }

    public void AddItemToCart(Recipe recipe) {
        GameObject cartItem = Instantiate(cartItemPrefab, cartPanelLayout.transform);
        CartItemUIHandler cartItemHandler = cartItem.GetComponent<CartItemUIHandler>();
        cartItemHandler.SetContent(this, recipe);
        cartItems.Add(cartItemHandler);
        RefreshUI();
    }

    public void RemoveItemFromCart(InventorySystem.SellableItem item) {
        foreach (CartItemUIHandler cartItem in cartItems) {
            if (cartItem.recipe.GetResultItem().Equals(item)) {
                cartItems.Remove(cartItem);
                Destroy(cartItem.gameObject);
                break;
            }
        }
        RefreshUI();
    }

    public void SellButtonClicked() {
        if (WillCustomerBuy()) {
            CompleteTheSale();
        } else {
            customer.IncorrectSaleAttempt();
            RefreshUI();
            // TODO: When Customer doesn't want what's in the cart.
        }
    }

    private void CompleteTheSale() {
        uint cartValue = CalculateCartValue();
        StatsController.instance.AddRevenue(cartValue);
        StatsController.instance.CustomerServed();
        AudioController.instance.PlayPurchaseSound(cartValue);
        ShopkeeperController.instance.SendOffCustomer();

        // Spawn a bunch of juicy coins where the sell button was (about) and send to wallet. Mmmmm juice.
        for (int i = 0; i < cartValue; i++) {
            GameObject coin = Instantiate(coinPrefab, walletUI.transform);
            RectTransform rectTransform = coin.GetComponent<RectTransform>();
            float randomXOffset = UnityEngine.Random.Range(-40, 40);
            float randomYOffset = UnityEngine.Random.Range(-20, 20);
            rectTransform.anchoredPosition = new Vector2(-400f + randomXOffset, -250f + randomYOffset);
            LeanTween
                .move(rectTransform, Vector3.zero, 0.5f)
                .setDelay(UnityEngine.Random.Range(0f, 0.3f))
                .setEaseInOutCubic()
                .setOnComplete(() => { Destroy(coin); });
        }

        foreach (CartItemUIHandler cartItem in cartItems) {
            Recipe recipe = cartItem.recipe;
            // TODO: Unchecked removal may not be safe.
            InventorySystem.instance.SellItem(recipe.GetResultItem(), recipe.GetValue());
            Destroy(cartItem.gameObject);
        }
        cartItems.Clear();
        customer.hasBeenServed = true;
        InventorySystem.instance.RefreshUI();
    }

    private uint CalculateCartValue() {
        uint totalValue = 0;
        foreach (CartItemUIHandler cartItem in cartItems) {
            totalValue += cartItem.recipe.GetValue();
        }
        return totalValue;
    }

    private Dictionary<InventorySystem.SellableItem, uint> CountItemsInCart() {
        Dictionary<InventorySystem.SellableItem, uint> result = new Dictionary<InventorySystem.SellableItem, uint>();
        foreach (CartItemUIHandler cartItem in cartItems) {
            InventorySystem.SellableItem itemType = cartItem.recipe.GetResultItem();
            if (result.ContainsKey(itemType)) {
                result[itemType] = result[itemType] + 1;
            } else {
                result[itemType] = 1;
            }
        }
        return result;
    }

    public void RefreshUI() {
        Dictionary<InventorySystem.SellableItem, uint> cartTotals = CountItemsInCart();

        // HACKY. Destroy the whole inventory and recreate it for each content refresh... kind of okay though? Blech.
        foreach (Transform child in inventoryGridLayout.transform) {
            GameObject.Destroy(child.gameObject);
        }

        // Draw Invetnory, taking into account what is in the cart.
        foreach (Recipe recipe in ForgeSystem.instance.GetRecipes()) {
            GameObject itemPanel = Instantiate(inventoryItemPrefab, inventoryGridLayout.transform);
            SellableItemUIHandler sellableItemUI = itemPanel.GetComponent<SellableItemUIHandler>();

            InventorySystem.SellableItem sellableItem = recipe.GetResultItem();
            uint inventoryQuantity = InventorySystem.instance.GetSellableItemCount(sellableItem);
            // Deduct inventory if something is in cart.
            if (cartTotals.ContainsKey(sellableItem)) {
                inventoryQuantity -= cartTotals[sellableItem];
            }
            Sprite sprite = IconLookup.instance.ForSellable(sellableItem);

            sellableItemUI.SetContent(this, recipe, inventoryQuantity);
        }

        // Draw Cart.
        foreach (CartItemUIHandler cartItem in cartItems) {
            // TODO: Do we do anything here?
        }

        // Set Sell button data to the value of the cart.
        sellButtonText.text = String.Format("SELL ({0})", CalculateCartValue());

        // Draw Customer Screen data.
        if (customer != null) {
            customerDialogText.text = customer.CustomerIntroText();
            customerImage.sprite = customer.sprite;
        }
    }
}
