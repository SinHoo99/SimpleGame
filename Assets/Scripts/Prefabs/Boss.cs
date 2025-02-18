using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    public BossID bossID; // 현재 보스 ID
    public int maxHealth; // 최대 체력
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
        Debug.Log($"현재 보스: {bossID}, MaxHealth: {maxHealth}");
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
            Debug.Log($"현재 보스: {bossID}, MaxHealth: {maxHealth}");
            UpdateBossAnimation();
        }
        else
        {
            Debug.LogError($"BossID {bossID}에 해당하는 데이터 없음!");
        }

        spriteRenderer.enabled = true;
        boxCollider.enabled = true;
    }

    //  다음 보스 ID를 가져오는 메서드
    private BossID GetNextBossID()
    {
        return (bossID < BossID.E) ? bossID + 1 : BossID.A;
    }

    private void UpdateBossAnimation()
    {
        animator.SetTrigger(bossID.ToString());
    }

}
