using UnityEngine;
using System;
using System.Collections;

public class Boss : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    private BossRuntimeData BossRuntimeData => GM.BossDataManager.BossRuntimeData;
    private BossData NowBossData => GM.BossDataManager.NowBossData;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private BoxCollider2D boxCollider;

    public HealthSystem HealthSystem;
    public event Action OnChangeBossHP;
    public BossData BossData;
    public BossID BossID;
    [SerializeField] private HealthStatusUI HealthStatusUI;

    private void Awake()
    {     
      
        BossID = GM.BossDataManager.BossRuntimeData.CurrentBossID;

        BossData = GM.GetBossData(BossID);
      
        HealthSystem = GetComponent<HealthSystem>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
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
        NowBossData.ID = BossID;
        HealthSystem.MaxHP = NowBossData.MaxHealth;
        BossRuntimeData.CurrentBossID = BossID; 
        BossData = GM.GetBossData(BossID);
        Invoke(nameof(Respawn), 3f);
    }

    public void TakeDamage(float damage)
    {
        if (HealthSystem != null)
        {
            HealthSystem.TakeDamage(damage);
            BossRuntimeData.CurrentHealth = HealthSystem.CurHP;
            GM.BossDataManager.SaveBossRuntimeData();
            StartCoroutine(TakeDamageCoroutine());
        }
    }

    private IEnumerator TakeDamageCoroutine()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private void Respawn()
    {
        BossData = GM.GetBossData(BossID);

        if (BossData == null)
        {
            Debug.LogError($"Respawn 시 BossData가 null입니다! BossID: {BossID}");
            return;
        }

        BossRuntimeData.CurrentBossID = BossID;

        //  MaxHealth가 정상적으로 변경되는지 확인
        Debug.Log($"[Respawn] 보스 ID: {BossID}, MaxHealth 업데이트: {BossData.MaxHealth}");

        //  기존 체력이 0이면 MaxHealth로 초기화
        if (BossRuntimeData.CurrentHealth <= 0)
        {
            BossRuntimeData.CurrentHealth = BossData.MaxHealth;
        }

        //  MaxHealth를 명확히 설정
        HealthSystem.MaxHP = BossData.MaxHealth;
        HealthSystem.InitHP(BossRuntimeData.CurrentHealth, BossData.MaxHealth);

        Debug.Log($"[Respawn] 설정된 MaxHealth: {BossData.MaxHealth}, HealthSystem.MaxHP: {HealthSystem.MaxHP}");

        InitBossHPBar();
        spriteRenderer.enabled = true;
        boxCollider.enabled = true;
        UpdateBossAnimation();
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
        if (NowBossData == null)
        {
            Debug.LogError("[InitHealth] NowBossData가 null입니다. 초기화할 수 없습니다.");
            return;
        }

        //  기존 체력이 0 이하일 경우에만 초기화
        if (BossRuntimeData.CurrentHealth <= 0)
        {
            Debug.Log("[InitHealth] 현재 체력이 0이므로 MaxHealth로 설정.");
            BossRuntimeData.CurrentHealth = NowBossData.MaxHealth;
        }

        //  MaxHealth를 HealthSystem에 반영
        HealthSystem.MaxHP = NowBossData.MaxHealth;

        Debug.Log($"[InitHealth] 보스 체력 설정: {BossRuntimeData.CurrentHealth}/{NowBossData.MaxHealth}, HealthSystem.MaxHP: {HealthSystem.MaxHP}");

        if (HealthSystem != null)
        {
            HealthSystem.InitHP(BossRuntimeData.CurrentHealth, NowBossData.MaxHealth);
        }
        else
        {
            Debug.LogError("[InitHealth] HealthSystem이 null입니다.");
        }
    }


}
