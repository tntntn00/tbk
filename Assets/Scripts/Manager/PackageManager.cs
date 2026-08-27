using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PackageManager : MonoBehaviour
{
    [Header("Main Package Buttons")]
    [SerializeField] private Button TbkButton;
    [SerializeField] private Button packageBoxButton;
    [SerializeField] private Button plasticBagButton;

    [Header("Side Menu")]
    [SerializeField] private Button colpisButton;
    [SerializeField] private Button eggButton;

    [Header("Delivery")]
    [SerializeField] private DeliveryHandPopup deliveryHandPopup;
    [SerializeField] private Button deliveryButton;

    [Header("UI")]
    [SerializeField] private TMP_Text statusText;

    [Header("Movement Settings")]
    [SerializeField] private float moveDuration = 0.4f;
    [SerializeField] private float foodInBoxScale = 0.55f;
    [SerializeField] private float sideMenuInBoxScale = 0.7f;
    [SerializeField] private float boxInBagScale = 0.75f;


    private RectTransform foodRect;
    private RectTransform boxRect;
    private RectTransform bagRect;

    private RectTransform colpisRect;
    private RectTransform eggRect;


    private bool foodPacked;
    private bool colpisPacked;
    private bool eggPacked;

    private bool bagPacked;
    private bool readyForDelivery;

    private bool isMoving;


    // ========================================
    // 시작
    // ========================================

    private void Start()
    {
        foodRect =
            TbkButton.GetComponent<RectTransform>();

        boxRect =
            packageBoxButton.GetComponent<RectTransform>();

        bagRect =
            plasticBagButton.GetComponent<RectTransform>();

        colpisRect =
            colpisButton.GetComponent<RectTransform>();

        eggRect =
            eggButton.GetComponent<RectTransform>();


        TbkButton.interactable = true;
        colpisButton.interactable = true;
        eggButton.interactable = true;

        packageBoxButton.interactable = false;
        plasticBagButton.interactable = false;

        deliveryButton.gameObject.SetActive(true);
        deliveryButton.interactable = false;


        TbkButton.image.raycastTarget = true;
        colpisButton.image.raycastTarget = true;
        eggButton.image.raycastTarget = true;

        packageBoxButton.image.raycastTarget = true;
        plasticBagButton.image.raycastTarget = true;


        statusText.text =
            "음식을 포장 용기에 넣으세요";
    }


    // ========================================
    // 떡볶이 포장
    // ========================================

    public void PutFoodIntoContainer()
    {
        if (foodPacked || isMoving)
            return;

        StartCoroutine(
            MoveFoodToContainerRoutine()
        );
    }


    private IEnumerator MoveFoodToContainerRoutine()
    {
        isMoving = true;

        TbkButton.interactable = false;

        statusText.text =
            "포장 중...";


        Vector2 startPosition =
            foodRect.position;

        Vector2 targetPosition =
            boxRect.position;


        Vector3 startScale =
            foodRect.localScale;

        Vector3 targetScale =
            Vector3.one * foodInBoxScale;


        float elapsedTime = 0f;


        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;

            float t =
                elapsedTime / moveDuration;


            foodRect.position =
                Vector2.Lerp(
                    startPosition,
                    targetPosition,
                    t
                );


            foodRect.localScale =
                Vector3.Lerp(
                    startScale,
                    targetScale,
                    t
                );


            yield return null;
        }


        foodRect.position =
            targetPosition;

        foodRect.localScale =
            targetScale;


        foodPacked = true;

        isMoving = false;


        TbkButton.image.raycastTarget = false;


        packageBoxButton.interactable = true;


        if (eggPacked && colpisPacked)
        {
            statusText.text =
                "비닐에 옮겨 포장하세요";
        }
        else
        {
            statusText.text =
                "사이드 메뉴를 포장하세요";
        }
    }


    // ========================================
    // 쿨피스
    // ========================================

    public void PutColpisIntoContainer()
    {
        if (colpisPacked || isMoving)
            return;


        StartCoroutine(
            MoveSideMenuRoutine(
                colpisButton,
                colpisRect,
                "쿨피스"
            )
        );
    }


    // ========================================
    // 계란찜
    // ========================================

    public void PutEggIntoContainer()
    {
        if (eggPacked || isMoving)
            return;


        StartCoroutine(
            MoveSideMenuRoutine(
                eggButton,
                eggRect,
                "계란찜"
            )
        );
    }


    // ========================================
    // 사이드 메뉴 이동
    // ========================================

    private IEnumerator MoveSideMenuRoutine(
        Button sideButton,
        RectTransform sideRect,
        string itemName)
    {
        isMoving = true;


        sideButton.interactable = false;


        statusText.text =
            itemName + " 추가중...";


        Vector2 startPosition =
            sideRect.position;

        Vector2 targetPosition =
            boxRect.position;


        Vector3 startScale =
            sideRect.localScale;

        Vector3 targetScale =
            Vector3.one * sideMenuInBoxScale;


        float elapsedTime = 0f;


        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;


            float t =
                elapsedTime / moveDuration;


            sideRect.position =
                Vector2.Lerp(
                    startPosition,
                    targetPosition,
                    t
                );


            sideRect.localScale =
                Vector3.Lerp(
                    startScale,
                    targetScale,
                    t
                );


            yield return null;
        }


        sideRect.position =
            targetPosition;


        sideRect.localScale =
            targetScale;


        sideButton.image.raycastTarget = false;


        // ========================================
        // 실제 포장 결과 GameData 저장
        // ========================================

        if (itemName == "쿨피스")
        {
            colpisPacked = true;

            GameData.HasColpis = true;
        }


        if (itemName == "계란찜")
        {
            eggPacked = true;

            GameData.HasSteamedEgg = true;
        }


        isMoving = false;


        if (eggPacked &&
            colpisPacked &&
            foodPacked)
        {
            statusText.text =
                "비닐에 옮겨 포장하세요";
        }
        else if (
            eggPacked &&
            colpisPacked)
        {
            statusText.text =
                "떡볶이를 포장하세요";
        }
        else
        {
            statusText.text =
                "사이드 메뉴를 포장하세요";
        }
    }


    // ========================================
    // 용기 → 비닐
    // ========================================

    public void PutContainerIntoBag()
    {
        if (!foodPacked)
            return;


        if (bagPacked || isMoving)
            return;


        StartCoroutine(
            MoveContainerToBagRoutine()
        );
    }


    private IEnumerator MoveContainerToBagRoutine()
    {
        isMoving = true;


        packageBoxButton.interactable = false;


        colpisButton.interactable = false;
        eggButton.interactable = false;


        statusText.text =
            "비닐에 옮겨 포장하세요";


        Vector2 targetPosition =
            bagRect.position;


        Vector2 boxStartPosition =
            boxRect.position;


        Vector2 foodStartPosition =
            foodRect.position;


        Vector2 colpisStartPosition =
            colpisRect.position;


        Vector2 eggStartPosition =
            eggRect.position;


        Vector3 boxStartScale =
            boxRect.localScale;


        Vector3 targetBoxScale =
            Vector3.one * boxInBagScale;


        float elapsedTime = 0f;


        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;


            float t =
                elapsedTime / moveDuration;


            boxRect.position =
                Vector2.Lerp(
                    boxStartPosition,
                    targetPosition,
                    t
                );


            foodRect.position =
                Vector2.Lerp(
                    foodStartPosition,
                    targetPosition,
                    t
                );


            if (colpisPacked)
            {
                colpisRect.position =
                    Vector2.Lerp(
                        colpisStartPosition,
                        targetPosition,
                        t
                    );
            }


            if (eggPacked)
            {
                eggRect.position =
                    Vector2.Lerp(
                        eggStartPosition,
                        targetPosition,
                        t
                    );
            }


            boxRect.localScale =
                Vector3.Lerp(
                    boxStartScale,
                    targetBoxScale,
                    t
                );


            yield return null;
        }


        boxRect.position =
            targetPosition;


        foodRect.position =
            targetPosition;


        if (colpisPacked)
        {
            colpisRect.position =
                targetPosition;
        }


        if (eggPacked)
        {
            eggRect.position =
                targetPosition;
        }


        boxRect.localScale =
            targetBoxScale;


        bagPacked = true;

        isMoving = false;


        packageBoxButton.image.raycastTarget = false;


        plasticBagButton.interactable = true;


        statusText.text =
            "비닐을 밀봉하세요";
    }


    // ========================================
    // 비닐 밀봉
    // ========================================

    public void SealPlasticBag()
    {
        if (!bagPacked)
            return;


        if (readyForDelivery)
            return;


        plasticBagButton.interactable = false;


        readyForDelivery = true;


        deliveryButton.gameObject.SetActive(true);

        deliveryButton.interactable = true;


        statusText.text =
            "포장을 전부 마쳤습니다";
    }


    // ========================================
    // 배달 요청
    // ========================================

    public void RequestDelivery()
    {
        if (!readyForDelivery || isMoving)
            return;


        deliveryButton.interactable = false;


        StartCoroutine(
            DeliveryRoutine()
        );
    }


    // ========================================
    // 실제 배달
    // ========================================

    private IEnumerator DeliveryRoutine()
    {
        isMoving = true;


        statusText.text =
            "배달 기사 이동 중...";


        // ----------------------------------------
        // 1. 손 등장
        // ----------------------------------------

        yield return StartCoroutine(
            deliveryHandPopup.ShowHand()
        );


        // ----------------------------------------
        // 2. 음식 집는 시간
        // ----------------------------------------

        yield return new WaitForSeconds(
            0.5f
        );


        // ----------------------------------------
        // 3. 음식 제거
        // ----------------------------------------

        TbkButton.gameObject
            .SetActive(false);


        packageBoxButton.gameObject
            .SetActive(false);


        plasticBagButton.gameObject
            .SetActive(false);


        if (colpisPacked)
        {
            colpisButton.gameObject
                .SetActive(false);
        }


        if (eggPacked)
        {
            eggButton.gameObject
                .SetActive(false);
        }


        // ----------------------------------------
        // 4. 손 퇴장
        // ----------------------------------------

        yield return StartCoroutine(
            deliveryHandPopup.HideHand()
        );


        // ----------------------------------------
        // 5. 배달 완료
        // ----------------------------------------

        statusText.text =
            "배달 완료!";


        isMoving = false;


        // ========================================
        // ★ 핵심
        //
        // 방금 완료한 실제 BillData를 받아온다.
        // ========================================

        BillData completedBill = null;


        if (OrderManager.Instance != null)
        {
            completedBill =
                OrderManager.Instance
                    .CompleteCurrentOrder();
        }
        else
        {
            Debug.LogWarning(
                "OrderManager.Instance가 없습니다."
            );
        }


        // ========================================
        // ★ 리뷰 기록 저장
        //
        // BillData + 현재 GameData 결과를
        // ReviewHistory에 한 세트로 저장
        // ========================================

        if (completedBill != null)
        {
            ReviewHistory.AddCompletedOrder(
                completedBill
            );
        }


        // 저장 확인용
        Debug.Log(
            "현재 저장된 리뷰 수: "
            + ReviewHistory.Records.Count
        );


        // ----------------------------------------
        // 완료 문구 표시
        // ----------------------------------------

        yield return new WaitForSeconds(
            1.2f
        );


        // ----------------------------------------
        // MainScene 복귀
        // ----------------------------------------

        SceneManager.LoadScene(
            "MainScene"
        );
    }
}