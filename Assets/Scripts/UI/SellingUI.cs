using UnityEngine;

public class SellingUI : MonoBehaviour, IShowAndHide
{
    public void ShowAndHide()
    {
        GameManager.Instance.UIManager.TriggerInventoryUpdate();
        this.gameObject.SetActive(!gameObject.activeSelf);
    }
}
