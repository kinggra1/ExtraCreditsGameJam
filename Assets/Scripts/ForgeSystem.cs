using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeSystem : MonoBehaviour {
    public static ForgeSystem instance;

    public Recipe[] recipes;

    private void Awake() {
        if (instance) {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    // Update is called once per frame
    void Update() {

    }

    public Recipe[] GetRecipes() {
        return recipes;
    }
}
