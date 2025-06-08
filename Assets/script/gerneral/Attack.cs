using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float damage;
    public float attackRange;
    public float attackRate;
    public float pushForce;
    public float reducePower = 0;
    public Character shooter;
    private HashSet<Character> damagedCharacters = new HashSet<Character>();

    private void OnEnable()
    {
        // 攻擊啟動時清空紀錄
        damagedCharacters.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Character target = other.GetComponent<Character>();
        if (target != null && !damagedCharacters.Contains(target) && target != shooter)
        {
            target.TakeDamage(this);
            damagedCharacters.Add(target);
        }
    }

}
