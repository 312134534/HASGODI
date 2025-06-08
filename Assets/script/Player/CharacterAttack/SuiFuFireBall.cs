using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuiFuFireBall : BaseBullet
{
    protected override void Awake()
    {
        base.Awake();
        rb.gravityScale = 0f; // �T�έ��O
    }

    public override void Launch(Vector2 direction, float speed, string tag)
    {
        playerTag = tag;
        rb.velocity = direction.normalized * speed;
        transform.right = direction; // ��V�����V�]�i��^
    }

    protected override void OnHit(Collider2D collision)
    {
        // ���n����ۤv
        if (collision.CompareTag(playerTag) || collision.CompareTag("suifuFireBall") || collision.CompareTag("hansbullet"))
        {
            Debug.Log("Hit own player, ignore.");
            return;
        }

        Destroy(gameObject);
    }
}
