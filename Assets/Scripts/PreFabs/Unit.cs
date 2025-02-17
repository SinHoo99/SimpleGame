using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : PoolObject
{
    private GameManager GM => GameManager.Instance;
    [SerializeField] private FruitsID FruitsID;

    [SerializeField] private Transform FirePoint;

    private void OnEnable()
    {
        Debug.Log($"{gameObject.name}의 FruitID 값: {FruitsID}");

        if ((int)FruitsID == 0) // 기본값 체크
        {
            Debug.LogWarning($"{gameObject.name}의 FruitID가 설정되지 않았습니다! 초기화가 필요합니다.");
            AssignFruitID();
            return;
        }

        StartCoroutine(UpdateCoinCoroutine());
        StartCoroutine(ShootCoroutine());
    }
    private void OnDisable()
    {
        StopCoroutine(UpdateCoinCoroutine());
        StopCoroutine(ShootCoroutine());
    }

    private IEnumerator UpdateCoinCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // 1초마다 실행
            if (GM.DataManager.FriutDatas.TryGetValue(FruitsID, out var fruitsData))
            {
                GM.PlayerDataManager.NowPlayerData.PlayerCoin += fruitsData.Price;
                GM.UIManager.TriggerInventoryUpdate();
            }
            else
            {
                Debug.LogWarning($"해당 과일 ID({FruitsID})에 대한 데이터를 찾을 수 없습니다.");
            }
        }
    }
    public void AssignFruitID()
    {
        if ((int)FruitsID != 0) return; // **이미 설정된 경우 무시**

        string prefabName = gameObject.name.Replace("(Clone)", "").Trim();
        Debug.Log($"[AssignFruitID] {gameObject.name}의 PrefabName: {prefabName}");

        if (GM.DataManager.FriutDatas == null)
        {
            Debug.LogError("[AssignFruitID] GM.DataManager.FriutDatas가 초기화되지 않았습니다.");
            return;
        }

        foreach (var fruitData in GM.DataManager.FriutDatas.Values)
        {
            if (fruitData.Name.Trim() == prefabName)
            {
                FruitsID = fruitData.ID;
                Debug.Log($"[AssignFruitID] {gameObject.name}의 FruitID가 {FruitsID}로 설정됨.");
                return;
            }
        }

        Debug.LogError($"[AssignFruitID] {gameObject.name}의 FruitsID 자동 할당 실패! CSV에서 {prefabName}을 찾을 수 없습니다.");
    }


    private IEnumerator ShootCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f); // 지정된 간격으로 발사
            ShootBullet();
        }
    }

    private void ShootBullet()
    {
        Vector2 direction = Vector2.right; // 예제: 오른쪽으로 발사
        CreateBullet(Tag.Bullet, FirePoint.position, direction, gameObject.tag);
    }
    public PoolObject CreateBullet(string tag, Vector2 position, Vector2 direction, string ownerTag)
    {
        PoolObject bullet = GM.ObjectPool.SpawnFromPool(tag);
        bullet.ReturnMyComponent<Bullet>().Initialize(position, direction, ownerTag);
        return bullet;
    }
}

