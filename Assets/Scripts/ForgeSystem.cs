using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeSystem : MonoBehaviour {
    public static ForgeSystem instance;

    private static readonly float DEFAULT_FORGE_TIME = 10f;

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
        InventorySystem.instance.StartCrafting(recipe);
    }

    public void FinishForging() {
        forgeActive = false;
        InventorySystem.instance.FinishCrafting(currentlyForgingRecipe);
        currentlyForgingRecipe = null;
    }

    public bool ForgingThis(Recipe recipe) {
        return forgeActive && currentlyForgingRecipe.Equals(recipe);
    }

    public Recipe CurrentRecipe() {
        return currentlyForgingRecipe;
    }

    public Recipe[] GetRecipes() {
        return recipes;
    }

    public bool IsForgeActive() {
        return forgeActive;
    }
}
