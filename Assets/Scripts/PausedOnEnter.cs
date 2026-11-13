using UnityEngine;

public class PausedOnEnter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OrderManager.Instance.SetPaused(true);
    }

    // Update is called once per frame
    private void OnDestroy() {
        OrderManager.Instance.SetPaused(false);
    }
}
