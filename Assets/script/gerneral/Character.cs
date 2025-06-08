using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("血量設定")]
    public float maxHealth = 100;
    public float currentHealth;

    [Header("精力設定")]
    public float maxPower = 100;
    public float currentPower; 
    public float powerRegenAmount = 20f;

    [Header("藍量設定")]
    public float maxMana = 100;
    public float currentMana;
    public float manaRegenAmount = 10f;

    [Header("受傷無敵")]
    public float invulnerableDuration;
    private float invulnerableCounter;
    public bool invulnerable;
    public event System.Action<Transform> OnTakeDamage;

    private Rigidbody2D rb;
    private void Start()
    {
        currentHealth = maxHealth;
        currentPower = maxPower;
        currentMana = maxMana;
        invulnerable = false;
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if(invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0) invulnerable = false;
        }
        RegeneratePower();
        RegenerateMana();
    }
    //減生命
    public void TakeDamage(Attack attacker)
    {
        if (invulnerable) return;
        if (currentHealth - attacker.damage > 0)
        {
            currentHealth -= attacker.damage;
            OnTakeDamage?.Invoke(attacker.transform);

            Vector2 knockbackDir = (transform.position - attacker.transform.position + new Vector3(1f, 0.5f)).normalized;
            rb.AddForce(knockbackDir * attacker.pushForce, ForceMode2D.Impulse);

            TriggerInvulnerable();
        }
        else
        {
            currentHealth = 0;
            OnTakeDamage?.Invoke(attacker.transform);
        }
        if (currentPower - attacker.reducePower >= 0) currentPower -= attacker.reducePower;
        else
        {
            GetComponent<PlayerController>().FreezeForSeconds(2f);
        }
    }
    //精力相關
    public void TakePower(float power)
    {
        currentPower -= power;
    }
    private void RegeneratePower()
    {
        if (currentPower >= maxPower)
            return;
        currentPower += powerRegenAmount * Time.deltaTime;
        currentPower = Mathf.Min(currentPower, maxPower); // 不超過最大值
            
    }
    //藍量相關
    public void TakeMana(float mana)
    {
        currentMana -= mana;
    }
    private void RegenerateMana()
    {
        if (currentMana >= maxMana)
            return;
        currentMana += manaRegenAmount * Time.deltaTime;
        currentMana = Mathf.Min(currentMana, maxMana); // 不超過最大值

    }

    //無敵相關
    public void TriggerInvulnerable()
    {
        if(invulnerable == false)
        {
            invulnerableCounter = invulnerableDuration;
            invulnerable = true;
        }
    }
    public void SetTemporaryInvulnerability(float duration)
    {
        invulnerable = true;
        invulnerableCounter = Mathf.Max(invulnerableCounter, duration);
    }
}
