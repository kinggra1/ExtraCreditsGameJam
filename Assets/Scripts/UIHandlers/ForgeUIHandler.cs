using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForgeUIHandler : MonoBehaviour {
    public GameObject forgeRecipePanelPrefab;
    public GameObject recipeLayoutGroup;

    public Scrollbar scrollbar;

    private List<RecipeUIHandler> recipes = new List<RecipeUIHandler>();

    // Start is called before the first frame update
    void Start() {
        RefreshUI();
    }

    public void RefreshUI() {
        foreach (Transform child in recipeLayoutGroup.transform) {
            GameObject.Destroy(child.gameObject);
        }
        // Initialize with Recipes from ForgeSystem.
        foreach (Recipe recipe in ForgeSystem.instance.GetRecipes()) {
            GameObject recipePanel = Instantiate(forgeRecipePanelPrefab, recipeLayoutGroup.transform);
            RecipeUIHandler recipeUI = recipePanel.GetComponent<RecipeUIHandler>();

            recipeUI.SetRecipe(recipe);
            recipes.Add(recipeUI);
            recipeUI.RefreshUI();
        }
    }

    // Update is called once per frame
    void Update() {
        // Hacky handling of scoll bar scaling :^(
        if (transform.localScale.magnitude < 0.9f) {
            scrollbar.value = 1f;
        }
    }
}
