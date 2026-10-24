using TMPro;
using UnityEngine;

public class ReviewItemUI : MonoBehaviour
{
    [Header("Review Texts")]
    [SerializeField] private TMP_Text dayText2;
    [SerializeField] private TMP_Text starText2;
    [SerializeField] private TMP_Text orderText2;
    [SerializeField] private TMP_Text commentText2;

    public void Setup(
        int day,
        int stars,
        string order,
        string comment)
    {
        //dayText.text =
            //day + "일차";

        starText2.text =
            stars + "/5점";

        //orderText.text =
            //"주문 내용: " + order;

        commentText2.text =
            "\"" + comment + "\"";
    }
}
