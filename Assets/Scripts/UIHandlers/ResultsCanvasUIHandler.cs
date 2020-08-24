using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsCanvasUIHandler : MonoBehaviour
{

    public Text customersServed;
    public Text customersMissed;
    public Text revenue;
    public Text costs;
    public Text guildFee;
    public GameObject dividerLine;
    public Text profit;
    public Button upgradeButton;
    public Text upgradeButtonText;

    public void ShowToday() {

        customersServed.text = string.Format("Happy Customers: {0}", StatsController.instance.GetCustomersServedToday());
        customersMissed.text = string.Format("Unhappy Customers: {0}", StatsController.instance.GetCustomersMissedToday());
        uint revenueValue = StatsController.instance.GetRevenueToday();
        revenue.text = string.Format("Revenue: {0}", revenueValue);
        int costValue = StatsController.instance.GetCostsToday();
        costs.text = string.Format("Costs {0}", -costValue);
        uint guildFeeValue = LevelManager.instance.CurrentGuildFee();
        guildFee.text = string.Format("Guild Fee: {0}", -guildFeeValue);
        profit.text = string.Format("Profit: {0}", revenueValue - costValue - guildFeeValue);

        if (InventorySystem.instance.CanSpendMoney(guildFeeValue)) {
            InventorySystem.instance.SpendMoney(guildFeeValue);
        } else {
            GameController.instance.LoseGame();
        }

        SetUpgradeButtonUI();
        HideAll();

        LeanTween.delayedCall(1.0f, () => ActivateStat(customersServed.gameObject));
        LeanTween.delayedCall(1.3f, () => ActivateStat(customersMissed.gameObject));
        LeanTween.delayedCall(2f, () => ActivateStat(revenue.gameObject));
        LeanTween.delayedCall(2.3f, () => ActivateStat(costs.gameObject));
        LeanTween.delayedCall(2.6f, () => ActivateStat(guildFee.gameObject));
        LeanTween.delayedCall(2.6f, () => ActivateStat(dividerLine.gameObject));
        LeanTween.delayedCall(4f, () => ActivateStat(profit.gameObject));
    }

    public void UpgradeButtonPressed() {
        LevelManager.instance.UpgradeShop();
        SetUpgradeButtonUI();
    }

    private void SetUpgradeButtonUI() {
        if (LevelManager.instance.CanUpgrade()) {
            upgradeButton.interactable = true;
        }
        else {
            upgradeButton.interactable = false;
        }
        uint upgradeFee = LevelManager.instance.CurrentUpgradeFee();
        upgradeButtonText.text = string.Format("Upgrade ({0})", upgradeFee);
    }

    private void HideAll() {
        customersServed.gameObject.SetActive(false);
        customersMissed.gameObject.SetActive(false);
        revenue.gameObject.SetActive(false);
        costs.gameObject.SetActive(false);
        guildFee.gameObject.SetActive(false);
        dividerLine.gameObject.SetActive(false);
        profit.gameObject.SetActive(false);
    }

    private void ActivateStat(GameObject stat) {
        stat.SetActive(true);
        AudioController.instance.PlaySmallCoinSound();
    }
}
