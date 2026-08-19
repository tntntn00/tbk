using System.Collections.Generic;
using UnityEngine;

public class ReviewEvaluator : MonoBehaviour
{
    public ReviewResult Evaluate()
    {
        int mistakeCount = 0;

        List<string> comments =
            new List<string>();


        // ========================================
        // 떡 종류
        // ========================================

        if (GameData.OrderedRiceCake !=
            GameData.SelectedRiceCake)
        {
            mistakeCount++;

            comments.Add(
                "I ordered " +
                GameData.OrderedRiceCake +
                " rice cake, but got " +
                GameData.SelectedRiceCake + "."
            );
        }


        // ========================================
        // 소스
        // ========================================

        if (GameData.OrderedSauce !=
            GameData.SelectedSauce)
        {
            mistakeCount++;

            comments.Add(
                "I ordered " +
                GameData.OrderedSauce +
                ", but got " +
                GameData.SelectedSauce + "."
            );
        }


        // ========================================
        // 맵기
        // ========================================

        if (GameData.OrderedSpicy !=
            GameData.SelectedSpicy)
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
            GameData.OrderedFishCake,
            GameData.HasFishCake,
            "Fish cake",
            comments,
            ref mistakeCount
        );


        // ========================================
        // 대파
        // ========================================

        CompareIngredient(
            GameData.OrderedGreenOnion,
            GameData.HasGreenOnion,
            "Green onion",
            comments,
            ref mistakeCount
        );


        // ========================================
        // 치즈
        // ========================================

        CompareIngredient(
            GameData.OrderedCheese,
            GameData.HasCheese,
            "Cheese",
            comments,
            ref mistakeCount
        );


        // ========================================
        // 쿨피스
        // ========================================

        CompareIngredient(
            GameData.OrderedColpis,
            GameData.HasColpis,
            "Colpis",
            comments,
            ref mistakeCount
        );


        // ========================================
        // 계란찜
        // ========================================

        CompareIngredient(
            GameData.OrderedSteamedEgg,
            GameData.HasSteamedEgg,
            "Steamed egg",
            comments,
            ref mistakeCount
        );


        // ========================================
        // 불 조절
        // ========================================

        if (GameData.HeatResult == "Under")
        {
            mistakeCount++;

            comments.Add(
                "The food was undercooked."
            );
        }
        else if (GameData.HeatResult == "Over")
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


        // ========================================
        // 리뷰 문장
        // ========================================

        string finalComment;

        if (mistakeCount == 0)
        {
            finalComment =
                "Everything was perfect!";
        }
        else
        {
            finalComment =
                string.Join(" ", comments);
        }


        return new ReviewResult(
            stars,
            finalComment
        );
    }


    private void CompareIngredient(
        bool ordered,
        bool actual,
        string ingredientName,
        List<string> comments,
        ref int mistakeCount)
    {
        if (ordered == actual)
            return;

        mistakeCount++;

        if (ordered && !actual)
        {
            comments.Add(
                ingredientName +
                " was missing."
            );
        }
        else
        {
            comments.Add(
                "I didn't order " +
                ingredientName + "."
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