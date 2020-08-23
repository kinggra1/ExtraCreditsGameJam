using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sellable.asset", menuName = "Crafting/Sellable")]
[System.Serializable]
public class Sellable : ScriptableObject {

    [SerializeField]
    private InventorySystem.SellableItem item;
    [SerializeField]
    private uint coins;


    public InventorySystem.SellableItem GetType() {
        return item;
    }

    public uint GetValue() {
        return coins;
    }
}