using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUIHandler : MonoBehaviour {
    public Image resultItemImage;
    public GameObject ingredientUIPrefab;
    public GameObject ingredientHorizontalLayout;
    public GameObject blurPanel;
    public GameObject invisiblePanel;
    public Image progressBar;

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
        resultItemImage.sprite = IconLookup.instance.ForSellable(recipe.GetResultItem());

        foreach (Ingredient ingredient in recipe.GetIngredients()) {
            GameObject ingredientPanel = Instantiate(ingredientUIPrefab, ingredientHorizontalLayout.transform);
            IngredientUIHandler ingredientUI = ingredientPanel.GetComponent<IngredientUIHandler>();

            Sprite iconImage = IconLookup.instance.ForResource(ingredient.resource);
            ingredientUI.SetContent(iconImage, ingredient.quantity);
            ingredientPanels.Add(ingredientUI);
        }
    }

    public void Craft() {
        if (InventorySystem.instance.CanCraft(recipe)) {
            ForgeSystem.instance.StartForging(recipe);
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
        // blur out any recipe that is not actively being forged.
        if (ForgeSystem.instance.IsForgeActive()) {
            if (ForgeSystem.instance.ForgingThis(recipe)) {
                invisiblePanel.SetActive(true);
            } else {
                blurPanel.SetActive(true);
            }
        } else {
            invisiblePanel.SetActive(false);
            if (!allValid) {
                blurPanel.SetActive(true);
            } else {
                blurPanel.SetActive(false);
            }
        }
        return allValid;
    }

    // Update is called once per frame
    void Update() {
        if (ForgeSystem.instance.ForgingThis(recipe)) {
            progressBar.gameObject.SetActive(true);
            progressBar.fillAmount = ForgeSystem.instance.ForgeProgress();
        } else {
            progressBar.gameObject.SetActive(false);
        }
    }
}
