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
        // 이미 이름을 입력한 적이 있으면
        // 이름 입력창 없이 바로 메인 허브 표시
        if (!string.IsNullOrEmpty(PlayerName))
        {
            nameInputPanel.SetActive(false);
            mainPanel.SetActive(true);

            logoImage.enabled = false;

            shopNameText.text =
                PlayerName + "의 떡볶이";

            // 기존 주문 UI 다시 표시
            if (OrderManager.Instance != null)
            {
                OrderManager.Instance.SetBillAreaVisible(true);
            }

            return;
        }

        // 최초 실행
        nameInputPanel.SetActive(true);
        mainPanel.SetActive(false);

        logoImage.enabled = true;

        // 이름 입력 중에는 주문서 영역 숨김
        if (OrderManager.Instance != null)
        {
            OrderManager.Instance.SetBillAreaVisible(false);
        }

        inputField.text = "";

        inputField.Select();
        inputField.ActivateInputField();
    }

    public void ConfirmName()
    {
        string playerName =
            inputField.text.Trim();

        // 빈 이름 방지
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning(
                "이름을 입력하세요."
            );

            inputField.Select();
            inputField.ActivateInputField();

            return;
        }

        // 이름 저장
        PlayerName = playerName;

        // 상호명 표시
        shopNameText.text =
            PlayerName + "의 떡볶이";

        // 메인 허브 활성화
        mainPanel.SetActive(true);

        // 이름 입력 UI 제거
        logoImage.enabled = false;
        nameInputPanel.SetActive(false);

        // 이름 입력 완료 후 주문 UI 표시 + 주문 시작
        if (OrderManager.Instance != null)
        {
            OrderManager.Instance.SetBillAreaVisible(true);
            OrderManager.Instance.StartOrderSystem();
        }
        else
        {
            Debug.LogWarning(
                "OrderManager.Instance가 없습니다."
            );
        }

        Debug.Log(
            "가게 이름 저장: "
            + PlayerName
            + "의 떡볶이"
        );
    }
}