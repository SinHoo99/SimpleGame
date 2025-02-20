using UnityEngine;
using System;
using System.Collections;

public class Boss : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private BoxCollider2D boxCollider;

    public HealthSystem HealthSystem;
    public float BossCurHP => HealthSystem.CurHP;
    public event Action OnChangeBossHP;
    public BossData BossData;
    [SerializeField] private BossID BossID;
    [SerializeField] private HealthStatusUI HealthStatusUI;
    private void Awake()
    {
        BossData = GM.GetBossData(BossID);
        HealthSystem = GetComponent<HealthSystem>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

        if (HealthSystem != null)
        {
            HealthSystem.InitHP(BossData.MaxHealth, BossData.MaxHealth);
            HealthSystem.OnDeath += OnDie;
            HealthSystem.OnChangeHP += HandleChangeHP;
        }

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
        BossData = GM.GetBossData(BossID);
        Invoke(nameof(Respawn), 3f);
    }

    public void TakeDamage(float damage)
    {
        if (HealthSystem != null)
        {
            HealthSystem.TakeDamage(damage);
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
        HealthSystem.InitHP(BossData.MaxHealth, BossData.MaxHealth);
        InitBossHPBar();
        spriteRenderer.enabled = true;
        boxCollider.enabled = true;
        UpdateBossAnimation();
    }

    private BossID GetNextBossID()
    {
        return (BossID < BossID.E) ? BossID + 1 : BossID.A;
    }

    private void UpdateBossAnimation()
    {
        animator.SetTrigger(BossID.ToString());
    }

    private void InitBossHPBar()
    {
        HealthStatusUI.UpdateHPStatus();
        HealthStatusUI.ShowSlider();
    }
}
