using UnityEngine;
using DG.Tweening;

public class SellingUI : MonoBehaviour, IShowAndHide
{
    public Vector3 OriginalPosition { get; private set; }

    private void Awake()
    {
        OriginalPosition = transform.position;
    }

    public void ShowAndHide()
    {
        GameManager.Instance.UIManager.InventoryManager.TriggerInventoryUpdate();
        GameManager.Instance.UIManager.OnDoTween(this.gameObject, OriginalPosition, 1500);
    }
}
