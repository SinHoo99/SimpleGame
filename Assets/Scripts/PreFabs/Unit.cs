using System.Collections;
using UnityEngine;

public class Unit : PoolObject
{
    private GameManager GM => GameManager.Instance;
    [SerializeField] private FruitsID FruitsID;

    [SerializeField] private Transform FirePoint;
    private Boss Boss;

    private void OnEnable()
    {
        if (Boss == null)
        {
            Boss = FindObjectOfType<Boss>(); //  ���̾��Ű���� `Boss` ��ũ��Ʈ�� �ִ� ������Ʈ ã��
        }

        Debug.Log($"{gameObject.name}�� FruitID ��: {FruitsID}");

        if ((int)FruitsID == 0)
        {
            Debug.LogWarning($"{gameObject.name}�� FruitID�� �������� �ʾҽ��ϴ�! �ʱ�ȭ�� �ʿ��մϴ�.");
            AssignFruitID();
            return;
        }

        StopAllCoroutines(); //  ���� �ڷ�ƾ �����Ͽ� �ߺ� ���� ����
        StartCoroutine(ShootCoroutine());
        StartCoroutine(UpdateCoinCoroutine());
    }

    #region ���� ID �Ҵ� �� ���� ������Ʈ �ڷ�ƾ
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
        if ((int)FruitsID != 0) return;

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
    #endregion

    #region �Ѿ� ���� ���� �޼���
    public PoolObject CreateBullet(string tag, Vector2 position, Vector2 direction, string ownerTag)
    {
        PoolObject bullet = GM.ObjectPool.SpawnFromPool(tag);
        bullet.ReturnMyComponent<Bullet>().Initialize(position, direction, ownerTag);
        return bullet;
    }

    private IEnumerator ShootCoroutine()
    {
        //  ������ ������ ��� (�ݺ������� `FindWithTag()` �������� ����)
        while (Boss == null)
        {
            yield return null;
            Boss = FindObjectOfType<Boss>();
        }

        while (true)
        {
            float randomNum = Random.Range(0.5f, 1.5f);
            yield return new WaitForSeconds(randomNum);
            ShootBullet();
        }
    }

    private void ShootBullet()
    {
        if (Boss == null) return; //  ������ null���� ���� üũ

        SpriteRenderer bossSprite = Boss.GetComponentInChildren<SpriteRenderer>();
        if (bossSprite == null || !bossSprite.enabled)
        {
            return; //  ���� ��������Ʈ�� ��Ȱ��ȭ�Ǿ����� �߻����� ����
        }

        Vector2 direction = (Boss.transform.position - FirePoint.position).normalized;
        CreateBullet(Tag.Bullet, FirePoint.position, direction, gameObject.tag);
    }
    #endregion
}
