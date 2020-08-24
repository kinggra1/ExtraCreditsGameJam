using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public static LevelManager instance;

    private int currentLevel = 0;

    private HashSet<InventorySystem.ResourceType>[] allowedResources = new HashSet<InventorySystem.ResourceType>[] {
        new HashSet<InventorySystem.ResourceType>{ InventorySystem.ResourceType.WHEAT },
        new HashSet<InventorySystem.ResourceType>{ InventorySystem.ResourceType.WHEAT, InventorySystem.ResourceType.WOOD },
        new HashSet<InventorySystem.ResourceType>{ InventorySystem.ResourceType.WHEAT, InventorySystem.ResourceType.WOOD, InventorySystem.ResourceType.IRON }
    };

    private HashSet<InventorySystem.SellableItem>[] allowedItems = 
        new HashSet<InventorySystem.SellableItem>[] {
            new HashSet<InventorySystem.SellableItem>{
                InventorySystem.SellableItem.BREAD
            },
            new HashSet<InventorySystem.SellableItem>{
                InventorySystem.SellableItem.BREAD,
                InventorySystem.SellableItem.WOODEN_SHIELD,
                InventorySystem.SellableItem.WOODEN_SWORD
            },
            new HashSet<InventorySystem.SellableItem>{
                InventorySystem.SellableItem.BREAD,
                InventorySystem.SellableItem.WOODEN_SHIELD,
                InventorySystem.SellableItem.WOODEN_SWORD,
                InventorySystem.SellableItem.IRON_SHIELD,
                InventorySystem.SellableItem.IRON_SWORD
            }
        };

    private uint[] guildFees = new uint[] {
        10,
        20,
        50
    };

    private uint[] upgradeFees = new uint[] {
        200,
        500,
        1000
    };

    private uint[] minDelays = new uint[] {
        20,
        15,
        10
    };

    private void Awake() {
        if (instance) {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    public bool CanUpgrade() {
        return InventorySystem.instance.CanSpendMoney(CurrentUpgradeFee());
    }

    public int CurrentLevel() {
        return currentLevel;
    }

    public void UpgradeShop() {
        if (currentLevel == 2) {
            // YOU WIN.
            GameController.instance.WinGame();
        }
        else {
            AudioController.instance.PlayPurchaseSound(100);
            InventorySystem.instance.SpendMoney(CurrentUpgradeFee());
            currentLevel++;
            InventorySystem.instance.RefreshUI();
        }
    }

    public uint CurrentGuildFee() {
        return guildFees[currentLevel];
    }

    public uint CurrentUpgradeFee() {
        return upgradeFees[currentLevel];
    }

    public bool HaveAccessToResource(InventorySystem.ResourceType type) {
        return allowedResources[currentLevel].Contains(type);
    }

    public bool HaveAccessToItem(InventorySystem.SellableItem type) {
        return allowedItems[currentLevel].Contains(type);
    }

}
