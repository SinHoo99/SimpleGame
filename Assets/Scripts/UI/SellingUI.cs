using UnityEngine;

public class SellingUI : MonoBehaviour, IShowAndHide
{
    public void ShowAndHide()
    {
        GameManager.Instance.uiManager.TriggerInventoryUpdate();
        this.gameObject.SetActive(!gameObject.activeSelf);
    }
}
