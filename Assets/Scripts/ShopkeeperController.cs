using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopkeeperController : MonoBehaviour
{
    public static ShopkeeperController instance;
    public GameObject takeCarePrompt;

    private void Awake()
    {
        if (instance)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    public void SendOffCustomer()
    {
        takeCarePrompt.SetActive(true);
        LeanTween.scale(takeCarePrompt, Vector3.zero, 0.5f).setDelay(2f);
    }
}
