using UnityEngine;
using System;
using System.Collections;

public class Boss : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    private BossRuntimeData BossRuntimeData => GM.BossDataManager.BossRuntimeData;// 현재 보스의 동적데이터 저장용 프로퍼티

    public BossData BossData; //현재 보스의 정적 데이터 (보스의 기본 정보)
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private BoxCollider2D boxCollider;

    public HealthSystem HealthSystem;
    public event Action OnChangeBossHP;
   
    public BossID BossID;
    [SerializeField] private HealthStatusUI HealthStatusUI;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        BossData = GM.GetBossData(GM.BossDataManager.BossRuntimeData.CurrentBossID);
        HealthSystem = GetComponent<HealthSystem>();
        InitHealth();
        HealthSystem.OnDeath += OnDie;
        HealthSystem.OnChangeHP += HandleChangeHP;
        InitBossHPBar();
    }
    private void OnEnable()
    {
        HealthStatusUI.SetStatusEvent();
        Respawn();
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

        BossID = GetNextBossID();
        BossRuntimeData.CurrentBossID = BossID;
        Invoke(nameof(Respawn), 3f);
    }
    private void Respawn()
    {
        InitHealth();
        InitBossHPBar();
        spriteRenderer.enabled = true;
        boxCollider.enabled = true;
        UpdateBossAnimation();
    }


    public void TakeDamage(float damage)
    {
        if (HealthSystem != null)
        {
            HealthSystem.TakeDamage(damage);
            BossRuntimeData.CurrentHealth = HealthSystem.CurHP; // 체력 갱신
            StartCoroutine(TakeDamageCoroutine());
        }
    }

    private IEnumerator TakeDamageCoroutine()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private BossID GetNextBossID()
    {
        return (BossID < BossID.E) ? BossID + 1 : BossID.A;
    }

    public void UpdateBossAnimation()
    {
        animator.SetTrigger(BossID.ToString());
    }

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
}
