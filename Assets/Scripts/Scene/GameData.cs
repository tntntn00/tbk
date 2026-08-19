public static class GameData
{
    // ========================================
    // 현재 날짜
    // ========================================

    public static int CurrentDay = 1;


    // ========================================
    // 고객이 주문한 내용
    // 지금은 테스트값
    // 나중에 주문 생성 시스템이 자동으로 채움
    // ========================================

    public static string OrderedRiceCake = "Wheat";
    public static string OrderedSauce = "Original";
    public static string OrderedSpicy = "Spicy";

    public static bool OrderedFishCake = true;
    public static bool OrderedGreenOnion = true;
    public static bool OrderedCheese = true;

    public static bool OrderedColpis = true;
    public static bool OrderedSteamedEgg = false;


    // ========================================
    // WashScene - 플레이어가 실제 선택한 떡
    // ========================================

    public static string SelectedRiceCake = "";


    // ========================================
    // CookScene - 플레이어가 실제 만든 음식
    // ========================================

    public static string SelectedSauce = "";
    public static string SelectedSpicy = "";

    public static bool HasFishCake = false;
    public static bool HasGreenOnion = false;
    public static bool HasCheese = false;


    // ========================================
    // 불 조절 결과
    // "Under", "Perfect", "Over"
    // ========================================

    public static string HeatResult = "";


    // ========================================
    // PackageScene - 실제 포장 결과
    // ========================================

    public static bool HasColpis = false;
    public static bool HasSteamedEgg = false;


    // ========================================
    // 다음 주문을 위한 플레이 결과 초기화
    // 주문 내용은 초기화하지 않음
    // ========================================

    public static void ResetPlayerResult()
    {
        SelectedRiceCake = "";

        SelectedSauce = "";
        SelectedSpicy = "";

        HasFishCake = false;
        HasGreenOnion = false;
        HasCheese = false;

        HeatResult = "";

        HasColpis = false;
        HasSteamedEgg = false;
    }
}