using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeSystem : MonoBehaviour {
    public static ForgeSystem instance;

    private static readonly float DEFAULT_FORGE_TIME = 10f;

    public GameObject forgeAnimationOverlay;

    public Recipe[] recipes;

    private float forgeTimer;
    private bool forgeActive;
    private Recipe currentlyForgingRecipe;

    private void Awake() {
        if (instance) {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        forgeAnimationOverlay.SetActive(false);
    }

    // Update is called once per frame
    void Update() {

        // TODO: Use TimeSystem.
        if (forgeActive) {
            forgeTimer += Time.deltaTime;

            if (forgeTimer > currentlyForgingRecipe.GetForgeTime()) {
                FinishForging();
            }
        }
    }

    public float ForgeProgress() {
        return forgeTimer / currentlyForgingRecipe.GetForgeTime();
    }

    public void StartForging(Recipe recipe) {
        forgeActive = true;
        currentlyForgingRecipe = recipe;
        forgeTimer = 0f;
        forgeAnimationOverlay.SetActive(true);
        AudioController.instance.PlayForgeHammerSound();
        InventorySystem.instance.StartCrafting(recipe);
    }

    public void FinishForging() {
        forgeActive = false;
        InventorySystem.instance.FinishCrafting(currentlyForgingRecipe);
        currentlyForgingRecipe = null;
        forgeAnimationOverlay.SetActive(false);
    }

    public bool ForgingThis(Recipe recipe) {
        return forgeActive && currentlyForgingRecipe.Equals(recipe);
    }

    public Recipe CurrentRecipe() {
        return currentlyForgingRecipe;
    }

    public Recipe[] GetRecipes() {
        // Limit the Recipes available shown as defined by the LevelSystem.
        List<Recipe> restrictedRecipes = new List<Recipe>();
        foreach (Recipe recipe in recipes) {
            if (LevelManager.instance.HaveAccessToItem(recipe.GetResultItem())) {
                restrictedRecipes.Add(recipe);
            }
        }
        return restrictedRecipes.ToArray();
    }

    public bool IsForgeActive() {
        return forgeActive;
    }
}
