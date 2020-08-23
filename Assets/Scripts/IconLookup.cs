using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconLookup : MonoBehaviour {
    public static IconLookup instance;

    public Sprite moneyIcon;
    public Sprite woodIcon;
    public Sprite ironIcon;
    public Sprite wheatIcon;


    public Sprite breadIcon;

    public Sprite woodenSwordIcon;
    public Sprite woodenShieldIcon;

    public Sprite ironSwordIcon;
    public Sprite ironShieldIcon;

    private void Awake() {
        if (instance) {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    public Sprite ForResource(InventorySystem.ResourceType type) {
        switch (type) {
            case InventorySystem.ResourceType.NONE:
                return null;
            case InventorySystem.ResourceType.WOOD:
                return woodIcon;
            case InventorySystem.ResourceType.IRON:
                return ironIcon;
            case InventorySystem.ResourceType.WHEAT:
                return wheatIcon;
            default:
                return null;
        }
    }

    public Sprite ForSellable(InventorySystem.SellableItem item) {
        switch (item) {
            case InventorySystem.SellableItem.NONE:
                return null;
            case InventorySystem.SellableItem.BREAD:
                return breadIcon;
            case InventorySystem.SellableItem.WOODEN_SHIELD:
                return woodenShieldIcon;
            case InventorySystem.SellableItem.WOODEN_SWORD:
                return woodenSwordIcon;
            case InventorySystem.SellableItem.IRON_SHIELD:
                return ironShieldIcon;
            case InventorySystem.SellableItem.IRON_SWORD:
                return ironSwordIcon;
            default:
                return null;
        }
    }
}
