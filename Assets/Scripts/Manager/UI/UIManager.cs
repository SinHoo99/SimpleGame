using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    [SerializeField] private InventoryManager _inventoryManager; // Inspector에서 할당
    public InventoryManager InventoryManager => _inventoryManager;

    private void Start()
    {
        _inventoryManager.TriggerInventoryUpdate(); // UI 초기화
    }
}
