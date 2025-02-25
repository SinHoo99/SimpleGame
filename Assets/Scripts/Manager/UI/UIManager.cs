using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    [SerializeField] private InventoryManager _inventoryManager; // Inspector���� �Ҵ�
    public InventoryManager InventoryManager => _inventoryManager;

    private void Start()
    {
        _inventoryManager.TriggerInventoryUpdate(); // UI �ʱ�ȭ
    }
}
