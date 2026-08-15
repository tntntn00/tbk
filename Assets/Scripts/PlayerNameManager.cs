using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class PlayerNameManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject nameInputPanel;
    [SerializeField] private GameObject mainPanel;

    [Header("Name UI")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text shopNameText;
    [SerializeField] private Image logoImage;

    private void Start()
    {
        // 시작할 때 입력 화면만 표시
        nameInputPanel.SetActive(true);
        mainPanel.SetActive(false);
        logoImage.enabled=true;

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

        // 가게 이름 변경
        shopNameText.text = playerName + "의 떡볶이 가게";

        // 메인 화면부터 켜기
        mainPanel.SetActive(true);
        logoImage.enabled=false;

        // 입력창, Confirm 버튼, 흐릿한 배경을 포함한 패널 전체 끄기
        nameInputPanel.SetActive(false);

        Debug.Log("NameInputPanel 비활성화 완료");
    }
}
