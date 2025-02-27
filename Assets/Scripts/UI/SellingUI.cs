using UnityEngine;
using DG.Tweening;

public class SellingUI : MonoBehaviour, IShowAndHide
{
    public Vector3 OriginalPosition { get; private set; }
    private GameManager GM => GameManager.Instance;

    private void Awake()
    {
        OriginalPosition = transform.position;
    }

    private void OnEnable()
    {
        GM.UIManager.InventoryManager.TriggerInventoryUpdate();
    }

    public void ShowAndHide()
    {
        GM.PlaySFX(SFX.Click);
        GM.UIManager.InventoryManager.TriggerInventoryUpdate();
        GM.UIManager.OnDoTween(this.gameObject, OriginalPosition);
    }
}
