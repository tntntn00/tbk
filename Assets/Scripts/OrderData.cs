using UnityEngine;

public class OrderData : MonoBehaviour
{
    public static OrderData Instance;

    [Header("Current Order")]
    public string flavor = "Spicy";
    public string riceCake = "Wheat";

    public bool cheese = true;
    public bool greenOnion = true;
    public bool coolpis = true;

    [Header("Player Result")]
    public string resultFlavor = "Spicy";
    public string resultRiceCake = "Wheat";

    public bool resultCheese = true;
    public bool resultGreenOnion = false;
    public bool resultCoolpis = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }
}
