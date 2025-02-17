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
        Debug.Log($"{gameObject.name}�� FruitID ��: {FruitsID}");

        if ((int)FruitsID == 0) // �⺻�� üũ
        {
            Debug.LogWarning($"{gameObject.name}�� FruitID�� �������� �ʾҽ��ϴ�! �ʱ�ȭ�� �ʿ��մϴ�.");
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
            yield return new WaitForSeconds(1f); // 1�ʸ��� ����
            if (GM.DataManager.FriutDatas.TryGetValue(FruitsID, out var fruitsData))
            {
                GM.PlayerDataManager.NowPlayerData.PlayerCoin += fruitsData.Price;
                GM.UIManager.TriggerInventoryUpdate();
            }
            else
            {
                Debug.LogWarning($"�ش� ���� ID({FruitsID})�� ���� �����͸� ã�� �� �����ϴ�.");
            }
        }
    }
    public void AssignFruitID()
    {
        if ((int)FruitsID != 0) return; // **�̹� ������ ��� ����**

        string prefabName = gameObject.name.Replace("(Clone)", "").Trim();
        Debug.Log($"[AssignFruitID] {gameObject.name}�� PrefabName: {prefabName}");

        if (GM.DataManager.FriutDatas == null)
        {
            Debug.LogError("[AssignFruitID] GM.DataManager.FriutDatas�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
            return;
        }

        foreach (var fruitData in GM.DataManager.FriutDatas.Values)
        {
            if (fruitData.Name.Trim() == prefabName)
            {
                FruitsID = fruitData.ID;
                Debug.Log($"[AssignFruitID] {gameObject.name}�� FruitID�� {FruitsID}�� ������.");
                return;
            }
        }

        Debug.LogError($"[AssignFruitID] {gameObject.name}�� FruitsID �ڵ� �Ҵ� ����! CSV���� {prefabName}�� ã�� �� �����ϴ�.");
    }


    private IEnumerator ShootCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f); // ������ �������� �߻�
            ShootBullet();
        }
    }

    private void ShootBullet()
    {
        Vector2 direction = Vector2.right; // ����: ���������� �߻�
        CreateBullet(Tag.Bullet, FirePoint.position, direction, gameObject.tag);
    }
    public PoolObject CreateBullet(string tag, Vector2 position, Vector2 direction, string ownerTag)
    {
        PoolObject bullet = GM.ObjectPool.SpawnFromPool(tag);
        bullet.ReturnMyComponent<Bullet>().Initialize(position, direction, ownerTag);
        return bullet;
    }
}

