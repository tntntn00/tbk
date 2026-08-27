using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CookManager : MonoBehaviour
{
    [Header("Sauce Buttons")]
    [SerializeField] private Button originalButton;
    [SerializeField] private Button creamButton;
    [SerializeField] private Button jjajangButton;

    [Header("Ingredient Buttons")]
    [SerializeField] private Button fishCakeButton;
    [SerializeField] private Button greenOnionButton;
    [SerializeField] private Button cheeseButton;

    [Header("Spicy Buttons")]
    [SerializeField] private Button mildButton;
    [SerializeField] private Button mediumButton;
    [SerializeField] private Button spicyButton;

    [Header("Cooking UI")]
    [SerializeField] private Button startCookingButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Slider cookingSlider;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private TMP_Text potStatusText;

    [Header("Pot Images")]
    [SerializeField] private Image sauceImage;
    [SerializeField] private Image riceCakeImage;

    [SerializeField] private GameObject fishCakeImage;
    [SerializeField] private GameObject greenOnionImage;
    [SerializeField] private GameObject cheeseImage;

    [SerializeField] private GameObject cookingEffect;

    [Header("Heat Control")]
    [SerializeField] private GameObject heatGaugePanel;
    [SerializeField] private RectTransform heatGauge;
    [SerializeField] private RectTransform heatMarker;

    [SerializeField] private Button fireButton;
    [SerializeField] private TMP_Text fireButtonText;
    [SerializeField] private GameObject fireEffect;

    [Header("Heat Settings")]
    [SerializeField] private float markerSpeed = 280f;

    [Range(0.1f, 0.8f)]
    [SerializeField] private float perfectZoneRatio = 0.30f;

    [Header("Scene")]
    [SerializeField] private string nextSceneName = "PackageScene";

    [Header("Washing Settings")]
    [SerializeField] private float washDuration = 3f;

    [Header("RiceCake Images")]
    [SerializeField] private Sprite wheatimage;
    [SerializeField] private Sprite riceimage;

    private string selectedSauce = "";
    private string selectedSpicy = "";

    private bool fishCakeSelected;
    private bool greenOnionSelected;
    private bool cheeseSelected;

    private bool heatGameRunning;
    private bool cookingCompleted;

    private float markerDirection = 1f;

    private Color originalNormalColor;
    private Color creamNormalColor;
    private Color jjajangNormalColor;

    private Color fishCakeNormalColor;
    private Color greenOnionNormalColor;
    private Color cheeseNormalColor;

    private Color mildNormalColor;
    private Color mediumNormalColor;
    private Color spicyNormalColor;

    private readonly Color selectedHighlightColor =
        new Color32(210, 255, 210, 255);

    private readonly Color selectedpepperHighlightColor =
    new Color32(210, 255, 210, 200);

    private void Start()
    {
        SaveOriginalButtonColors();

        cookingSlider.minValue = 0f;
        cookingSlider.maxValue = 1f;
        cookingSlider.value = 0f;
        cookingSlider.interactable = false;
        riceCakeImage.enabled = false;

        statusText.text = "소스/재료/맵기를 선택하세요.";
        potStatusText.text = "Select sauce and ingredients";

        startCookingButton.interactable = false;
        nextButton.gameObject.SetActive(false);

        fireButton.interactable = false;
        fireButtonText.text = "Fire OFF";

        fishCakeImage.SetActive(false);
        greenOnionImage.SetActive(false);
        cheeseImage.SetActive(false);

        //cookingEffect.SetActive(false);
        fireEffect.SetActive(false);
        heatGaugePanel.SetActive(false);

        sauceImage.color =
            new Color32(170, 210, 225, 255);

        ApplyRiceCakeSelection();

        RestoreAllButtonColors();
    }

    private void Update()
    {
        if (!heatGameRunning)
            return;

        MoveHeatMarker();
    }

    private void ApplyRiceCakeSelection()
    {
        if (GameData.SelectedRiceCake == "Wheat")
        {
            //riceCakeImage.color =
                //new Color32(228, 210, 165, 255);
            riceCakeImage.sprite = wheatimage;
            riceCakeImage.enabled = true;
        }
        else if (GameData.SelectedRiceCake == "Rice")
        {
            //riceCakeImage.color =
                //new Color32(248, 243, 230, 255);
            riceCakeImage.sprite = riceimage;
            riceCakeImage.enabled = true;
        }
        else
        {
            riceCakeImage.color =
                new Color32(235, 235, 235, 255);
        }
    }

    public void SelectOriginalSauce()
    {
        if (CannotChangeSelection())
            return;

        selectedSauce = "Original";

        sauceImage.color =
            new Color32(230, 55, 45, 190);

        RestoreSauceButtonColors();
        originalButton.image.color = selectedHighlightColor;

        UpdateStatus();
        CheckStartCondition();
    }

    public void SelectCreamSauce()
    {
        if (CannotChangeSelection())
            return;

        selectedSauce = "Cream";

        sauceImage.color =
            new Color32(255, 190, 110, 190);

        RestoreSauceButtonColors();
        creamButton.image.color = selectedHighlightColor;

        UpdateStatus();
        CheckStartCondition();
    }

    public void SelectJjajangSauce()
    {
        if (CannotChangeSelection())
            return;

        selectedSauce = "Jjajang";

        sauceImage.color =
            new Color32(125, 75, 45, 222);

        RestoreSauceButtonColors();
        jjajangButton.image.color = selectedHighlightColor;

        UpdateStatus();
        CheckStartCondition();
    }

    public void ToggleFishCake()
    {
        if (CannotChangeSelection())
            return;

        fishCakeSelected = !fishCakeSelected;

        fishCakeImage.SetActive(fishCakeSelected);

        fishCakeButton.image.color =
            fishCakeSelected
                ? selectedHighlightColor
                : fishCakeNormalColor;

        UpdateStatus();
    }

    public void ToggleGreenOnion()
    {
        if (CannotChangeSelection())
            return;

        greenOnionSelected = !greenOnionSelected;

        greenOnionImage.SetActive(greenOnionSelected);

        greenOnionButton.image.color =
            greenOnionSelected
                ? selectedHighlightColor
                : greenOnionNormalColor;

        UpdateStatus();
    }

    public void ToggleCheese()
    {
        if (CannotChangeSelection())
            return;

        cheeseSelected = !cheeseSelected;

        cheeseImage.SetActive(cheeseSelected);

        cheeseButton.image.color =
            cheeseSelected
                ? selectedHighlightColor
                : cheeseNormalColor;

        UpdateStatus();
    }

    public void SelectMild()
    {
        if (CannotChangeSelection())
            return;

        selectedSpicy = "Mild";

        RestoreSpicyButtonColors();
        mildButton.image.color = selectedpepperHighlightColor;

        UpdateStatus();
        CheckStartCondition();
    }

    public void SelectMedium()
    {
        if (CannotChangeSelection())
            return;

        selectedSpicy = "Medium";

        RestoreSpicyButtonColors();
        mediumButton.image.color = selectedpepperHighlightColor;

        UpdateStatus();
        CheckStartCondition();
    }

    public void SelectSpicy()
    {
        if (CannotChangeSelection())
            return;

        selectedSpicy = "Spicy";

        RestoreSpicyButtonColors();
        spicyButton.image.color = selectedpepperHighlightColor;

        UpdateStatus();
        CheckStartCondition();
    }

    public void StartCooking()
    {
        if (heatGameRunning || cookingCompleted)
            return;

        if (string.IsNullOrEmpty(selectedSauce))
        {
            statusText.text = "소스를 선택하세요";
            return;
        }

        if (string.IsNullOrEmpty(selectedSpicy))
        {
            statusText.text = "맵기를 선택하세요";
            return;
        }

        SaveRecipeToGameData();

        SetSelectionButtonsInteractable(false);

        startCookingButton.interactable = false;

        heatGaugePanel.SetActive(true);

        ResetHeatMarker();

        fireButton.interactable = true;
        fireButtonText.text = "STOP FIRE";

        statusText.text = "불의 세기를 조절하세요.";
        potStatusText.text = "Heating...";

        //cookingEffect.SetActive(true);

        heatGameRunning = true;
    }

    private void ResetHeatMarker()
    {
        float halfHeight =
            heatGauge.rect.height / 2f;

        float markerHalf =
            heatMarker.rect.height / 2f;

        float bottom =
            -halfHeight + markerHalf;

        Vector2 position =
            heatMarker.anchoredPosition;

        position.y = bottom;

        heatMarker.anchoredPosition = position;

        markerDirection = 1f;
    }

    private void MoveHeatMarker()
    {
        float halfHeight =
            heatGauge.rect.height / 2f;

        float markerHalf =
            heatMarker.rect.height / 2f;

        float bottom =
            -halfHeight + markerHalf;

        float top =
            halfHeight - markerHalf;

        Vector2 position =
            heatMarker.anchoredPosition;

        position.y +=
            markerSpeed *
            markerDirection *
            Time.deltaTime;

        if (position.y >= top)
        {
            position.y = top;
            markerDirection = -1f;
        }
        else if (position.y <= bottom)
        {
            position.y = bottom;
            markerDirection = 1f;
        }

        heatMarker.anchoredPosition = position;
    }

    public void StopFire()
    {
        if (!heatGameRunning)
            return;

        heatGameRunning = false;

        fireButton.interactable = false;
        fireButtonText.text = "Fire OFF";

        //cookingEffect.SetActive(false);

        JudgeHeatResult();
    }

    private void JudgeHeatResult()
    {
        float gaugeHeight =
            heatGauge.rect.height;

        float halfHeight =
            gaugeHeight / 2f;

        float normalized =
            (heatMarker.anchoredPosition.y + halfHeight)
            / gaugeHeight;

        float perfectHalf =
            perfectZoneRatio / 2f;

        float perfectMin =
            0.5f - perfectHalf;

        float perfectMax =
            0.5f + perfectHalf;

        if (normalized < perfectMin)
        {
            GameData.HeatResult = "Under";

            statusText.text = "불 세기가 약해요";
            potStatusText.text = "Undercooked";
        }
        else if (normalized <= perfectMax)
        {
            GameData.HeatResult = "Perfect";

            statusText.text = "불 세기가 적당해요!";
            potStatusText.text = "Perfectly Cooked";
        }
        else
        {
            GameData.HeatResult = "Over";

            statusText.text = "불 세기가 강해요";
            potStatusText.text = "Overcooked";
        }

        StartCoroutine(FinishCookingRoutine());
    }

    private IEnumerator FinishCookingRoutine()
    {   
        fireEffect.SetActive(true);
        yield return new WaitForSeconds(1f);

        heatGaugePanel.SetActive(false);

        statusText.text = "떡볶이를 익히는 중...";

        //cookingSlider.value = 1f;

        float elapsedTime = 0f;
        while (elapsedTime < washDuration)
        {
            elapsedTime += Time.deltaTime;
            cookingSlider.value = elapsedTime / washDuration;
            yield return null;
        }

        cookingSlider.value = 1f;

        fireEffect.SetActive(false);

        cookingCompleted = true;

        statusText.text = "떡볶이 완성!";

        nextButton.gameObject.SetActive(true); 

    }

    private void SaveRecipeToGameData()
    {
        GameData.SelectedSauce = selectedSauce;

        GameData.HasFishCake = fishCakeSelected;
        GameData.HasGreenOnion = greenOnionSelected;
        GameData.HasCheese = cheeseSelected;

        GameData.SelectedSpicy = selectedSpicy;
    }

    public void GoToNextScene()
    {
        if (!cookingCompleted)
            return;

        SceneManager.LoadScene(nextSceneName);
    }

    private bool CannotChangeSelection()
    {
        return heatGameRunning || cookingCompleted;
    }

    private void CheckStartCondition()
    {
        bool sauceReady =
            !string.IsNullOrEmpty(selectedSauce);

        bool spicyReady =
            !string.IsNullOrEmpty(selectedSpicy);

        startCookingButton.interactable =
            sauceReady && spicyReady;
    }

    private void UpdateStatus()
    {
        string sauceText =
            string.IsNullOrEmpty(selectedSauce)
                ? "No Sauce"
                : selectedSauce;

        string spicyText =
            string.IsNullOrEmpty(selectedSpicy)
                ? "No Spicy Level"
                : selectedSpicy;

        //statusText.text =
            //sauceText + " / " + spicyText;

        potStatusText.text =
            GetIngredientText();
    }

    private string GetIngredientText()
    {
        string text = "Ingredients: ";

        bool hasIngredient = false;

        if (fishCakeSelected)
        {
            text += "Fish Cake ";
            hasIngredient = true;
        }

        if (greenOnionSelected)
        {
            text += "Green Onion ";
            hasIngredient = true;
        }

        if (cheeseSelected)
        {
            text += "Cheese ";
            hasIngredient = true;
        }

        if (!hasIngredient)
        {
            text += "None";
        }

        return text;
    }

    private void SaveOriginalButtonColors()
    {
        originalNormalColor = originalButton.image.color;
        creamNormalColor = creamButton.image.color;
        jjajangNormalColor = jjajangButton.image.color;

        fishCakeNormalColor = fishCakeButton.image.color;
        greenOnionNormalColor = greenOnionButton.image.color;
        cheeseNormalColor = cheeseButton.image.color;

        mildNormalColor = mildButton.image.color;
        mediumNormalColor = mediumButton.image.color;
        spicyNormalColor = spicyButton.image.color;
    }

    private void RestoreAllButtonColors()
    {
        RestoreSauceButtonColors();

        fishCakeButton.image.color = fishCakeNormalColor;
        greenOnionButton.image.color = greenOnionNormalColor;
        cheeseButton.image.color = cheeseNormalColor;

        RestoreSpicyButtonColors();
    }

    private void RestoreSauceButtonColors()
    {
        originalButton.image.color = originalNormalColor;
        creamButton.image.color = creamNormalColor;
        jjajangButton.image.color = jjajangNormalColor;
    }

    private void RestoreSpicyButtonColors()
    {
        mildButton.image.color = mildNormalColor;
        mediumButton.image.color = mediumNormalColor;
        spicyButton.image.color = spicyNormalColor;
    }

    private void SetSelectionButtonsInteractable(bool value)
    {
        originalButton.interactable = value;
        creamButton.interactable = value;
        jjajangButton.interactable = value;

        fishCakeButton.interactable = value;
        greenOnionButton.interactable = value;
        cheeseButton.interactable = value;

        mildButton.interactable = value;
        mediumButton.interactable = value;
        spicyButton.interactable = value;
    }
}