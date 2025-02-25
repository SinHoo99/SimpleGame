using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject Boss;
    [SerializeField] private float minSpawnDistance;
    [SerializeField] private float maxSpawnDistance;
    public void SpawnFruitFromPool(FruitsID fruitID)
    {
        string tag = fruitID.ToString(); // ������ �±� ��ȯ

        PoolObject fruit = GameManager.Instance.PoolManager.CreateUnitPrefabs(tag);
        if (fruit != null)
        {
            Vector3 bossPosition = Boss.transform.position;
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            float distance = Random.Range(minSpawnDistance, maxSpawnDistance);
            Vector3 spawnPosition = bossPosition + new Vector3(randomDir.x * distance, randomDir.y * distance, bossPosition.z);

            fruit.transform.position = spawnPosition;
            fruit.transform.rotation = Quaternion.identity;
            fruit.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"{fruitID}�� �ش��ϴ� Ȱ��ȭ ������ ������Ʈ�� �����ϴ�.");
        }
    }

    public void ReturnAllFruitsToPool()
    {
        GameManager.Instance.ObjectPool.ReturnAllObjects();
        Debug.Log("��� ���� �������� Object Pool�� ��ȯ�Ǿ����ϴ�.");
    }
}
