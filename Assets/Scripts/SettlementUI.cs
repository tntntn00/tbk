using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    [SerializeField] private TMP_Text Round2Total;
    [SerializeField] private TMP_Text Round3Total;
    [SerializeField] private TMP_Text Round4Total;
    [SerializeField] private TMP_Text TotalScore;

    [Header("Final")]
    [SerializeField] private GameObject finalPanel;
    [SerializeField] private Button nextroundbutton;
    void Start()
{
    nextroundbutton.gameObject.SetActive(true);
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
GameProgress.RoundScores[GameProgress.Round - 1] = roundSum;  // 이번 라운드 점수 저장

Round1Total.text = RoundText(0);
Round2Total.text = RoundText(1);
Round3Total.text = RoundText(2);
Round4Total.text = RoundText(3);

int total = 0;
foreach (int s in GameProgress.RoundScores)
    if (s >= 0) total += s;
TotalScore.text = total + "/100점";

    if (GameProgress.Round >= 4)
    {
        finalPanel.SetActive(true);
        nextroundbutton.gameObject.SetActive(false);
    }
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

    string RoundText(int index)
{
    int s = GameProgress.RoundScores[index];
    return s < 0 ? "-" : s + "/25점";
}

    public void OnNextRoundButton()
{
    if (GameProgress.Round >= 4)
        return;   // 마지막 라운드는 패널로 마무리

    GameProgress.Round++;

    ReviewHistory.Records.Clear();

    if (OrderManager.Instance != null)
        OrderManager.Instance.ResetForNewRound();

    SceneManager.LoadScene("MainScene");
}
}