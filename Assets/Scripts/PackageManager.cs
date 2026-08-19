using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PackageManager : MonoBehaviour
{
    [Header("Main Package Buttons")]
    [SerializeField] private Button cookedFoodButton;
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
    [SerializeField] private float sideMenuInBoxScale = 0.45f;
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

    private void Start()
    {
        foodRect =
            cookedFoodButton.GetComponent<RectTransform>();

        boxRect =
            packageBoxButton.GetComponent<RectTransform>();

        bagRect =
            plasticBagButton.GetComponent<RectTransform>();

        colpisRect =
            colpisButton.GetComponent<RectTransform>();

        eggRect =
            eggButton.GetComponent<RectTransform>();

        cookedFoodButton.interactable = true;
        colpisButton.interactable = true;
        eggButton.interactable = true;

        packageBoxButton.interactable = false;
        plasticBagButton.interactable = false;
        deliveryButton.gameObject.SetActive(true);
        deliveryButton.interactable = false;

        cookedFoodButton.image.raycastTarget = true;
        colpisButton.image.raycastTarget = true;
        eggButton.image.raycastTarget = true;

        packageBoxButton.image.raycastTarget = true;
        plasticBagButton.image.raycastTarget = true;

        statusText.text =
            "Put the food into the container";
    }

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

        cookedFoodButton.interactable = false;

        statusText.text =
            "Packing the food...";

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

        cookedFoodButton.image.raycastTarget = false;

        packageBoxButton.interactable = true;

        statusText.text =
            "Add side dishes or pack the container";
    }

    public void PutColpisIntoContainer()
    {
        if (colpisPacked || isMoving)
            return;

        StartCoroutine(
            MoveSideMenuRoutine(
                colpisButton,
                colpisRect,
                "Colpis"
            )
        );
    }

    public void PutEggIntoContainer()
    {
        if (eggPacked || isMoving)
            return;

        StartCoroutine(
            MoveSideMenuRoutine(
                eggButton,
                eggRect,
                "Steamed Egg"
            )
        );
    }

    private IEnumerator MoveSideMenuRoutine(
        Button sideButton,
        RectTransform sideRect,
        string itemName)
    {
        isMoving = true;

        sideButton.interactable = false;

        statusText.text =
            "Adding " + itemName + "...";

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

        if (itemName == "Colpis")
        {
            colpisPacked = true;
            GameData.HasColpis = true;
        }

        if (itemName == "Steamed Egg")
        {
            eggPacked = true;
            GameData.HasSteamedEgg = true;
        }

        isMoving = false;

        statusText.text =
            "Add side dishes or pack the container";
    }

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
            "Putting everything into the bag...";

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

        boxRect.position = targetPosition;
        foodRect.position = targetPosition;

        if (colpisPacked)
        {
            colpisRect.position = targetPosition;
        }

        if (eggPacked)
        {
            eggRect.position = targetPosition;
        }

        boxRect.localScale =
            targetBoxScale;

        bagPacked = true;
        isMoving = false;

        packageBoxButton.image.raycastTarget = false;

        plasticBagButton.interactable = true;

        statusText.text =
            "Seal the plastic bag";
    }

    public void SealPlasticBag()
    {
        if (!bagPacked)
            return;

        if (readyForDelivery)
            return;

        plasticBagButton.interactable = false;

        readyForDelivery = true;

        // 배달 버튼을 확실하게 활성화
        deliveryButton.gameObject.SetActive(true);
        deliveryButton.interactable = true;

        statusText.text =
            "Ready for delivery";
    }

    public void RequestDelivery()
    {
        if (!readyForDelivery || isMoving)
            return;

        deliveryButton.interactable = false;

        StartCoroutine(
            DeliveryRoutine()
        );
    }

    private IEnumerator DeliveryRoutine()
    {
        isMoving = true;

        statusText.text =
            "Delivery rider is coming...";

        // 1. 손이 오른쪽에서 들어온다.
        yield return StartCoroutine(
            deliveryHandPopup.ShowHand()
        );

        // 2. 잠깐 멈춰서 음식을 집는 느낌
        yield return new WaitForSeconds(0.5f);

        // 3. 포장된 음식들을 화면에서 없앤다.
        cookedFoodButton.gameObject.SetActive(false);
        packageBoxButton.gameObject.SetActive(false);
        plasticBagButton.gameObject.SetActive(false);

        if (colpisPacked)
        {
            colpisButton.gameObject.SetActive(false);
        }

        if (eggPacked)
        {
            eggButton.gameObject.SetActive(false);
        }

        // 4. 손이 다시 오른쪽 밖으로 나간다.
        yield return StartCoroutine(
            deliveryHandPopup.HideHand()
        );

        // 5. 배달 완료
        statusText.text =
            "Delivery Complete!";

        isMoving = false;

        // 완료 문구를 잠깐 보여준다.
        yield return new WaitForSeconds(1.2f);

        // 메인 허브로 복귀
        SceneManager.LoadScene("MainScene");

        // 완료 메시지를 잠깐 보여준다.
        yield return new WaitForSeconds(1f);

        // 리뷰 화면으로 이동
        SceneManager.LoadScene("ReviewScene");
            }
}