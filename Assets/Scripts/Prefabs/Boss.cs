using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    private SpriteRenderer spriteRenderer;
    private Collider2D bossCollider;

    [SerializeField] private float respawnTime = 3f; // ��Ȱ ��� �ð�

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        bossCollider = GetComponent<Collider2D>(); //  �浹 ������ ���� �ݶ��̴� �߰�
    }

    private void OnEnable()
    {
        currentHealth = maxHealth; //  ���� ü�� �ʱ�ȭ
        ActivateBoss(); //  ���� �ٽ� ���̰� ����
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
        Debug.Log("���� ���! " + respawnTime + "�� �� ��Ȱ.");
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
        spriteRenderer.enabled = true; // ��������Ʈ �ٽ� ���̰� ����
        bossCollider.enabled = true; // �浹 �ٽ� Ȱ��ȭ
        currentHealth = maxHealth; // ü�� �ʱ�ȭ
        Debug.Log("���� ��Ȱ!");
    }

    //  ������ ��Ȱ��ȭ�ϴ� �Լ� (��������Ʈ �� �ݶ��̴� ����)
    private void DeactivateBoss()
    {
        spriteRenderer.enabled = false; // ��������Ʈ ����
        bossCollider.enabled = false; // �浹 ��Ȱ��ȭ
    }
}
