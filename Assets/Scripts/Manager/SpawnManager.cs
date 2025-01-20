using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    /// <summary>
    /// Object Pool에서 과일 프리팹을 가져와 활성화
    /// </summary>
    public void SpawnFruitFromPool(FruitsID fruitID)
    {
        var prefab = GameManager.Instance.DataManager.GetFruitPrefab(fruitID);
        if (prefab == null)
        {
            Debug.LogWarning($"{fruitID}의 프리팹이 로드되지 않았습니다.");
            return;
        }

        // Object Pool에서 프리팹 가져오기
        GameObject fruit = GameManager.Instance.objectPool.GetObject(prefab.name);
        if (fruit != null)
        {
            // 랜덤 위치에 배치
            Vector3 spawnPosition = new Vector3(
                Random.Range(-5, 5),
                0,
                Random.Range(-5, 5)
            );

            fruit.transform.position = spawnPosition;
            fruit.transform.rotation = Quaternion.identity;
            fruit.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"{fruitID}에 해당하는 활성화 가능한 오브젝트가 없습니다.");
        }
    }

    /// <summary>
    /// 모든 활성화된 프리팹을 반환
    /// </summary>
    public void ReturnAllFruitsToPool()
    {
        GameManager.Instance.objectPool.ReturnAllObjects();
        Debug.Log("모든 과일 프리팹이 Object Pool로 반환되었습니다.");
    }
}
