using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    /// <summary>
    /// Object Pool���� ���� �������� ������ Ȱ��ȭ
    /// </summary>
    public void SpawnFruitFromPool(FruitsID fruitID)
    {
        var prefab = GameManager.Instance.DataManager.GetFruitPrefab(fruitID);
        if (prefab == null)
        {
            Debug.LogWarning($"{fruitID}�� �������� �ε���� �ʾҽ��ϴ�.");
            return;
        }

        // Object Pool���� ������ ��������
        GameObject fruit = GameManager.Instance.objectPool.GetObject(prefab.name);
        if (fruit != null)
        {
            // ���� ��ġ�� ��ġ
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
            Debug.LogWarning($"{fruitID}�� �ش��ϴ� Ȱ��ȭ ������ ������Ʈ�� �����ϴ�.");
        }
    }

    /// <summary>
    /// ��� Ȱ��ȭ�� �������� ��ȯ
    /// </summary>
    public void ReturnAllFruitsToPool()
    {
        GameManager.Instance.objectPool.ReturnAllObjects();
        Debug.Log("��� ���� �������� Object Pool�� ��ȯ�Ǿ����ϴ�.");
    }
}
