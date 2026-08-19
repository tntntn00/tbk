using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameManager : MonoBehaviour
{
    public static string PlayerName { get; private set; } = "";

    [Header("Panels")]
    [SerializeField] private GameObject nameInputPanel;
    [SerializeField] private GameObject mainPanel;

    [Header("Name UI")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text shopNameText;
    [SerializeField] private Image logoImage;

    private void Start()
    {
        // 이미 이름을 입력한 적이 있으면 바로 메인 허브 표시
        if (!string.IsNullOrEmpty(PlayerName))
        {
            nameInputPanel.SetActive(false);
            mainPanel.SetActive(true);
            logoImage.enabled = false;
            shopNameText.text = PlayerName + "의 떡볶이";
            return;
        }

        // 최초 실행일 때만 이름 입력창 표시
        nameInputPanel.SetActive(true);
        mainPanel.SetActive(false);
        logoImage.enabled = true;

        inputField.text = "";
        inputField.Select();
        inputField.ActivateInputField();
    }

    public void ConfirmName()
    {
        string playerName = inputField.text.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("이름을 입력하세요.");
            inputField.Select();
            inputField.ActivateInputField();
            return;
        }

        PlayerName = playerName;
        shopNameText.text = PlayerName + "의 떡볶이";

        mainPanel.SetActive(true);
        logoImage.enabled = false;
        nameInputPanel.SetActive(false);

        Debug.Log("가게 이름 저장: " + PlayerName + "의 떡볶이");
    }
}
