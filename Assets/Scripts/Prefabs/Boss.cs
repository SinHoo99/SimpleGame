using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;


public class Boss : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    public BossID bossID;
    public int maxHealth;
    private int currentHealth;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private BoxCollider2D boxCollider;


    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        Respawn();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        spriteRenderer.enabled = false;
        boxCollider.enabled = false;
        bossID = GetNextBossID();
        Invoke(nameof(Respawn), 3f);
    }

    private void Respawn()
    {
        if (GM.DataManager.BossDatas.TryGetValue(bossID, out BossData bossData))
        {
            maxHealth = bossData.MaxHealth;
            currentHealth = maxHealth;
            UpdateBossAnimation();
        }
        spriteRenderer.enabled = true;
        boxCollider.enabled = true;
    }

    private BossID GetNextBossID()
    {
        return (bossID < BossID.E) ? bossID + 1 : BossID.A;
    }

    private void UpdateBossAnimation()
    {
        animator.SetTrigger(bossID.ToString());
    }

}
