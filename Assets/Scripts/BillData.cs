using UnityEngine;

[System.Serializable]
public class BillData
{
    [Header("Order Info")]
    public int orderNumber;

    public string flavor;
    public string riceCake;
    public string spicy;

    public bool fishCake;
    public bool greenOnion;
    public bool cheese;

    public bool coolpis;
    public bool steamedEgg;

    [Header("Timer")]
    public float maxTime;
    public float remainingTime;

    [Header("State")]
    public bool isCompleted;
    public bool isExpired;

    public BillData(
        int orderNumber,
        string flavor,
        string riceCake,
        string spicy,
        bool fishCake,
        bool greenOnion,
        bool cheese,
        bool coolpis,
        bool steamedEgg,
        float maxTime)
    {
        this.orderNumber = orderNumber;

        this.flavor = flavor;
        this.riceCake = riceCake;
        this.spicy = spicy;

        this.fishCake = fishCake;
        this.greenOnion = greenOnion;
        this.cheese = cheese;

        this.coolpis = coolpis;
        this.steamedEgg = steamedEgg;

        this.maxTime = maxTime;
        this.remainingTime = maxTime;

        this.isCompleted = false;
        this.isExpired = false;
    }
}