using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform spawnParent;
    public Vector3 spawnAreaMin;
    public Vector3 spawnAreaMax;

    /// <summary>
    /// Inventory에 있는 과일을 프리팹으로 소환합니다.
    /// </summary>
    public void SpawnFruitsFromInventory()
    {
        var inventory = GameManager.Instance.NowPlayerData.Inventory;

        foreach (var fruit in inventory)
        {
            if (fruit.Value.Amount <= 0) continue; // 수량이 0인 경우 스킵

            // DataManager를 통해 프리팹 가져오기
            var prefab = GameManager.Instance.DataManager.GetFruitPrefab(fruit.Key);
            if (prefab == null) continue;

            // 과일 수량만큼 소환
            SpawnFruitPrefab(prefab, fruit.Value.Amount);
        }
    }

    private void SpawnFruitPrefab(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                0, // 고정된 Y 좌표
                Random.Range(spawnAreaMin.z, spawnAreaMax.z)
            );

            Instantiate(prefab, spawnPosition, Quaternion.identity, spawnParent);
        }
    }
}
