using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class ReviewManager : MonoBehaviour
{
    [Header("Store")]
    [SerializeField] private TMP_Text storeNameText;
    [Header("Review List")]
    [SerializeField] private RectTransform reviewContent;
    [SerializeField] private ReviewItemUI reviewItemPrefab;

    [Header("Evaluator")]
    [SerializeField] private ReviewEvaluator reviewEvaluator;

    [Header("Review Layout")]
    [SerializeField] private float startY = -20f;
    [SerializeField] private float reviewSpacing = 240f;

    private readonly List<ReviewItemUI> reviewItems =
        new List<ReviewItemUI>();

    private void Start()
    {
        UpdateStoreName();
        CreateDeliveryReview();
    }
    private void UpdateStoreName()
    {
        if (string.IsNullOrEmpty(PlayerNameManager.PlayerName))
        {
            storeNameText.text = "떡볶이";
            return;
        }

        storeNameText.text =
            PlayerNameManager.PlayerName + "의 떡볶이";
    }
    public void GoBackToMain()
    {
        SceneManager.LoadScene("MainScene");
    }
    private void CreateDeliveryReview()
    {
        ReviewResult result =
            reviewEvaluator.Evaluate();

        string orderText =
            BuildOrderText();

        AddReview(
            GameData.CurrentDay,
            result.stars,
            orderText,
            result.comment
        );
    }

    private string BuildOrderText()
    {
        List<string> parts =
            new List<string>();

        parts.Add(GameData.OrderedSauce);
        parts.Add(GameData.OrderedRiceCake);
        parts.Add(GameData.OrderedSpicy);

        if (GameData.OrderedFishCake)
            parts.Add("Fish Cake");

        if (GameData.OrderedGreenOnion)
            parts.Add("Green Onion");

        if (GameData.OrderedCheese)
            parts.Add("Cheese");

        if (GameData.OrderedColpis)
            parts.Add("Colpis");

        if (GameData.OrderedSteamedEgg)
            parts.Add("Steamed Egg");

        return string.Join(
            " / ",
            parts
        );
    }

    public void AddReview(
        int day,
        int stars,
        string order,
        string comment)
    {
        for (int i = 0;
             i < reviewItems.Count;
             i++)
        {
            RectTransform oldRect =
                reviewItems[i]
                .GetComponent<RectTransform>();

            Vector2 position =
                oldRect.anchoredPosition;

            position.y -= reviewSpacing;

            oldRect.anchoredPosition =
                position;
        }

        ReviewItemUI newReview =
            Instantiate(
                reviewItemPrefab,
                reviewContent
            );

        RectTransform newRect =
            newReview
            .GetComponent<RectTransform>();

        newRect.anchoredPosition =
            new Vector2(
                0f,
                startY
            );

        newReview.Setup(
            day,
            stars,
            order,
            comment
        );

        reviewItems.Insert(
            0,
            newReview
        );

        UpdateContentHeight();
    }

    private void UpdateContentHeight()
    {
        RectTransform viewport =
            reviewContent.parent
            as RectTransform;

        float minimumHeight =
            viewport.rect.height;

        float neededHeight =
            Mathf.Abs(startY)
            + reviewItems.Count
            * reviewSpacing
            + 20f;

        reviewContent.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            Mathf.Max(
                minimumHeight,
                neededHeight
            )
        );
    }
}