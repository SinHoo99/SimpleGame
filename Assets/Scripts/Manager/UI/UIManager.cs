using DG.Tweening;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    [SerializeField] private InventoryManager _inventoryManager; // Inspector에서 할당
    public InventoryManager InventoryManager => _inventoryManager;


    private bool isVisible = false;

    private void Start()
    {
        _inventoryManager.TriggerInventoryUpdate(); // UI 초기화
    }
    public void OnDoTween(GameObject gameObject, Vector3 originalPosition, int targetpositionY)
    {
        if (!isVisible)
        {
            // UI 활성화 후 올라가는 애니메이션
            gameObject.SetActive(true);
            gameObject.transform.DOMoveY(targetpositionY, 0.5f).SetEase(Ease.OutCubic);
        }
        else
        {
            // 내려가는 애니메이션 후 비활성화
            gameObject.transform.DOMoveY(originalPosition.y, 0.5f)
                .SetEase(Ease.InCubic)
                .OnComplete(() => gameObject.SetActive(false));
        }

        isVisible = !isVisible;
    }
}
