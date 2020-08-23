using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientUIHandler : MonoBehaviour {
    public Image resourceImage;
    public Image notEnoughFilter;
    public Text quantityText;

    public void SetContent(Sprite resourceSprite, uint quantityText) {
        resourceImage.sprite = resourceSprite;
        this.quantityText.text = String.Format("{0}", quantityText);
    }

    public void SetFaded(bool fade) {
        notEnoughFilter.enabled = fade;
    }
}
