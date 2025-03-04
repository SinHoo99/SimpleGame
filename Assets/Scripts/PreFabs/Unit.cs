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
            Boss = FindObjectOfType<Boss>(); //  하이어라키에서 `Boss` 스크립트가 있는 오브젝트 찾기
        }

        if ((int)_fruitsID == 0)
        {
            Debug.LogWarning($"{gameObject.name}의 FruitID가 설정되지 않았습니다! 초기화가 필요합니다.");
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
    #region 유닛 ID 할당

    public void AssignFruitID()
    {
        if ((int)_fruitsID != 0) return;

        string prefabName = gameObject.name.Replace("(Clone)", "").Trim();
        Debug.Log($"[AssignFruitID] {gameObject.name}의 PrefabName: {prefabName}");

        if (GM.DataManager.FruitDatas == null)
        {
            Debug.LogError("[AssignFruitID] GM.DataManager.FriutDatas가 초기화되지 않았습니다.");
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

        Debug.LogError($"[AssignFruitID] {gameObject.name}의 FruitsID 자동 할당 실패! CSV에서 {prefabName}을 찾을 수 없습니다.");
    }
    #endregion

    #region 총알 생성 관련 메서드
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
        if (Boss == null) return; //  보스가 null인지 먼저 체크

        SpriteRenderer bossSprite = Boss.GetComponentInChildren<SpriteRenderer>();
        if (bossSprite == null || !bossSprite.enabled)
        {
            return; //  보스 스프라이트가 비활성화되었으면 발사하지 않음
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

    #region 유닛 이펙트 관련

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
        _firePoint.DOShakePosition(0.2f, 0.1f); // (지속시간, 강도)
    }
    private void FirePointScaleEffect()
    {
        _firePoint.DOScale(1.2f, 0.05f) 
            .OnComplete(() => _firePoint.DOScale(1f, 0.05f));
    }
    #endregion
}
