using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBase : MonoBehaviour
{
    public PlayerAnimation playerAnimation;
    public Character character;
    protected int combo = 1;
    protected virtual void Awake()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        character = GetComponent<Character>();
    }
    public abstract void CloseCombat();
    public virtual void RangedAttack()
    {

    }

    public virtual void StartCharge()
    {

    }

    public virtual void ReleaseCharge()
    {

    }

}
