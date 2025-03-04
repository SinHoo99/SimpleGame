using UnityEngine;
using System;
using DG.Tweening;

public class Boss : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    private BossRuntimeData BossRuntimeData => GM.BossDataManager.BossRuntimeData; // 현재 보스의 동적 데이터 저장

    public BossData BossData; // 현재 보스의 기본 정보
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private BoxCollider2D boxCollider;

    public HealthSystem HealthSystem;
    public event Action OnChangeBossHP;

    [SerializeField] private HealthStatusUI HealthStatusUI;

    [Header("소리 조절")]
    private float lastSFXTime = 0f;
    private float sfxCooldown = 2f;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        HealthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        InitializeBoss();
    }

    private void OnEnable()
    {
        HealthStatusUI.SetStatusEvent();
        Respawn();
    }

    ///  보스 초기화 로직을 하나로 정리
    private void InitializeBoss()
    {
        BossData = GM.GetBossData(GM.BossDataManager.BossRuntimeData.CurrentBossID);
        // 체력 시스템 설정
        InitHealth();
        HealthSystem.OnDeath -= OnDie;
        HealthSystem.OnDeath += OnDie;

        HealthSystem.OnChangeHP -= HandleChangeHP;
        HealthSystem.OnChangeHP += HandleChangeHP;

        // UI 및 상태 초기화
        InitBossHPBar();
        UpdateBossAnimation();
        GM.PlayerStatusUI.BossStatus();
    }

    private void HandleChangeHP()
    {
        OnChangeBossHP?.Invoke();
    }

    private void OnDie()
    {
        spriteRenderer.enabled = false;
        boxCollider.enabled = false;
        HealthStatusUI.HideSlider();
        var nextBossID = GetNextBossID();
        Debug.Log($"[Boss] 현재 보스 ID: {BossRuntimeData.CurrentBossID} → 다음 보스 ID: {nextBossID}");
        BossRuntimeData.CurrentBossID = nextBossID;
        Invoke(nameof(Respawn), 3f);
    }

    private void Respawn()
    {
        InitializeBoss();
        spriteRenderer.enabled = true;
        boxCollider.enabled = true;
    }

    public void TakeDamage(float damage)
    {
        if (HealthSystem != null)
        {
            if (HealthSystem.CurHP <= 0)
            {
                Debug.Log("[Boss] 이미 사망한 상태입니다. OnDeath 중복 호출 방지!");
                return; // 이미 죽은 상태라면 중복 호출 방지
            }

            HealthSystem.TakeDamage(damage);
            BossRuntimeData.CurrentHealth = HealthSystem.CurHP; // 체력 갱신
            TakeDamageEffect();
        }
    }

    private BossID GetNextBossID()
    {
        return (BossRuntimeData.CurrentBossID < BossID.E) ? BossRuntimeData.CurrentBossID + 1 : BossID.A;
    }

    public void UpdateBossAnimation()
    {
        animator.SetTrigger(BossRuntimeData.CurrentBossID.ToString());
    }
    #region Boss Health 관련
    private void InitBossHPBar()
    {
        HealthStatusUI.UpdateHPStatus();
        HealthStatusUI.ShowSlider();
    }

    private void InitHealth()
    {
        BossData = GM.GetBossData(BossRuntimeData.CurrentBossID);

        if (BossRuntimeData.CurrentHealth <= 0)
        {
            BossRuntimeData.CurrentHealth = BossData.MaxHealth;
        }

        if (HealthSystem != null)
        {
            HealthSystem.MaxHP = BossData.MaxHealth;
            HealthSystem.InitHP(BossRuntimeData.CurrentHealth, BossData.MaxHealth);
        }
    }

    public void ResetBossHealth()
    {
        BossRuntimeData.CurrentHealth = BossData.MaxHealth;

        if (HealthSystem != null)
        {
            HealthSystem.MaxHP = BossData.MaxHealth;
            HealthSystem.InitHP(BossRuntimeData.CurrentHealth, BossData.MaxHealth);
        }
        HealthStatusUI.UpdateHPStatus();
        GM.PlayerStatusUI.BossStatus();
    }
    #endregion
    public void ResetBossData()
    {
        BossData = GM.GetBossData(BossRuntimeData.CurrentBossID);

        ResetBossHealth();
        UpdateBossAnimation();

        Debug.Log($"[Boss] 보스 데이터가 초기화되었습니다: {BossRuntimeData.CurrentBossID}, 체력: {BossRuntimeData.CurrentHealth}");
    }

    #region 보스 타격 효과 관련
    private void TakeDamageEffect()
    {
        ColorEffect();
        ScaleEffect();
        //PlayLimitedSFX();
    }
    private void ColorEffect()
    {
        spriteRenderer.DOColor(Color.red, 0.1f).OnComplete(() => spriteRenderer.DOColor(Color.white, 0.1f));
    }
    private void ScaleEffect()
    {
        transform.DOScale(new Vector3(1.2f, 0.8f, 1f), 0.1f).OnComplete(() => transform.DOScale(Vector3.one, 0.1f));
    }

    private void PlayLimitedSFX()
    {
        if (Time.time - lastSFXTime < sfxCooldown) return;
        lastSFXTime = Time.time;

        GM.PlaySFX(SFX.TakeDamage);
    }
    #endregion
}
