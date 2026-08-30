using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WashManager : MonoBehaviour
{
    [Header("Rice Cake Selection")]
    [SerializeField] private Button wheatButton;
    [SerializeField] private Button riceButton;
    [SerializeField] private Image riceCakeImage;

    [Header("Washing UI")]
    [SerializeField] private Button washButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private GameObject waterEffect;

    [Header("Washing Settings")]
    [SerializeField] private float washDuration = 3f;

    [Header("RiceCake Images")]
    [SerializeField] private Sprite wheatimage;
    [SerializeField] private Sprite riceimage;

    [Header("RiceCake Sizes")]
    [SerializeField] private Vector2 wheatSize = new Vector2(200, 200);
    [SerializeField] private Vector2 riceSize = new Vector2(200, 200);

    private string selectedRiceCake = "";
    private bool isWashing;
    private bool washCompleted;

    private void Start()
    {
        progressSlider.minValue = 0f;
        progressSlider.maxValue = 1f;
        progressSlider.value = 0f;
        progressSlider.interactable = false;

        statusText.text = "떡 종류를 선택하세요!";
        washButton.interactable = false;
        nextButton.gameObject.SetActive(false);
        waterEffect.SetActive(false);
        riceCakeImage.enabled = false;

        //riceCakeImage.color = new Color32(220, 220, 220, 255);
    }

    public void SelectWheatRiceCake()
    {
        if (isWashing || washCompleted) return;

        selectedRiceCake = "밀떡";
        GameData.SelectedRiceCake = "밀떡";

        riceCakeImage.sprite = wheatimage;
        riceCakeImage.enabled = true;
        riceCakeImage.rectTransform.sizeDelta = wheatSize;

        //riceCakeImage.color = new Color32(228, 210, 165, 255);
        statusText.text = "밀떡이 선택됨";

        wheatButton.interactable = false;
        riceButton.interactable = true;
        washButton.interactable = true;
    }

    public void SelectRiceRiceCake()
    {
        if (isWashing || washCompleted) return;

        selectedRiceCake = "쌀떡";
        GameData.SelectedRiceCake = "쌀떡";

        riceCakeImage.sprite = riceimage;
        riceCakeImage.enabled = true;
        riceCakeImage.rectTransform.sizeDelta = riceSize;

        //riceCakeImage.color = new Color32(248, 243, 230, 255);
        statusText.text = "쌀떡이 선택됨";

        wheatButton.interactable = true;
        riceButton.interactable = false;
        washButton.interactable = true;
    }

    public void StartWashing()
    {
        if (isWashing || washCompleted) return;

        if (string.IsNullOrEmpty(selectedRiceCake))
        {
            statusText.text = "떡을 먼저 선택해 주세요.";
            return;
        }

        StartCoroutine(WashRoutine());
    }

    private IEnumerator WashRoutine()
    {
        isWashing = true;

        wheatButton.interactable = false;
        riceButton.interactable = false;
        washButton.interactable = false;

        waterEffect.SetActive(true);
        statusText.text = "헹구는 중...";

        float elapsedTime = 0f;
        while (elapsedTime < washDuration)
        {
            elapsedTime += Time.deltaTime;
            progressSlider.value = elapsedTime / washDuration;
            yield return null;
        }

        progressSlider.value = 1f;
        waterEffect.SetActive(false);

        isWashing = false;
        washCompleted = true;
        statusText.text = "헹구기 완료";
        nextButton.gameObject.SetActive(true);
    }

    public void GoToCookScene()
    {
        if (!washCompleted) return;
        SceneManager.LoadScene("CookScene");
    }
}