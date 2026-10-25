using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BillItemUI : MonoBehaviour
{
    [Header("Order Texts")]
    [SerializeField] private TMP_Text orderNumberText;
    [SerializeField] private TMP_Text sauceText;
    [SerializeField] private TMP_Text riceCakeText;
    [SerializeField] private TMP_Text spicyText;
    [SerializeField] private TMP_Text ingredientText;
    [SerializeField] private TMP_Text sideMenuText;

    [Header("Timer")]
    [SerializeField] private Slider timeSlider;

    [Header("Cancel")]
    [SerializeField] private Button cancelbutton;

    public void Setup(
        int orderNumber,
        string sauce,
        string riceCake,
        string spicy,
        string ingredients,
        string sideMenus,
        float maxTime)
    {
        orderNumberText.text =
            "ORDER #" + orderNumber;

        sauceText.text =
            sauce;

        riceCakeText.text =
            riceCake;

        spicyText.text =
            spicy;

        ingredientText.text =
            ingredients;

        sideMenuText.text =
            sideMenus;

        timeSlider.minValue = 0f;
        timeSlider.maxValue = maxTime;
        timeSlider.value = maxTime;
    }

    public void UpdateTimer(float remainingTime)
    {
        timeSlider.value = remainingTime;
    }

    public void OnclickCancel(){
        OrderManager.Instance.CancelBill(this);
    }
}
