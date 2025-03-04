using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Unit : PoolObject
{
    private GameManager GM => GameManager.Instance;

    [SerializeField] private FruitsID _fruitsID;
    [SerializeField] private Transform _firePoint;

    private Boss Boss;
    private Coroutine _shootCoroutine;

    private void Awake()
    {
        AssignFruitID();
    }
    private void OnEnable()
    {
        if (Boss == null)
        {
            Boss = FindObjectOfType<Boss>(); //  ���̾��Ű���� `Boss` ��ũ��Ʈ�� �ִ� ������Ʈ ã��
        }

        if ((int)_fruitsID == 0)
        {
            Debug.LogWarning($"{gameObject.name}�� FruitID�� �������� �ʾҽ��ϴ�! �ʱ�ȭ�� �ʿ��մϴ�.");
            return;
        }

        if (_shootCoroutine != null)
        {
            StopCoroutine(_shootCoroutine);
        }
        _shootCoroutine = StartCoroutine(ShootCoroutine());
    }

    private void OnDisable()
    {
        if (_shootCoroutine != null)
        {
            StopCoroutine(_shootCoroutine);
            _shootCoroutine = null;
        }
    }
    #region ���� ID �Ҵ�

    public void AssignFruitID()
    {
        if ((int)_fruitsID != 0) return;

        string prefabName = gameObject.name.Replace("(Clone)", "").Trim();
        Debug.Log($"[AssignFruitID] {gameObject.name}�� PrefabName: {prefabName}");

        if (GM.DataManager.FruitDatas == null)
        {
            Debug.LogError("[AssignFruitID] GM.DataManager.FriutDatas�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
            return;
        }

        foreach (var fruitData in GM.DataManager.FruitDatas.Values)
        {
            if (fruitData.Name.Trim() == prefabName)
            {
                _fruitsID = fruitData.ID;
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

        float bulletDamage = GetBulletDamage();
        bullet.ReturnMyComponent<Bullet>().Initialize(position, direction, ownerTag, bulletDamage);
        return bullet;
    }

    private IEnumerator ShootCoroutine()
    {
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

        Vector2 direction = (Boss.transform.position - _firePoint.position).normalized;
        CreateBullet(Tag.Bullet, _firePoint.position, direction, gameObject.tag);
        ShootEffect();
        GM.PlaySFX(SFX.Shoot);
    }

    private float GetBulletDamage()
    {
        return GM.GetFruitsData(_fruitsID).Damage * 0.1f;
    }

    #endregion

    #region ���� ����Ʈ ����

    private void ShootEffect()
    {
        PlayerRecoilEffect();
        FirePointShakeEffect();
        FirePointScaleEffect();
    }
    private void PlayerRecoilEffect()
    {
        transform.DOScale(new Vector3(0.95f, 1.05f, 1f), 0.05f) 
            .OnComplete(() => transform.DOScale(Vector3.one, 0.05f)); 
    }
    private void FirePointShakeEffect()
    {
        _firePoint.DOShakePosition(0.2f, 0.1f); // (���ӽð�, ����)
    }
    private void FirePointScaleEffect()
    {
        _firePoint.DOScale(1.2f, 0.05f) 
            .OnComplete(() => _firePoint.DOScale(1f, 0.05f));
    }
    #endregion
}
