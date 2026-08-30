using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    [SerializeField] private float reviewSpacing = 250f;

    private readonly List<ReviewItemUI> reviewItems =
        new List<ReviewItemUI>();


    private void Start()
    {
        UpdateStoreName();

        RefreshReviews();
    }


    // ========================================
    // 가게 이름
    // ========================================

    private void UpdateStoreName()
    {
        if (string.IsNullOrEmpty(
            PlayerNameManager.PlayerName))
        {
            storeNameText.text =
                "떡볶이";

            return;
        }

        storeNameText.text =
            PlayerNameManager.PlayerName
            + "의 떡볶이 가게";
    }


    // ========================================
    // MainScene으로 복귀
    // ========================================

    public void GoBackToMain()
    {
        SceneManager.LoadScene(
            "MainScene"
        );
    }


    // ========================================
    // 리뷰 화면 전체 갱신
    // ========================================

    public void RefreshReviews()
    {
        // 기존에 생성되어 있던 리뷰 삭제
        for (int i = 0;
             i < reviewItems.Count;
             i++)
        {
            if (reviewItems[i] != null)
            {
                Destroy(
                    reviewItems[i].gameObject
                );
            }
        }

        reviewItems.Clear();


        // ========================================
        // 오래된 리뷰부터 생성
        //
        // AddReview에서
        // 기존 리뷰들을 아래로 밀기 때문에
        // 최종적으로 최신 리뷰가 맨 위에 온다.
        // ========================================

        for (int i = 0;
             i < ReviewHistory.Records.Count;
             i++)
        {
            ReviewRecord record =
                ReviewHistory.Records[i];

            ReviewResult result =
                reviewEvaluator.Evaluate(
                    record
                );

            string orderText =
                BuildOrderText(
                    record
                );

            AddReview(
                record.day,
                result.stars,
                orderText,
                result.comment
            );
        }


        Debug.Log(
            "리뷰 갱신 완료 / "
            + reviewItems.Count
            + "개"
        );
    }


    // ========================================
    // 새 리뷰 생성
    // ========================================

    private void AddReview(
        int day,
        int stars,
        string order,
        string comment)
    {
        // ----------------------------------------
        // 1. 기존 리뷰들을 전부 아래로 내린다.
        // ----------------------------------------

        for (int i = 0;
             i < reviewItems.Count;
             i++)
        {
            RectTransform oldRect =
                reviewItems[i]
                .GetComponent<RectTransform>();

            Vector2 position =
                oldRect.anchoredPosition;

            // X는 절대 건드리지 않음
            // Y만 아래로 이동
            position.y -= reviewSpacing;

            oldRect.anchoredPosition =
                position;
        }


        // ----------------------------------------
        // 2. 새로운 리뷰 생성
        //
        // 위치는 ReviewItem Prefab의
        // 원래 위치를 그대로 사용한다.
        // ----------------------------------------

        ReviewItemUI newReview =
            Instantiate(
                reviewItemPrefab,
                reviewContent
            );


        newReview.Setup(
            day,
            stars,
            order,
            comment
        );


        // 최신 리뷰를 리스트 0번에 저장
        reviewItems.Insert(
            0,
            newReview
        );


        // ----------------------------------------
        // 3. Content 높이만 늘린다.
        // ----------------------------------------

        UpdateContentHeight();
    }


    // ========================================
    // 주문 내용 표시
    // ========================================

    private string BuildOrderText(
        ReviewRecord record)
    {
        List<string> parts =
            new List<string>();


        parts.Add(
            record.orderedFlavor
        );

        parts.Add(
            record.orderedRiceCake
        );

        parts.Add(
            record.orderedSpicy
        );


        if (record.orderedFishCake)
            parts.Add("어묵");


        if (record.orderedGreenOnion)
            parts.Add("대파");


        if (record.orderedCheese)
            parts.Add("치즈");


        if (record.orderedCoolpis)
            parts.Add("쿨피스");


        if (record.orderedSteamedEgg)
            parts.Add("계란찜");


        return string.Join(
            " / ",
            parts
        );
    }


    // ========================================
    // Scroll Content 높이 조정
    // ========================================

    private void UpdateContentHeight()
    {
        if (reviewContent == null)
            return;


        RectTransform viewport =
            reviewContent.parent
            as RectTransform;


        if (viewport == null)
            return;


        float minimumHeight =
            viewport.rect.height;


        float requiredHeight =
            reviewItems.Count
            * reviewSpacing;


        reviewContent.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,

            Mathf.Max(
                minimumHeight,
                requiredHeight
            )
        );
    }
}