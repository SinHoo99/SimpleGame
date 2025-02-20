using System.Collections;
using UnityEngine;

public class Unit : PoolObject
{
    private GameManager GM => GameManager.Instance;
    [SerializeField] private FruitsID FruitsID;

    [SerializeField] private Transform FirePoint;
    [SerializeField] GameObject ShootEffectPrefab;
    private Boss Boss;

    private ParticleSystem ParticleSystem;

    private void Awake()
    {
        ParticleSystem = ShootEffectPrefab.GetComponentInChildren<ParticleSystem>();
    }
    private void OnEnable()
    {
        if (Boss == null)
        {
            Boss = FindObjectOfType<Boss>(); //  하이어라키에서 `Boss` 스크립트가 있는 오브젝트 찾기
        }

        if ((int)FruitsID == 0)
        {
            Debug.LogWarning($"{gameObject.name}의 FruitID가 설정되지 않았습니다! 초기화가 필요합니다.");
            AssignFruitID();
            return;
        }

        StopAllCoroutines(); //  기존 코루틴 중지하여 중복 실행 방지
        StartCoroutine(ShootCoroutine());
        StartCoroutine(UpdateCoinCoroutine());
    }

    #region 유닛 ID 할당 및 코인 업데이트 코루틴
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
        if ((int)FruitsID != 0) return;

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
        bullet.ReturnMyComponent<Bullet>().Initialize(position, direction, ownerTag);
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

        Vector2 direction = (Boss.transform.position - FirePoint.position).normalized;
        CreateBullet(Tag.Bullet, FirePoint.position, direction, gameObject.tag);

    }
    #endregion
}
