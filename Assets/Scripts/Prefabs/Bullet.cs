using System.Collections;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Bullet : PoolObject
{
    private GameManager GM => GameManager.Instance;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private string bulletOwnerTag;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    #region ºÒ·¿ Ãæµ¹
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == LayerMask.NameToLayer(Layer.Boss)))
        {
            gameObject.SetActive(false);
        }
    }
    #endregion

    private void OnAfterInitialize()
    {
        StartCoroutine(DeactivateAfterDelay(3f));
    }

    private IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
    public void Initialize(Vector2 position, Vector2 direction, string ownerTag)
    {
        bulletOwnerTag = ownerTag;
        transform.position = position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        rb.velocity = direction.normalized * 10f;

        OnAfterInitialize();
    }

}
