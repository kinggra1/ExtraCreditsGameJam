﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeUIHandler : MonoBehaviour {
    public GameObject ingredientUIPrefab;
    public GameObject ingredientHorizontalLayout;

    private Recipe recipe;
    private List<IngredientUIHandler> ingredientPanels = new List<IngredientUIHandler>();

    // Start is called before the first frame update
    void Awake() {
        foreach (Transform child in ingredientHorizontalLayout.transform) {
            GameObject.Destroy(child.gameObject);
        }
    }

    public Recipe GetRecipe() {
        return recipe;
    }

    public void SetRecipe(Recipe recipe) {
        this.recipe = recipe;
        foreach (Ingredient ingredient in recipe.GetIngredients()) {
            GameObject ingredientPanel = Instantiate(ingredientUIPrefab, ingredientHorizontalLayout.transform);
            IngredientUIHandler ingredientUI = ingredientPanel.GetComponent<IngredientUIHandler>();

            ingredientUI.SetContent(InventorySystem.instance.LookupImage(ingredient.resource), ingredient.quantity);
            ingredientPanels.Add(ingredientUI);
        }
    }

    public void Craft() {
        if (InventorySystem.instance.CanCraft(recipe)) {
            InventorySystem.instance.Craft(recipe);
        } else {
            // TODO: :^(
        }
    }

    public bool RefreshUI() {
        Ingredient[] ingredients = recipe.GetIngredients();
        bool allValid = true;
        for (int i = 0; i < ingredients.Length; i++) {
            Ingredient ingredient = ingredients[i];
            if (!InventorySystem.instance.CanSpendResource(ingredient.resource, ingredient.quantity)) {
                // Set the ingredient panel to look faded and make sure we return false.
                ingredientPanels[i].SetFaded(true);
                allValid = false;
            } else {
                ingredientPanels[i].SetFaded(false);
            }
        }
        return allValid;
    }

    // Update is called once per frame
    void Update() {
        
    }
}
