using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    [SerializeField] private FruitUIManager _fruitUIManager; // Inspector에서 할당
    public FruitUIManager FruitUIManager => _fruitUIManager;

    public event Action OnInventoryUpdated;

    private void Start()
    {
        OnInventoryUpdated += UpdateInventoryUI;
        _fruitUIManager.SetFruitData(GM.DataManager.FriutDatas); // 과일 데이터 설정
    }

    private void OnDestroy()
    {
        OnInventoryUpdated -= UpdateInventoryUI;
    }

    public void TriggerInventoryUpdate()
    {
        OnInventoryUpdated?.Invoke();
        GM.PlayerStatusUI.PlayerCoin();
    }

    private void UpdateInventoryUI()
    {
        if (GM.PlayerDataManager.NowPlayerData.Inventory == null || GM.PlayerDataManager.NowPlayerData.Inventory.Count == 0)
        {
            _fruitUIManager.UpdateFruitCountsUI(new Dictionary<FruitsID, int>());
            return;
        }


        _fruitUIManager.UpdateFruitCountsUI(
            GM.PlayerDataManager.NowPlayerData.Inventory.ToDictionary(kv => kv.Key, kv => kv.Value.Amount)
        );
    }
}
