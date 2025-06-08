using UnityEngine;

public abstract class BaseBullet : MonoBehaviour
{
    protected Rigidbody2D rb;
    public float lifetime = 5f;
    protected string playerTag;

    protected float chargeLevel = 1f; // 1 = ���q�A>1 = �W�O

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void SetChargeLevel(float level)
    {
        chargeLevel = Mathf.Clamp(level, 1f, 3f); // �i����̤j�W�O���v
        transform.localScale *= chargeLevel; // ��j�l�u�]�̷ӻW�O�^
    }

    public abstract void Launch(Vector2 direction, float speed, string tag);

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        OnHit(collision);
    }

    protected virtual void OnHit(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
