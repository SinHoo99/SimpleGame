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
            Debug.LogError($"Respawn �� BossData�� null�Դϴ�! BossID: {BossID}");
            return;
        }

        BossRuntimeData.CurrentBossID = BossID;

        //  MaxHealth�� ���������� ����Ǵ��� Ȯ��
        Debug.Log($"[Respawn] ���� ID: {BossID}, MaxHealth ������Ʈ: {BossData.MaxHealth}");

        //  ���� ü���� 0�̸� MaxHealth�� �ʱ�ȭ
        if (BossRuntimeData.CurrentHealth <= 0)
        {
            BossRuntimeData.CurrentHealth = BossData.MaxHealth;
        }

        //  MaxHealth�� ��Ȯ�� ����
        HealthSystem.MaxHP = BossData.MaxHealth;
        HealthSystem.InitHP(BossRuntimeData.CurrentHealth, BossData.MaxHealth);

        Debug.Log($"[Respawn] ������ MaxHealth: {BossData.MaxHealth}, HealthSystem.MaxHP: {HealthSystem.MaxHP}");

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
            Debug.LogError("[InitHealth] NowBossData�� null�Դϴ�. �ʱ�ȭ�� �� �����ϴ�.");
            return;
        }

        //  ���� ü���� 0 ������ ��쿡�� �ʱ�ȭ
        if (BossRuntimeData.CurrentHealth <= 0)
        {
            Debug.Log("[InitHealth] ���� ü���� 0�̹Ƿ� MaxHealth�� ����.");
            BossRuntimeData.CurrentHealth = NowBossData.MaxHealth;
        }

        //  MaxHealth�� HealthSystem�� �ݿ�
        HealthSystem.MaxHP = NowBossData.MaxHealth;

        Debug.Log($"[InitHealth] ���� ü�� ����: {BossRuntimeData.CurrentHealth}/{NowBossData.MaxHealth}, HealthSystem.MaxHP: {HealthSystem.MaxHP}");

        if (HealthSystem != null)
        {
            HealthSystem.InitHP(BossRuntimeData.CurrentHealth, NowBossData.MaxHealth);
        }
        else
        {
            Debug.LogError("[InitHealth] HealthSystem�� null�Դϴ�.");
        }
    }


}
