using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeUIHandler : MonoBehaviour {

    public GameObject forgeRecipePanelPrefab;
    public GameObject recipeLayoutGroup;

    private List<RecipeUIHandler> recipes = new List<RecipeUIHandler>();

    // Start is called before the first frame update
    void Start() {
        foreach (Transform child in recipeLayoutGroup.transform) {
            GameObject.Destroy(child.gameObject);
        }

        // Initialize with Recipes from ForgeSystem.
        foreach (Recipe recipe in ForgeSystem.instance.GetRecipes()) {
            GameObject recipePanel = Instantiate(forgeRecipePanelPrefab, recipeLayoutGroup.transform);
            RecipeUIHandler recipeUI = recipePanel.GetComponent<RecipeUIHandler>();

            recipeUI.SetRecipe(recipe);
            recipes.Add(recipeUI);
        }
    }

    public void RefreshUI() {
        bool allValid = true;
        foreach (RecipeUIHandler recipePanel in recipes) {
            if (!recipePanel.RefreshUI()) {
                allValid = false;
            }
        }
    }

    // Update is called once per frame
    void Update() {
        
    }
}
