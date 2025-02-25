using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject Boss;
    [SerializeField] private float minSpawnDistance;
    [SerializeField] private float maxSpawnDistance;
    public void SpawnFruitFromPool(FruitsID fruitID)
    {
        string tag = fruitID.ToString(); // 과일의 태그 변환

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
            Debug.LogWarning($"{fruitID}에 해당하는 활성화 가능한 오브젝트가 없습니다.");
        }
    }

    public void ReturnAllFruitsToPool()
    {
        GameManager.Instance.ObjectPool.ReturnAllObjects();
        Debug.Log("모든 과일 프리팹이 Object Pool로 반환되었습니다.");
    }
}
