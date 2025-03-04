using System.Collections;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Bullet : PoolObject
{
    private GameManager GM => GameManager.Instance;
    private Rigidbody2D rb;
    private Animator animator;

    private string bulletOwnerTag;
    private float damage;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    #region ºÒ·¿ Ãæµ¹
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == LayerMask.NameToLayer(Layer.Boss)))
        {
            Boss boss = collision.GetComponent<Boss>();
            boss.TakeDamage(damage);
            BulletObjectreturn();
        }
    }
    #endregion

    public void BulletObjectreturn( )
    {
        gameObject.SetActive(false);
    }
    public void Initialize(Vector2 position, Vector2 direction, string ownerTag, float bulletDamage)
    {
        bulletOwnerTag = ownerTag;
        damage = bulletDamage;
        transform.position = position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        rb.velocity = direction.normalized * 10f;
    }
      
  /*  private void HitParticle()
    {
        ParticleSystem.transform.position = transform.position;
        ParticleSystem.MainModule main = ParticleSystem.main;
        main.startSpeed = new ParticleSystem.MinMaxCurve(0.5f, 2f);
        main.gravityModifier = 0.5f; 
        ParticleSystem.Play();   
    }

    private void FireParticle()
    {
        ParticleSystem.transform.position = transform.position;
        ParticleSystem.MainModule main = ParticleSystem.main;
        main.startSpeed = new ParticleSystem.MinMaxCurve(0.5f, 2f);
        ParticleSystem.Play();
    }*/
}
