using System.Collections;
using UnityEngine;

public class UnitController : PoolObject
{
    public bool IsWalk = true;
    public float Speed = 3f;
    public float MinChangeTime = 1f;
    public float MaxChangeTime = 4f;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private string _prefabsOwnerTag;
    private int _prefabsOwnerID;
    private ObjectPool _objectPool;
    public FruitsID FruitsID { get; set; }

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        moveDirection = Vector2.zero;
        StartCoroutine(UnitMove());
    }

    IEnumerator UnitMove()
    {
        while (true)
        {
            if (IsWalk)
            {
                moveDirection = Random.insideUnitCircle.normalized;
                Flip(moveDirection.x > 0);
                animator.SetBool("IsWalk", true);
            }
            else
            {
                moveDirection = Vector2.zero;
                animator.SetBool("IsWalk", false);
            }

            rb.velocity = moveDirection * Speed;

            yield return new WaitForSeconds(Random.Range(MinChangeTime, MaxChangeTime));

            moveDirection = Random.insideUnitCircle.normalized;
            Flip(moveDirection.x > 0);
        }
    }
    void Flip(bool isRight)
    {
        spriteRenderer.flipX = !isRight;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Fence"))
        {
            moveDirection = (Random.insideUnitCircle.normalized - moveDirection).normalized;
            rb.velocity = moveDirection * Speed;
            Flip(moveDirection.x > 0);
        }
    }
    public void Initialize(string tag, int ownerID)
    {
        _prefabsOwnerTag = tag;
        FruitsID = (FruitsID)ownerID;
        Debug.Log($"[UnitController] {gameObject.name}ÀÌ(°¡) »ý¼ºµÊ. FruitID: {FruitsID}");
    }

}
