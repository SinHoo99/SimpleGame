using System.Collections;
using UnityEngine;

public class CoinUpdater : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    public FruitsID FruitID;

    private void Awake()
    {
        AssignFruitID();
    }
    private void OnEnable()
    {
        Debug.Log($"{gameObject.name}의 FruitID 값: {FruitID}");

        if ((int)FruitID == 0) // 기본값 체크
        {
            Debug.LogWarning($"{gameObject.name}의 FruitID가 설정되지 않았습니다! 초기화가 필요합니다.");
            return;
        }

        StartCoroutine(UpdateCoinCoroutine());
    }

    public void AssignFruitID()
    {
        if ((int)FruitID == 0) 
        {
            string prefabName = gameObject.name.Replace("(Clone)", "").Trim();

            // CSV 데이터에서 prefabName에 해당하는 FruitsID 찾기
            foreach (var fruitData in GM.DataManager.FriutDatas.Values)
            {
                if (fruitData.Name == prefabName)
                {
                    FruitID = fruitData.ID;
                    Debug.Log($"[CoinUpdater] {gameObject.name}의 FruitID가 CSV 데이터에서 {FruitID}로 설정됨.");
                    return;
                }
            }
            Debug.LogError($"[CoinUpdater] {gameObject.name}의 FruitsID 자동 할당 실패! CSV에서 {prefabName}을(를) 찾을 수 없습니다.");
        }
    }

    private IEnumerator UpdateCoinCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // 1초마다 실행
            if (GM.DataManager.FriutDatas.TryGetValue(FruitID, out var fruitsData))
            {
                GM.PlayerDataManager.NowPlayerData.PlayerCoin += fruitsData.Price;
                GM.UIManager.TriggerInventoryUpdate();
            }
            else
            {
                Debug.LogWarning($"해당 과일 ID({FruitID})에 대한 데이터를 찾을 수 없습니다.");
            }
        }
    }
}
