using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Jobs;

public class hansAttack : AttackBase
{
    public GameObject bullet;
    public Transform bulletPosition;
    public Attack attack;
    public float ManaConsume = 20f;
    public float force;
    private void Start()
    {
        bulletPosition =  transform.Find("bulletPosition");
    }
    public override void CloseCombat()
    {
        playerAnimation.PlayAttack(combo);
    }
    public override void RangedAttack()
    {
        if (character.currentMana < ManaConsume) return;
        character.TakeMana(ManaConsume);
        playerAnimation.PlayTrigger("shoot");

        // �P�_���⭱�V��V�]X �b��/�t�^
        float directionX = transform.localScale.x > 0 ? 1f : -1f;
        Vector2 shootDir = new Vector2(directionX, 0.75f); 

        // �l�u��l���׬� 0�A�Ѥl�u�ۤv����]�۰ʰl�ܳt�ס^
        GameObject go = Instantiate(bullet, bulletPosition.position, Quaternion.identity);
        go.GetComponent<Attack>().shooter = character;
        go.GetComponent<BaseBullet>().Launch(shootDir, force, this.tag);
    }
}
