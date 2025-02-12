using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public void SpawnFruitFromPool(FruitsID fruitID)
    {
        string tag = fruitID.ToString(); // ������ �±� ��ȯ

        PoolObject fruit = GameManager.Instance.poolManager.CreatePrefabs(tag);
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
        GameManager.Instance.objectPool.ReturnAllObjects();
        Debug.Log("��� ���� �������� Object Pool�� ��ȯ�Ǿ����ϴ�.");
    }
}
