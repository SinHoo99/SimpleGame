using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform spawnParent;
    public Vector3 spawnAreaMin;
    public Vector3 spawnAreaMax;

    /// <summary>
    /// Inventory�� �ִ� ������ ���������� ��ȯ�մϴ�.
    /// </summary>
    public void SpawnFruitsFromInventory()
    {
        var inventory = GameManager.Instance.NowPlayerData.Inventory;

        foreach (var fruit in inventory)
        {
            if (fruit.Value.Amount <= 0) continue; // ������ 0�� ��� ��ŵ

            // DataManager�� ���� ������ ��������
            var prefab = GameManager.Instance.DataManager.GetFruitPrefab(fruit.Key);
            if (prefab == null) continue;

            // ���� ������ŭ ��ȯ
            SpawnFruitPrefab(prefab, fruit.Value.Amount);
        }
    }

    private void SpawnFruitPrefab(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                0, // ������ Y ��ǥ
                Random.Range(spawnAreaMin.z, spawnAreaMax.z)
            );

            Instantiate(prefab, spawnPosition, Quaternion.identity, spawnParent);
        }
    }
}
