using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSystem : MonoBehaviour {
    public static QuestSystem instance;

    private struct Quest {
        InventorySystem.ResourceType resource;
        uint amount;
        uint pricePerResource;

        Quest(InventorySystem.ResourceType resource, uint amount, uint pricePerResource) {
            this.resource = resource;
            this.amount = amount;
            this.pricePerResource = pricePerResource;
        }
    }

    private void Awake() {
        if (instance) {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

    }

    public int GetRequestedResources(InventorySystem.ResourceType type) {
        throw new System.NotImplementedException();
     }
}
