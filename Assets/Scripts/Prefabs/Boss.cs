using UnityEngine;
using System;
using DG.Tweening;

public class Boss : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    private BossRuntimeData BossRuntimeData => GM.BossDataManager.BossRuntimeData; // ���� ������ ���� ������ ����

    public BossData BossData; // ���� ������ �⺻ ����
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private BoxCollider2D boxCollider;

    public HealthSystem HealthSystem;
    public event Action OnChangeBossHP;

    [SerializeField] private HealthStatusUI HealthStatusUI;

    [Header("�Ҹ� ����")]
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

    ///  ���� �ʱ�ȭ ������ �ϳ��� ����
    private void InitializeBoss()
    {
        BossData = GM.GetBossData(GM.BossDataManager.BossRuntimeData.CurrentBossID);
        // ü�� �ý��� ����
        InitHealth();
        HealthSystem.OnDeath -= OnDie;
        HealthSystem.OnDeath += OnDie;

        HealthSystem.OnChangeHP -= HandleChangeHP;
        HealthSystem.OnChangeHP += HandleChangeHP;

        // UI �� ���� �ʱ�ȭ
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
        Debug.Log($"[Boss] ���� ���� ID: {BossRuntimeData.CurrentBossID} �� ���� ���� ID: {nextBossID}");
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
                Debug.Log("[Boss] �̹� ����� �����Դϴ�. OnDeath �ߺ� ȣ�� ����!");
                return; // �̹� ���� ���¶�� �ߺ� ȣ�� ����
            }

            HealthSystem.TakeDamage(damage);
            BossRuntimeData.CurrentHealth = HealthSystem.CurHP; // ü�� ����
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
    #region Boss Health ����
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

        Debug.Log($"[Boss] ���� �����Ͱ� �ʱ�ȭ�Ǿ����ϴ�: {BossRuntimeData.CurrentBossID}, ü��: {BossRuntimeData.CurrentHealth}");
    }

    #region ���� Ÿ�� ȿ�� ����
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
