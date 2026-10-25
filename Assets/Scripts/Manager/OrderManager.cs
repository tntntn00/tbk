using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance { get; private set; }

    [Header("Bill UI")]
    [SerializeField] private RectTransform billArea;
    [SerializeField] private BillItemUI billItemPrefab;

    [Header("Day 1 Settings")]
    [SerializeField] private int maxOrdersPerDay = 5;
    [SerializeField] private float orderSpawnInterval = 8f;
    [SerializeField] private float orderTimeLimit = 60f;

    [Header("Bill Spacing")]
    [SerializeField] private float billSpacingX = 300f;

    private readonly List<BillData> activeBills =
        new List<BillData>();

    private readonly List<BillItemUI> activeBillUIs =
        new List<BillItemUI>();

    private int nextOrderNumber = 1;
    private int createdOrderCount = 0;

    private bool orderSystemStarted = false;

    // 첫 번째 BillItem이 원래 가지고 있던 위치 저장
    private Vector2 billStartPosition;
    private bool hasBillStartPosition = false;


    // ========================================
    // DontDestroyOnLoad
    // ========================================

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }


    // ========================================
    // Update
    // ========================================

    private void Update()
    {
        if (!orderSystemStarted)
            return;

        UpdateBillTimers();
    }


    // ========================================
    // 이름 입력 완료 후 주문 시스템 시작
    // ========================================

    public void StartOrderSystem()
    {
        if (orderSystemStarted)
            return;

        orderSystemStarted = true;

        StartCoroutine(
            SpawnOrdersRoutine()
        );
    }


    // ========================================
    // BillArea 표시 / 숨김
    // ========================================

    public void SetBillAreaVisible(bool visible)
    {
        if (billArea == null)
            return;

        billArea.gameObject.SetActive(visible);
    }


    // ========================================
    // Day 1 주문 생성
    // ========================================

    private IEnumerator SpawnOrdersRoutine()
    {
        while (createdOrderCount < maxOrdersPerDay)
        {
            CreateRandomBill();

            createdOrderCount++;

            if (createdOrderCount >= maxOrdersPerDay)
                break;

            yield return new WaitForSeconds(
                orderSpawnInterval
            );
        }
    }


    // ========================================
    // 랜덤 주문 생성
    // ========================================

    private void CreateRandomBill()
    {
        string[] flavors =
        {
            "오리지널",
            "크림",
            "짜장"
        };

        string[] riceCakes =
        {
            "밀떡",
            "쌀떡"
        };

        string[] spicyLevels =
        {
            "순한 맛",
            "보통 맛",
            "매운 맛"
        };

        BillData newBill =
            new BillData(
                nextOrderNumber,

                flavors[
                    Random.Range(
                        0,
                        flavors.Length
                    )
                ],

                riceCakes[
                    Random.Range(
                        0,
                        riceCakes.Length
                    )
                ],

                spicyLevels[
                    Random.Range(
                        0,
                        spicyLevels.Length
                    )
                ],

                Random.value > 0.5f, // Fish Cake
                Random.value > 0.5f, // Green Onion
                Random.value > 0.5f, // Cheese

                Random.value > 0.5f, // Coolpis
                Random.value > 0.5f, // Steamed Egg

                orderTimeLimit
            );

        nextOrderNumber++;

        AddBill(newBill);
    }


    // ========================================
    // Bill UI 생성
    // ========================================

    private void AddBill(BillData bill)
    {
        activeBills.Add(bill);

        BillItemUI newBillUI =
            Instantiate(
                billItemPrefab,
                billArea
            );

        RectTransform billRect =
            newBillUI.GetComponent<RectTransform>();


        // 첫 번째 생성된 Bill의 원래 프리팹 위치 저장
        if (!hasBillStartPosition)
        {
            billStartPosition =
                billRect.anchoredPosition;

            hasBillStartPosition = true;
        }


        int index =
            activeBillUIs.Count;


        // X값만 이동
        // Y / Anchor / Pivot은 건드리지 않음
        billRect.anchoredPosition =
            new Vector2(
                billStartPosition.x
                - (index * billSpacingX),

                billStartPosition.y
            );


        string ingredientText =
            MakeIngredientText(bill);

        string sideMenuText =
            MakeSideMenuText(bill);


        newBillUI.Setup(
            bill.orderNumber,
            bill.flavor,
            bill.riceCake,
            bill.spicy,
            ingredientText,
            sideMenuText,
            bill.maxTime
        );


        activeBillUIs.Add(newBillUI);
    }


    // ========================================
    // 모든 주문의 제한시간 감소
    // ========================================

    private void UpdateBillTimers()
    {
        for (int i = activeBills.Count - 1;
             i >= 0;
             i--)
        {
            BillData bill =
                activeBills[i];


            if (bill.isCompleted ||
                bill.isExpired)
            {
                continue;
            }


            bill.remainingTime -=
                Time.deltaTime;


            if (bill.remainingTime < 0f)
            {
                bill.remainingTime = 0f;
            }


            activeBillUIs[i]
                .UpdateTimer(
                    bill.remainingTime
                );


            if (bill.remainingTime <= 0f)
            {
                ExpireBill(i);
            }
        }
    }


    // ========================================
    // 시간 초과 주문 처리
    // ========================================

    private void ExpireBill(int index)
    {
        BillData expiredBill =
            activeBills[index];

        expiredBill.isExpired = true;


        Debug.Log(
            "ORDER #"
            + expiredBill.orderNumber
            + " 시간 초과"
        );


        if (activeBillUIs[index] != null)
        {
            Destroy(
                activeBillUIs[index].gameObject
            );
        }


        activeBillUIs.RemoveAt(index);
        activeBills.RemoveAt(index);


        RepositionBills();

        // 나중에 여기서
        // 시간 초과 리뷰 1점 저장
        // 실패 주문 카운트 증가
        // Day 정산에 반영
    }


    // ========================================
    // 배달 완료 주문 처리
    //
    // 중요:
    // 예전에는 void였지만
    // 이제 완료한 BillData를 반환한다.
    // ========================================

    public BillData CompleteCurrentOrder()
    {
        if (activeBills.Count <= 0)
        {
            Debug.LogWarning(
                "완료할 주문이 없습니다."
            );

            return null;
        }


        // 주문 선택 기능은 사용하지 않으므로
        // 가장 오래된 주문부터 처리
        int index = 0;


        BillData completedBill =
            activeBills[index];


        completedBill.isCompleted = true;


        Debug.Log(
            "ORDER #"
            + completedBill.orderNumber
            + " 완료"
        );


        // 화면에서 Bill 삭제
        if (activeBillUIs[index] != null)
        {
            Destroy(
                activeBillUIs[index].gameObject
            );
        }


        // OrderManager의 활성 리스트에서도 제거
        activeBillUIs.RemoveAt(index);
        activeBills.RemoveAt(index);


        // 남아 있는 주문서 위치 재정렬
        RepositionBills();


        // ★ 핵심
        // 방금 완료한 주문 데이터를 반환
        return completedBill;
    }


    // ========================================
    // 주문 하나 제거 후 Bill 재정렬
    // ========================================

    private void RepositionBills()
    {
        if (!hasBillStartPosition)
            return;


        for (int i = 0;
             i < activeBillUIs.Count;
             i++)
        {
            RectTransform rect =
                activeBillUIs[i]
                .GetComponent<RectTransform>();


            Vector2 currentPosition =
                rect.anchoredPosition;


            // X만 다시 정렬
            // Y는 현재 위치 그대로 유지
            currentPosition.x =
                billStartPosition.x
                - (i * billSpacingX);


            rect.anchoredPosition =
                currentPosition;
        }
    }

    public void CancelBill(BillItemUI ui)
        {
    int index = activeBillUIs.IndexOf(ui);

    if (index < 0)
        return;

    Debug.Log("ORDER #" + activeBills[index].orderNumber + " 취소");

    Destroy(ui.gameObject);

    activeBillUIs.RemoveAt(index);
    activeBills.RemoveAt(index);

    RepositionBills();
    }


    // ========================================
    // 재료 Text 생성
    // ========================================

    private string MakeIngredientText(
        BillData bill)
    {
        string result = "";


        if (bill.fishCake)
        {
            result += "어묵";
        }


        if (bill.greenOnion)
        {
            if (result !="")
            {
                result += " / ";
            }
            result += "대파";
        }

        if (bill.cheese)
        {
            if (result != "")
            {
                result += " / ";
            }
            result += "치즈";
        }


        if (string.IsNullOrEmpty(result))
        {
            result = "---";
        }


        return result.TrimEnd();
    }


    // ========================================
    // 사이드 메뉴 Text 생성
    // ========================================

    private string MakeSideMenuText(
        BillData bill)
    {
        string result = "";


        if (bill.coolpis)
        {
            result += "쿨피스";
        }


        if (bill.steamedEgg)
        {
            if (result !="")
            {
                result += " / ";
            }
            result += "계란찜";
        }


        if (string.IsNullOrEmpty(result))
        {
            result = "---";
        }


        return result.TrimEnd();
    }
}