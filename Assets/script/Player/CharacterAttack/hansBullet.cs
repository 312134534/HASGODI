using UnityEngine;

public class hansBullet : BaseBullet
{
    public override void Launch(Vector2 direction, float force, string tag)
    {
        playerTag = tag;
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1f;

        //Debug.Log("Launch direction: " + direction.normalized + ", force: " + force);

        rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
    }

    void FixedUpdate()
    {
        // 根據速度自動旋轉砲彈朝向
        if (rb.velocity.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    protected override void OnHit(Collider2D collision)
    {
        // 不要打到自己
        if (collision.CompareTag(playerTag) || collision.CompareTag("hansbullet") || collision.CompareTag("suifuFireBall"))
        {
            Debug.Log("Hit own player, ignore.");
            return;
        }

        Debug.Log("Cannonball hit: " + collision.name);
        Destroy(gameObject);
    }
}
