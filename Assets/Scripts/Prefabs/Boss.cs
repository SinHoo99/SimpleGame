using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    private SpriteRenderer spriteRenderer;
    private Collider2D bossCollider;

    [SerializeField] private float respawnTime = 3f; // 부활 대기 시간

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        bossCollider = GetComponent<Collider2D>(); //  충돌 감지를 위한 콜라이더 추가
    }

    private void OnEnable()
    {
        currentHealth = maxHealth; //  보스 체력 초기화
        ActivateBoss(); //  보스 다시 보이게 설정
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
        Debug.Log("보스 사망! " + respawnTime + "초 후 부활.");
        DeactivateBoss();
        StartCoroutine(RespawnBoss()); 
    }

    private IEnumerator RespawnBoss()
    {
        yield return new WaitForSeconds(respawnTime);
        ActivateBoss(); 
    }
    private void ActivateBoss()
    {
        spriteRenderer.enabled = true; // 스프라이트 다시 보이게 설정
        bossCollider.enabled = true; // 충돌 다시 활성화
        currentHealth = maxHealth; // 체력 초기화
        Debug.Log("보스 부활!");
    }

    //  보스를 비활성화하는 함수 (스프라이트 및 콜라이더 숨김)
    private void DeactivateBoss()
    {
        spriteRenderer.enabled = false; // 스프라이트 숨김
        bossCollider.enabled = false; // 충돌 비활성화
    }
}
