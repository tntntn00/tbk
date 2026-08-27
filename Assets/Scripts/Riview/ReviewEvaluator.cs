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
                "I ordered "
                + record.orderedFlavor
                + ", but got "
                + record.resultFlavor
                + "."
            );
        }


        // ========================================
        // 떡 종류
        // ========================================

        if (record.orderedRiceCake !=
            record.resultRiceCake)
        {
            mistakeCount++;

            comments.Add(
                "The rice cake type was wrong."
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
                "The spicy level was wrong."
            );
        }


        // ========================================
        // 어묵
        // ========================================

        CompareIngredient(
            record.orderedFishCake,
            record.resultFishCake,
            "Fish cake",
            comments,
            ref mistakeCount
        );


        // ========================================
        // 대파
        // ========================================

        CompareIngredient(
            record.orderedGreenOnion,
            record.resultGreenOnion,
            "Green onion",
            comments,
            ref mistakeCount
        );


        // ========================================
        // 치즈
        // ========================================

        CompareIngredient(
            record.orderedCheese,
            record.resultCheese,
            "Cheese",
            comments,
            ref mistakeCount
        );


        // ========================================
        // 쿨피스
        // ========================================

        CompareIngredient(
            record.orderedCoolpis,
            record.resultCoolpis,
            "Coolpis",
            comments,
            ref mistakeCount
        );


        // ========================================
        // 계란찜
        // ========================================

        CompareIngredient(
            record.orderedSteamedEgg,
            record.resultSteamedEgg,
            "Steamed egg",
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
                "The food was undercooked."
            );
        }
        else if (record.heatResult == "Over")
        {
            mistakeCount++;

            comments.Add(
                "The food was overcooked."
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
                "Everything was perfect!";
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
                + " was missing."
            );
        }
        else
        {
            comments.Add(
                "I didn't order "
                + itemName
                + "."
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