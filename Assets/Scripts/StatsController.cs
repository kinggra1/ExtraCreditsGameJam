using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsController : MonoBehaviour
{

    public static StatsController instance;

    uint revenueToday;
    uint revenueTotal;

    int costsToday;
    int costsTotal;

    uint customersServedToday;
    uint customersServedTotal;

    uint customersMissedToday;
    uint customersMissedTotal;

    private void Awake() {
        if (instance) {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    public void ResetDailyCounts() {
        revenueToday = 0;
        costsToday = 0;
        customersServedToday = 0;
        customersMissedToday = 0;
    }

    public void CustomerServed() {
        customersServedToday++;
        customersServedTotal++;
    }

    public uint GetCustomersServedToday() {
        return customersServedToday;
    }

    public void CustomerMissed() {
        customersMissedToday++;
        customersMissedTotal++;
    }

    public uint GetCustomersMissedToday() {
        return customersMissedToday;
    }

    public void AddRevenue(uint delta) {
        revenueToday += delta;
        revenueTotal += delta;
    }

    public uint GetRevenueToday() {
        return revenueToday;
    }

    public void AddCosts(int delta) {
        costsToday += delta;
        costsTotal += delta;
    }

    public int GetCostsToday() {
        return costsToday;
    }


}
