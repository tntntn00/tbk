using UnityEngine;

public class BillToggleButton : MonoBehaviour
{
    public void OnClickToggle()
    {
        OrderManager.Instance.ToggleBillArea();
    }
}
