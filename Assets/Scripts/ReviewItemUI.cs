using TMPro;
using UnityEngine;

public class ReviewItemUI : MonoBehaviour
{
    [Header("Review Texts")]
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text starText;
    [SerializeField] private TMP_Text orderText;
    [SerializeField] private TMP_Text commentText;

    public void Setup(
        int day,
        int stars,
        string order,
        string comment)
    {
        dayText.text =
            "Day " + day;

        starText.text =
            stars + " / 5";

        orderText.text =
            "Order:\n" + order;

        commentText.text =
            "\"" + comment + "\"";
    }
}
