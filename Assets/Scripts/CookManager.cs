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

    [Header("Water and Fire")]
    [SerializeField] private Button waterButton;
    [SerializeField] private RectTransform waterFill;
    [SerializeField] private float maxWaterFillHeight = 180f;

    [SerializeField] private Button fireButton;
    [SerializeField] private TMP_Text fireButtonText;
    [SerializeField] private GameObject fireEffect;

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

    [Header("Cooking Settings")]
    [SerializeField] private float cookingDuration = 3f;
    [SerializeField] private string nextSceneName = "PackageScene";

    private const int MaxWaterLevel = 5;

    private string selectedSauce = "";
    private string selectedSpicy = "";

    private bool fishCakeSelected;
    private bool greenOnionSelected;
    private bool cheeseSelected;

    private int waterLevel;
    private bool fireOn;

    private bool isCooking;
    private bool cookingCompleted;

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

    private void Start()
    {
        SaveOriginalButtonColors();

        cookingSlider.minValue = 0f;
        cookingSlider.maxValue = 1f;
        cookingSlider.value = 0f;
        cookingSlider.interactable = false;

        statusText.text =
            "Select sauce, spicy level, water and fire";

        potStatusText.text =
            "Select sauce and ingredients";

        startCookingButton.interactable = false;
        nextButton.gameObject.SetActive(false);

        fishCakeImage.SetActive(false);
        greenOnionImage.SetActive(false);
        cheeseImage.SetActive(false);

        cookingEffect.SetActive(false);
        fireEffect.SetActive(false);

        sauceImage.color =
            new Color32(170, 210, 225, 255);

        ApplyRiceCakeSelection();

        waterLevel = 0;

        waterFill.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            0f
        );

        fireOn = false;

        fireButtonText.text = "Fire OFF";

        fireButton.image.color =
            new Color32(80, 80, 80, 255);

        RestoreAllButtonColors();
    }

    // ==================================================
    // Rice Cake from WashScene
    // ==================================================

    private void ApplyRiceCakeSelection()
    {
        if (GameData.SelectedRiceCake == "Wheat")
        {
            riceCakeImage.color =
                new Color32(228, 210, 165, 255);
        }
        else if (GameData.SelectedRiceCake == "Rice")
        {
            riceCakeImage.color =
                new Color32(248, 243, 230, 255);
        }
        else
        {
            riceCakeImage.color =
                new Color32(235, 235, 235, 255);
        }
    }

    // ==================================================
    // Sauce Selection
    // ==================================================

    public void SelectOriginalSauce()
    {
        if (CannotChangeSelection())
        {
            return;
        }

        selectedSauce = "Original";

        sauceImage.color =
            new Color32(230, 55, 45, 255);

        RestoreSauceButtonColors();
        originalButton.image.color =
            selectedHighlightColor;

        UpdateStatus();
        CheckCookingCondition();
    }

    public void SelectCreamSauce()
    {
        if (CannotChangeSelection())
        {
            return;
        }

        selectedSauce = "Cream";

        sauceImage.color =
            new Color32(255, 190, 110, 255);

        RestoreSauceButtonColors();
        creamButton.image.color =
            selectedHighlightColor;

        UpdateStatus();
        CheckCookingCondition();
    }

    public void SelectJjajangSauce()
    {
        if (CannotChangeSelection())
        {
            return;
        }

        selectedSauce = "Jjajang";

        sauceImage.color =
            new Color32(125, 75, 45, 255);

        RestoreSauceButtonColors();
        jjajangButton.image.color =
            selectedHighlightColor;

        UpdateStatus();
        CheckCookingCondition();
    }

    // ==================================================
    // Ingredient Toggle
    // ==================================================

    public void ToggleFishCake()
    {
        if (CannotChangeSelection())
        {
            return;
        }

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
        {
            return;
        }

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
        {
            return;
        }

        cheeseSelected = !cheeseSelected;

        cheeseImage.SetActive(cheeseSelected);

        cheeseButton.image.color =
            cheeseSelected
                ? selectedHighlightColor
                : cheeseNormalColor;

        UpdateStatus();
    }

    // ==================================================
    // Spicy Selection
    // ==================================================

    public void SelectMild()
    {
        if (CannotChangeSelection())
        {
            return;
        }

        selectedSpicy = "Mild";

        RestoreSpicyButtonColors();
        mildButton.image.color =
            selectedHighlightColor;

        UpdateStatus();
        CheckCookingCondition();
    }

    public void SelectMedium()
    {
        if (CannotChangeSelection())
        {
            return;
        }

        selectedSpicy = "Medium";

        RestoreSpicyButtonColors();
        mediumButton.image.color =
            selectedHighlightColor;

        UpdateStatus();
        CheckCookingCondition();
    }

    public void SelectSpicy()
    {
        if (CannotChangeSelection())
        {
            return;
        }

        selectedSpicy = "Spicy";

        RestoreSpicyButtonColors();
        spicyButton.image.color =
            selectedHighlightColor;

        UpdateStatus();
        CheckCookingCondition();
    }

    // ==================================================
    // Water
    // ==================================================

    public void AddWater()
    {
        if (CannotChangeSelection())
        {
            return;
        }

        if (waterLevel >= MaxWaterLevel)
        {
            return;
        }

        waterLevel++;

        float ratio =
            (float)waterLevel / MaxWaterLevel;

        float currentHeight =
            maxWaterFillHeight * ratio;

        waterFill.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            currentHeight
        );

        statusText.text =
            "Water: "
            + waterLevel
            + " / "
            + MaxWaterLevel;

        if (waterLevel >= MaxWaterLevel)
        {
            waterButton.interactable = false;
            statusText.text = "Water filled";
        }

        CheckCookingCondition();
    }

    // ==================================================
    // Fire
    // ==================================================

    public void ToggleFire()
    {
        if (CannotChangeSelection())
        {
            return;
        }

        fireOn = !fireOn;

        fireEffect.SetActive(fireOn);

        if (fireOn)
        {
            fireButtonText.text = "Fire ON";

            fireButton.image.color =
                new Color32(255, 100, 40, 255);
        }
        else
        {
            fireButtonText.text = "Fire OFF";

            fireButton.image.color =
                new Color32(80, 80, 80, 255);
        }

        CheckCookingCondition();
    }

    // ==================================================
    // Cooking
    // ==================================================

    public void StartCooking()
    {
        if (isCooking || cookingCompleted)
        {
            return;
        }

        if (string.IsNullOrEmpty(selectedSauce))
        {
            statusText.text = "Select sauce first";
            return;
        }

        if (string.IsNullOrEmpty(selectedSpicy))
        {
            statusText.text =
                "Select spicy level first";

            return;
        }

        if (waterLevel < MaxWaterLevel)
        {
            statusText.text = "Fill the water first";
            return;
        }

        if (!fireOn)
        {
            statusText.text = "Turn on the fire";
            return;
        }

        StartCoroutine(CookingRoutine());
    }

    private IEnumerator CookingRoutine()
    {
        isCooking = true;

        SetAllSelectionButtonsInteractable(false);

        waterButton.interactable = false;
        fireButton.interactable = false;
        startCookingButton.interactable = false;

        cookingEffect.SetActive(true);

        statusText.text = "Cooking...";
        potStatusText.text = "Cooking Tteokbokki...";

        float elapsedTime = 0f;

        while (elapsedTime < cookingDuration)
        {
            elapsedTime += Time.deltaTime;

            cookingSlider.value =
                elapsedTime / cookingDuration;

            yield return null;
        }

        cookingSlider.value = 1f;

        cookingEffect.SetActive(false);
        fireEffect.SetActive(false);

        fireOn = false;
        fireButtonText.text = "Fire OFF";

        isCooking = false;
        cookingCompleted = true;

        statusText.text = "Cooking Complete";
        potStatusText.text = GetFinalRecipeText();

        nextButton.gameObject.SetActive(true);
    }

    // ==================================================
    // Scene Change
    // ==================================================

    public void GoToNextScene()
    {
        if (!cookingCompleted)
        {
            return;
        }

        SceneManager.LoadScene(nextSceneName);
    }

    // ==================================================
    // Condition and Status
    // ==================================================

    private bool CannotChangeSelection()
    {
        return isCooking || cookingCompleted;
    }

    private void CheckCookingCondition()
    {
        bool sauceReady =
            !string.IsNullOrEmpty(selectedSauce);

        bool spicyReady =
            !string.IsNullOrEmpty(selectedSpicy);

        bool waterReady =
            waterLevel >= MaxWaterLevel;

        bool fireReady =
            fireOn;

        startCookingButton.interactable =
            sauceReady
            && spicyReady
            && waterReady
            && fireReady;
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

        statusText.text =
            sauceText + " / " + spicyText;

        potStatusText.text =
            GetCurrentIngredientText();
    }

    private string GetCurrentIngredientText()
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

    private string GetFinalRecipeText()
    {
        string result =
            selectedSauce
            + " / "
            + selectedSpicy;

        result +=
            GameData.SelectedRiceCake == "Wheat"
                ? " / Wheat Rice Cake"
                : " / Rice Rice Cake";

        if (fishCakeSelected)
        {
            result += " / Fish Cake";
        }

        if (greenOnionSelected)
        {
            result += " / Green Onion";
        }

        if (cheeseSelected)
        {
            result += " / Cheese";
        }

        return result;
    }

    // ==================================================
    // Button Colors
    // ==================================================

    private void SaveOriginalButtonColors()
    {
        originalNormalColor =
            originalButton.image.color;

        creamNormalColor =
            creamButton.image.color;

        jjajangNormalColor =
            jjajangButton.image.color;

        fishCakeNormalColor =
            fishCakeButton.image.color;

        greenOnionNormalColor =
            greenOnionButton.image.color;

        cheeseNormalColor =
            cheeseButton.image.color;

        mildNormalColor =
            mildButton.image.color;

        mediumNormalColor =
            mediumButton.image.color;

        spicyNormalColor =
            spicyButton.image.color;
    }

    private void RestoreAllButtonColors()
    {
        RestoreSauceButtonColors();

        fishCakeButton.image.color =
            fishCakeNormalColor;

        greenOnionButton.image.color =
            greenOnionNormalColor;

        cheeseButton.image.color =
            cheeseNormalColor;

        RestoreSpicyButtonColors();
    }

    private void RestoreSauceButtonColors()
    {
        originalButton.image.color =
            originalNormalColor;

        creamButton.image.color =
            creamNormalColor;

        jjajangButton.image.color =
            jjajangNormalColor;
    }

    private void RestoreSpicyButtonColors()
    {
        mildButton.image.color =
            mildNormalColor;

        mediumButton.image.color =
            mediumNormalColor;

        spicyButton.image.color =
            spicyNormalColor;
    }

    // ==================================================
    // Button Lock
    // ==================================================

    private void SetAllSelectionButtonsInteractable(
        bool value
    )
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