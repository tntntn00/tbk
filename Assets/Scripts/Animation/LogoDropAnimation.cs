using UnityEngine;
using System.Collections;

public class LogoDropAnimation : MonoBehaviour
{
    public RectTransform logoRect;
    public float startY = 500f;   // 시작 위치 (화면 위)
    public float endY = 0f;       // 도착 위치
    public float duration = 1.2f; // 애니메이션 시간
    public AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    void Start()
    {
        StartCoroutine(DropLogo());
    }

    IEnumerator DropLogo()
    {
        logoRect.anchoredPosition = new Vector2(logoRect.anchoredPosition.x, startY);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float easedT = easeCurve.Evaluate(t); // 이징 커브 적용
            float y = Mathf.Lerp(startY, endY, easedT);
            logoRect.anchoredPosition = new Vector2(logoRect.anchoredPosition.x, y);
            yield return null;
        }

        logoRect.anchoredPosition = new Vector2(logoRect.anchoredPosition.x, endY);
    }
}