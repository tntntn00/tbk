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

        riceCakeImage.color = new Color32(220, 220, 220, 255);
    }

    public void SelectWheatRiceCake()
    {
        if (isWashing || washCompleted) return;

        selectedRiceCake = "Wheat";
        GameData.SelectedRiceCake = "Wheat";

        riceCakeImage.color = new Color32(228, 210, 165, 255);
        statusText.text = "밀떡이 선택됨";

        wheatButton.interactable = false;
        riceButton.interactable = true;
        washButton.interactable = true;
    }

    public void SelectRiceRiceCake()
    {
        if (isWashing || washCompleted) return;

        selectedRiceCake = "Rice";
        GameData.SelectedRiceCake = "Rice";

        riceCakeImage.color = new Color32(248, 243, 230, 255);
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
            statusText.text = "Select rice cake first";
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