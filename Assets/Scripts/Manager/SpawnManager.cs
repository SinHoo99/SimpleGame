using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public void SpawnFruitFromPool(FruitsID fruitID)
    {
        var prefab = GameManager.Instance.DataManager.GetFruitPrefab(fruitID);
        if (prefab == null)
        {
            Debug.LogWarning($"{fruitID}의 프리팹이 로드되지 않았습니다.");
            return;
        }

        PoolObject fruit = GameManager.Instance.ObjectPool.SpawnFromPool(prefab.name);
        if (fruit != null)
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(-2, 2),
                0,
                Random.Range(-2, 2)
            );
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
