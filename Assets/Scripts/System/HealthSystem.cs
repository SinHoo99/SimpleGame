using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour
{
    // ü�� �ʵ�
    public float MaxHP;
    public float CurHP; //���� ü��

    public event Action OnChangeHP;

    // ��� �̺�Ʈ
    public event Action OnDeath;
    public bool IsDead => CurHP == 0;

    public void InitHP(float curHP, float maxHP)
    {
        CurHP = curHP;
        MaxHP = maxHP;
    }

    // ü���� ���ҽ�Ű�� �޼���
    public void TakeDamage(float damage)
    {
        CurHP -= damage;
        if (CurHP <= 0)
        {
            CurHP = 0;
            OnDeath?.Invoke();
        }

        OnChangeHP?.Invoke();
    }

}
