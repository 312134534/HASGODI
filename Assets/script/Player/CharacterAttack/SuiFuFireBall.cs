using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuiFuFireBall : BaseBullet
{
    protected override void Awake()
    {
        base.Awake();
        rb.gravityScale = 0f; // 禁用重力
    }

    public override void Launch(Vector2 direction, float speed, string tag)
    {
        playerTag = tag;
        rb.velocity = direction.normalized * speed;
        transform.right = direction; // 轉向飛行方向（可選）
    }

    protected override void OnHit(Collider2D collision)
    {
        // 不要打到自己
        if (collision.CompareTag(playerTag) || collision.CompareTag("suifuFireBall") || collision.CompareTag("hansbullet"))
        {
            Debug.Log("Hit own player, ignore.");
            return;
        }

        Destroy(gameObject);
    }
}
