using System.Collections.Generic;
using UnityEngine;

public class ReviewEvaluator : MonoBehaviour
{
    public ReviewResult Evaluate(ReviewRecord record)
    {
        int mistakeCount = 0;

        List<string> comments =
            new List<string>();


        // ========================================
        // 소스
        // ========================================

        if (record.orderedFlavor != record.resultFlavor)
        {
            mistakeCount++;

            comments.Add(
                record.orderedFlavor+ "을 시켰는데 "+ record.resultFlavor+ "이 나왔어요."
            );
        }


        // ========================================
        // 떡 종류
        // ========================================

        if (record.orderedRiceCake !=
            record.resultRiceCake)
        {
            Debug.Log("주문: [" + record.orderedRiceCake + "] / 결과: [" + record.resultRiceCake + "]");
            mistakeCount++;

            comments.Add(
                "주문한 떡이 나오지 않았어요."
            );
        }


        // ========================================
        // 맵기
        // ========================================

        if (record.orderedSpicy !=
            record.resultSpicy)
        {
            mistakeCount++;

            comments.Add(
                "주문한 맵기로 나오지 않았어요."
            );
        }


        // ========================================
        // 어묵
        // ========================================

        CompareIngredient(
            record.orderedFishCake,
            record.resultFishCake,
            "어묵",
            comments,
            ref mistakeCount
        );


        // ========================================
        // 대파
        // ========================================

        CompareIngredient(
            record.orderedGreenOnion,
            record.resultGreenOnion,
            "대파",
            comments,
            ref mistakeCount
        );


        // ========================================
        // 치즈
        // ========================================

        CompareIngredient(
            record.orderedCheese,
            record.resultCheese,
            "치즈",
            comments,
            ref mistakeCount
        );


        // ========================================
        // 쿨피스
        // ========================================

        CompareIngredient(
            record.orderedCoolpis,
            record.resultCoolpis,
            "쿨피스",
            comments,
            ref mistakeCount
        );


        // ========================================
        // 계란찜
        // ========================================

        CompareIngredient(
            record.orderedSteamedEgg,
            record.resultSteamedEgg,
            "계란찜",
            comments,
            ref mistakeCount
        );


        // ========================================
        // 불 조절
        // ========================================

        if (record.heatResult == "Under")
        {
            mistakeCount++;

            comments.Add(
                "떡이 익혀지지 않았어요."
            );
        }
        else if (record.heatResult == "Over")
        {
            mistakeCount++;

            comments.Add(
                "떡이 너무 익었어요."
            );
        }


        // ========================================
        // 별점
        // ========================================

        int stars =
            Mathf.Clamp(
                5 - mistakeCount,
                1,
                5
            );


        string finalComment;

        if (mistakeCount == 0)
        {
            finalComment =
                "주문한 대로 배달되었어요!";
        }
        else
        {
            finalComment =
                string.Join(
                    " ",
                    comments
                );
        }


        return new ReviewResult(
            stars,
            finalComment
        );
    }


    private void CompareIngredient(
        bool ordered,
        bool result,
        string itemName,
        List<string> comments,
        ref int mistakeCount)
    {
        if (ordered == result)
            return;


        mistakeCount++;


        if (ordered && !result)
        {
            comments.Add(
                itemName
                + " 누락되었어요."
            );
        }
        else
        {
            comments.Add(itemName + " 주문하지 않았어요."
            );
        }
    }
}


public class ReviewResult
{
    public int stars;
    public string comment;


    public ReviewResult(
        int stars,
        string comment)
    {
        this.stars = stars;
        this.comment = comment;
    }
}