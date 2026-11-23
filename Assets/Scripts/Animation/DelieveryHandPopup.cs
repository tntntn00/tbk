using System.Collections;
using UnityEngine;

public class DeliveryHandPopup : MonoBehaviour
{
    [Header("Delivery Hand")]
    [SerializeField] private RectTransform deliveryHand;

    [Header("Position")]
    [SerializeField] private Vector2 hiddenPosition = new Vector2(800f, 0f);
    [SerializeField] private Vector2 popupPosition = new Vector2(250f, 0f);

    [Header("Animation")]
    [SerializeField] private float moveDuration = 0.5f;

    private void Awake()
    {
        if (deliveryHand == null)
            deliveryHand = GetComponent<RectTransform>();

        deliveryHand.anchoredPosition = hiddenPosition;
    }

    public IEnumerator ShowHand()
    {
        yield return MoveHand(deliveryHand.anchoredPosition, popupPosition);
    }

    public IEnumerator HideHand()
    {
        yield return MoveHand(deliveryHand.anchoredPosition, hiddenPosition);
    }

    private IEnumerator MoveHand(Vector2 start, Vector2 end)
    {
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);
            deliveryHand.anchoredPosition = Vector2.Lerp(start, end, t);
            yield return null;
        }

        deliveryHand.anchoredPosition = end;
    }
}
