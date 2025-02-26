using UnityEngine;
using DG.Tweening;

public class SellingUI : MonoBehaviour, IShowAndHide
{
    private Vector3 originalPosition;

    private void Awake()
    {
        originalPosition = transform.position;
    }

    public void ShowAndHide()
    {
        GameManager.Instance.UIManager.InventoryManager.TriggerInventoryUpdate();
        GameManager.Instance.UIManager.OnDoTween(this.gameObject, originalPosition, 1500);
    }
}
