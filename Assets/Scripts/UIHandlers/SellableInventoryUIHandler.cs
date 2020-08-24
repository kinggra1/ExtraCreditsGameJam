using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellableInventoryUIHandler : MonoBehaviour{
    private static readonly uint CART_SIZE = 3;

    public GameObject inventoryItemPrefab;
    public GameObject inventoryGridLayout;

    public GameObject cartPanelLayout;
    public GameObject cartItemPrefab;

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

    public void SetCustomerData(int moneyAvailable) {

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
        uint totalValue = CalculateCartValue();
        InventorySystem.instance.AddMoney(totalValue);
        foreach (CartItemUIHandler cartItem in cartItems) {
            Recipe recipe = cartItem.recipe;
            // TODO: Unchecked removal may not be safe.
            InventorySystem.instance.SellItem(recipe.GetResultItem(), recipe.GetValue());
            Destroy(cartItem.gameObject);
        }
        cartItems.Clear();
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
    }
}
