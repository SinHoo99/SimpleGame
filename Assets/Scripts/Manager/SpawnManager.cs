using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public void SpawnFruitFromPool(FruitsID fruitID)
    {
        var prefab = GameManager.Instance.DataManager.GetFruitPrefab(fruitID);
        if (prefab == null)
        {
            Debug.LogWarning($"{fruitID}�� �������� �ε���� �ʾҽ��ϴ�.");
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
            Debug.LogWarning($"{fruitID}�� �ش��ϴ� Ȱ��ȭ ������ ������Ʈ�� �����ϴ�.");
        }
    }

    public void ReturnAllFruitsToPool()
    {
        GameManager.Instance.ObjectPool.ReturnAllObjects();
        Debug.Log("��� ���� �������� Object Pool�� ��ȯ�Ǿ����ϴ�.");
    }
}
