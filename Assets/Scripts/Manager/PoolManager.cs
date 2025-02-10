using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PoolManager : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    protected ObjectPool ObjectPool => GM.ObjectPool;

    [Header("Bullet")]
    [SerializeField] private PoolObject Apple;
    [SerializeField] private PoolObject Banana;
    [SerializeField] private PoolObject Carrot;
    [SerializeField] private PoolObject Melon;



    #region ������ƮǮ �ʱ�ȭ
    public void AddObjectPool()
    {

        ObjectPool.AddObjectPool(Tag.Apple, GM.GetFruitsData(FruitsID.Apple).Prefab, 20);
        ObjectPool.AddObjectPool(Tag.Banana, GM.GetFruitsData(FruitsID.Banana).Prefab, 20);
        ObjectPool.AddObjectPool(Tag.Carrot, GM.GetFruitsData(FruitsID.Carrot).Prefab, 20);
        ObjectPool.AddObjectPool(Tag.Melon, GM.GetFruitsData(FruitsID.Melon).Prefab, 20);
    }

    public PoolObject CreatePrefabs(string tag, FruitsID fruitID)
    {
        PoolObject fruit = GM.ObjectPool.SpawnFromPool(tag);
        if (fruit == null)
        {
            Debug.LogError($"[PoolManager] {tag} �������� Ǯ���� �������� ���߽��ϴ�.");
            return null;
        }

        // ������ ���� ��Ʈ�ѷ��� ID ����
        var unitController = fruit.ReturnMyComponent<UnitController>();
        unitController.FruitsID = fruitID; // FruitsID ����

        unitController.Initialize(tag, (int)fruitID); // �ʿ� �� �߰� ����

        return fruit; // ������ ������Ʈ ��ȯ
    }

    #endregion
}
