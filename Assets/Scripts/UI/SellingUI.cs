using UnityEngine;
using DG.Tweening;

public class SellingUI : MonoBehaviour, IShowAndHide
{
    private Vector3 originalPosition; 
    private bool isVisible = false;
    [SerializeField] private int targetpositionY = 90;

    private void Awake()
    {
        originalPosition = transform.position; 
    }

    public void ShowAndHide()
    {
        GameManager.Instance.UIManager.InventoryManager.TriggerInventoryUpdate();

        if (!isVisible)
        {
            // UI Ȱ��ȭ �� �ö󰡴� �ִϸ��̼�
            gameObject.SetActive(true);
            transform.DOMoveY(targetpositionY, 0.5f).SetEase(Ease.OutCubic);
        }
        else
        {
            // �������� �ִϸ��̼� �� ��Ȱ��ȭ
            transform.DOMoveY(originalPosition.y, 0.5f)
                .SetEase(Ease.InCubic)
                .OnComplete(() => gameObject.SetActive(false));
        }

        isVisible = !isVisible;
    }
}
