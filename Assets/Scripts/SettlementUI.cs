using UnityEngine;
using TMPro;
public class SettlementUI : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private TMP_Text Firstscore;
    [SerializeField] private TMP_Text Secondscore;
    [SerializeField] private TMP_Text Thirdscore;
    [SerializeField] private TMP_Text Forthscore;
    [SerializeField] private TMP_Text Fifthscore;

    [Header("condition")]
    [SerializeField] private TMP_Text Firstcondition;
    [SerializeField] private TMP_Text Secondcondition;
    [SerializeField] private TMP_Text Thirdcondition;
    [SerializeField] private TMP_Text Forthcondition;
    [SerializeField] private TMP_Text Fifthcondition;

    [SerializeField] private ReviewEvaluator reviewEvaluator;

    [Header("total")]
    [SerializeField] private TMP_Text Round1Total;
[SerializeField] private TMP_Text TotalScore;
    void Start()
{
    Firstscore.text = GetScoreText(0);
    Secondscore.text = GetScoreText(1);
    Thirdscore.text = GetScoreText(2);
    Forthscore.text = GetScoreText(3);
    Fifthscore.text = GetScoreText(4);

    Firstcondition.text = ReviewHistory.Records[0].status;
    Secondcondition.text = ReviewHistory.Records[1].status;
    Thirdcondition.text = ReviewHistory.Records[2].status;
    Forthcondition.text = ReviewHistory.Records[3].status;
    Fifthcondition.text = ReviewHistory.Records[4].status;

    int roundSum = GetStars(0) + GetStars(1) + GetStars(2) + GetStars(3) + GetStars(4);

    Round1Total.text = roundSum + "/25점";
    TotalScore.text = roundSum + "/100점";
}

string GetScoreText(int index)
{
    ReviewRecord record = ReviewHistory.Records[index];

    if (record.status == "주문 취소")
        return "0/5점";

    ReviewResult result = reviewEvaluator.Evaluate(record);
    return result.stars + "/5점";
}

    int GetStars(int index)
{
    ReviewRecord record = ReviewHistory.Records[index];

    if (record.status == "주문 취소")
        return 0;

    return reviewEvaluator.Evaluate(record).stars;
}
}