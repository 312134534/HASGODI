using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuifuAttack : AttackBase
{
    private bool isCharging = false;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float ManaConsume = 20f;
    private Coroutine chargeRoutine;
    private float chargeLevel = 1f;
    public override void CloseCombat()
    {
       playerAnimation.PlayAttack(combo);
    }

    public override void RangedAttack()
    {
        playerAnimation.PlayTrigger("shoot");
    }

    public override void StartCharge()
    {
        if (isCharging || character.currentMana < ManaConsume) return;
        isCharging = true;
        playerAnimation.SetBool("isCharge", true);
        playerAnimation.StartChargeMonitor(ReleaseCharge);
        StartCoroutine(ChargeLevelRoutine());
    }
    private IEnumerator ChargeLevelRoutine()
    {
        float maxChargeTime = 2f;
        float elapsed = 0f;

        while (isCharging)
        {
            elapsed += Time.deltaTime;
            chargeLevel = Mathf.Lerp(1f, 3f, elapsed / maxChargeTime); // 1~3 倍之間
            yield return null;
        }
    }
    public override void ReleaseCharge()
    {
        if (!isCharging) return;
        isCharging = false;
        StopCoroutine(ChargeLevelRoutine()); // 停止蓄力提升
        character.TakeMana(ManaConsume);
        playerAnimation.SetBool("isCharge", false);

        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        BaseBullet bullet = bulletObj.GetComponent<BaseBullet>();

        bullet.SetChargeLevel(chargeLevel); // 套用蓄力倍率

        float directionX = transform.localScale.x > 0 ? 1f : -1f;
        Vector2 shootDir = new Vector2(directionX, 0f);
        bullet.GetComponent<Attack>().shooter = character;
        bullet.GetComponent<Attack>().damage *= chargeLevel;
        bullet.Launch(shootDir, 10f, gameObject.tag); // 發射

        chargeLevel = 1f; // 重置
    }
}
