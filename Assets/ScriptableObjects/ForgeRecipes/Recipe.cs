using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe.asset", menuName = "Crafting/Recipe")]
public class Recipe : ScriptableObject {

    [SerializeField]
    private InventorySystem.SellableItem resultItem;
    [SerializeField]
    private float forgeTime;
    [SerializeField]
    private uint saleValue;
    // Collection of ingredients required to create this recipe
    [SerializeField]
    private Ingredient[] ingredients;

    // Override in subtypes for any special behavior to determine if the player can consume.
    public bool CanCraft() {
        foreach (Ingredient ingredient in ingredients) {
            uint available = InventorySystem.instance.GetResourceCount(ingredient.resource);
            if (available < ingredient.quantity) {
                return false;
            }
        }
        return true;
    }

    public InventorySystem.SellableItem GetResultItem() {
        return resultItem;
    }

    public Ingredient[] GetIngredients() {
        return ingredients;
    }

    public uint GetValue() {
        return saleValue;
    }

    public float GetForgeTime() {
        return forgeTime;
    }
}

[System.Serializable]
public class Ingredient {
    public InventorySystem.ResourceType resource;
    public uint quantity;

    public Ingredient(InventorySystem.ResourceType resource, uint quantity) {
        this.resource = resource;
        this.quantity = quantity;
    }
}