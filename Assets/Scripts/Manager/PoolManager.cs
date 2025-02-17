using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PoolManager : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    protected ObjectPool ObjectPool => GM.ObjectPool;

    [SerializeField] private PoolObject Apple;
    [SerializeField] private PoolObject Banana;
    [SerializeField] private PoolObject Carrot;
    [SerializeField] private PoolObject Melon;

    [SerializeField] private PoolObject Bullet;



    #region ������ƮǮ �ʱ�ȭ
    public void AddObjectPool()
    {
        ObjectPool.AddObjectPool(Tag.Apple, GM.GetFruitsData(FruitsID.Apple).Prefab, 20);
        ObjectPool.AddObjectPool(Tag.Banana, GM.GetFruitsData(FruitsID.Banana).Prefab, 20);
        ObjectPool.AddObjectPool(Tag.Carrot, GM.GetFruitsData(FruitsID.Carrot).Prefab, 20);
        ObjectPool.AddObjectPool(Tag.Melon, GM.GetFruitsData(FruitsID.Melon).Prefab, 20);
        ObjectPool.AddObjectPool(Tag.Bullet, Bullet, 100);
    }

    public PoolObject CreateUnitPrefabs(string tag)
    {
        PoolObject fruit = ObjectPool.SpawnFromPool(tag);
        if (fruit == null)
        {
            Debug.LogError($"[PoolManager] {tag} �������� Ǯ���� �������� ���߽��ϴ�.");
            return null;
        }
        Unit unit = fruit.ReturnMyComponent<Unit>();

        if (unit == null)
        {
            Debug.LogError($"[CreatePrefabs] {fruit.gameObject.name} ������Ʈ�� Unit ������Ʈ�� �����ϴ�!");
        }
        else
        {
            unit.AssignFruitID();
        }
        return fruit;
    }



    #endregion
}
