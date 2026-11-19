using System.Collections.Generic;

[System.Serializable]
public class ReviewRecord
{
    // ========================================
    // 기본 정보
    // ========================================

    public int day;
    public int orderNumber;


    // ========================================
    // 고객이 주문한 내용
    // ========================================

    public string orderedFlavor;
    public string orderedRiceCake;
    public string orderedSpicy;

    public bool orderedFishCake;
    public bool orderedGreenOnion;
    public bool orderedCheese;

    public bool orderedCoolpis;
    public bool orderedSteamedEgg;


    // ========================================
    // 플레이어가 실제 만든 결과
    // ========================================

    public string resultFlavor;
    public string resultRiceCake;
    public string resultSpicy;

    public bool resultFishCake;
    public bool resultGreenOnion;
    public bool resultCheese;

    public bool resultCoolpis;
    public bool resultSteamedEgg;

    public string heatResult;
    public string status = "정상처리";
}


public static class ReviewHistory
{
    // 완료된 모든 주문 기록
    public static readonly List<ReviewRecord> Records =
        new List<ReviewRecord>();


    // ========================================
    // 배달 완료된 주문 저장
    // ========================================

    public static void AddCompletedOrder(BillData bill)
    {
        if (bill == null)
            return;


        ReviewRecord record =
            new ReviewRecord();


        // 기본 정보
        record.day =
            GameData.CurrentDay;

        record.orderNumber =
            bill.orderNumber;


        // ========================================
        // 고객 주문 내용
        // BillData 기준
        // ========================================

        record.orderedFlavor =
            bill.flavor;

        record.orderedRiceCake =
            bill.riceCake;

        record.orderedSpicy =
            bill.spicy;

        record.orderedFishCake =
            bill.fishCake;

        record.orderedGreenOnion =
            bill.greenOnion;

        record.orderedCheese =
            bill.cheese;

        record.orderedCoolpis =
            bill.coolpis;

        record.orderedSteamedEgg =
            bill.steamedEgg;


        // ========================================
        // 실제 플레이 결과
        // GameData 기준
        // ========================================

        record.resultFlavor =
            GameData.SelectedSauce;

        record.resultRiceCake =
            GameData.SelectedRiceCake;

        record.resultSpicy =
            GameData.SelectedSpicy;

        record.resultFishCake =
            GameData.HasFishCake;

        record.resultGreenOnion =
            GameData.HasGreenOnion;

        record.resultCheese =
            GameData.HasCheese;

        record.resultCoolpis =
            GameData.HasColpis;

        record.resultSteamedEgg =
            GameData.HasSteamedEgg;

        record.heatResult =
            GameData.HeatResult;


        Records.Add(record);


        UnityEngine.Debug.Log(
            "REVIEW RECORD 저장 완료 - ORDER #"
            + bill.orderNumber
            + " / 현재 리뷰 기록 수: "
            + Records.Count
        );
    }

    public static void AddCanceledOrder(BillData bill)
{
    if (bill == null)
        return;

    ReviewRecord record = new ReviewRecord();

    record.day = GameData.CurrentDay;
    record.orderNumber = bill.orderNumber;
    record.status = "주문 취소";

    Records.Add(record);
}
}