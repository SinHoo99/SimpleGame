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



    #region 오브젝트풀 초기화
    public void AddObjectPool()
    {

        ObjectPool.AddObjectPool(Tag.Apple, GM.GetFruitsData(FruitsID.Apple).Prefab, 20);
        ObjectPool.AddObjectPool(Tag.Banana, GM.GetFruitsData(FruitsID.Banana).Prefab, 20);
        ObjectPool.AddObjectPool(Tag.Carrot, GM.GetFruitsData(FruitsID.Carrot).Prefab, 20);
        ObjectPool.AddObjectPool(Tag.Melon, GM.GetFruitsData(FruitsID.Melon).Prefab, 20);
    }

    public PoolObject CreatePrefabs(string tag)
    {
        PoolObject fruit = GM.ObjectPool.SpawnFromPool(tag);
        if (fruit == null)
        {
            Debug.LogError($"[PoolManager] {tag} 프리팹을 풀에서 가져오지 못했습니다.");
            return null;
        }
        Unit unit = fruit.ReturnMyComponent<Unit>();

        if (unit == null)
        {
            Debug.LogError($"[CreatePrefabs] {fruit.gameObject.name} 오브젝트에 Unit 컴포넌트가 없습니다!");
        }
        else
        {
            unit.AssignFruitID();
        }
        return fruit;
    }

    #endregion
}
