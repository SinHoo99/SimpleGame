using DG.Tweening;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    [SerializeField] private InventoryManager _inventoryManager; // Inspector���� �Ҵ�
    public InventoryManager InventoryManager => _inventoryManager;


    private bool isVisible = false;

    private void Start()
    {
        _inventoryManager.TriggerInventoryUpdate(); // UI �ʱ�ȭ
    }
    public void OnDoTween(GameObject gameObject, Vector3 originalPosition, int targetpositionY)
    {
        if (!isVisible)
        {
            // UI Ȱ��ȭ �� �ö󰡴� �ִϸ��̼�
            gameObject.SetActive(true);
            gameObject.transform.DOMoveY(targetpositionY, 0.5f).SetEase(Ease.OutCubic);
        }
        else
        {
            // �������� �ִϸ��̼� �� ��Ȱ��ȭ
            gameObject.transform.DOMoveY(originalPosition.y, 0.5f)
                .SetEase(Ease.InCubic)
                .OnComplete(() => gameObject.SetActive(false));
        }

        isVisible = !isVisible;
    }
}
