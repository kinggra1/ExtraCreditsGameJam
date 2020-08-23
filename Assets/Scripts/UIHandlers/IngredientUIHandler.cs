using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientUIHandler : MonoBehaviour {
    public Image resourceImage;
    public Text quantityText;

    public void SetContent(Sprite resourceSprite, uint quantityText) {
        resourceImage.sprite = resourceSprite;
        this.quantityText.text = String.Format("x{0}", quantityText);
    }

    public void SetFaded(bool fade) {
        if (fade) {
            resourceImage.color = Color.red;
        } else {
            resourceImage.color = Color.white;
        }
    }
}
