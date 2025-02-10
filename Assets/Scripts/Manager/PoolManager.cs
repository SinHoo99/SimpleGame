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



    #region 오브젝트풀 초기화
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
            Debug.LogError($"[PoolManager] {tag} 프리팹을 풀에서 가져오지 못했습니다.");
            return null;
        }

        // 과일의 유닛 컨트롤러에 ID 설정
        var unitController = fruit.ReturnMyComponent<UnitController>();
        unitController.FruitsID = fruitID; // FruitsID 저장

        unitController.Initialize(tag, (int)fruitID); // 필요 시 추가 설정

        return fruit; // 생성된 오브젝트 반환
    }

    #endregion
}
