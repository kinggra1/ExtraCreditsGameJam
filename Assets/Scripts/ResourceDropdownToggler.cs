using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceDropdownToggler : MonoBehaviour
{
    private Image panel;
    InventorySystem.ResourceType resource;
    // Start is called before the first frame update
    void Start()
    {
        // The grossest hacks. Can't even remember why I had to do this?
        panel = GetComponentInChildren<Image>();
        foreach (Image image in GetComponentsInChildren<Image>()) {
            if (image.gameObject.name.Equals("Panel")) {
                panel = image;
                break;
            }
        }

        string label = GetComponentInChildren<Text>().text;
        resource = InventorySystem.instance.StringToResourceType(label);

        UpdateState();
    }

    public void UpdateState() {
        if (LevelManager.instance.HaveAccessToResource(resource)) {
            panel.enabled = false;
        }
    }


}
