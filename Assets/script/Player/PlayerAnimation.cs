using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    public AnimatorStateInfo stateInfo;
    private Coroutine chargeCoroutine;
    private float chargeTimer = 0f;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
    }
    private void Update()
    {
        SetAnimation();
    }
    private void SetAnimation()
    {
        animator.SetFloat("velocityx", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("velocityy", rb.velocity.y);
        animator.SetBool("island", physicsCheck.isGround);
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        animator.SetBool("isAttack", stateInfo.IsTag("attack"));
    }
    //���O��{
    public void StartChargeMonitor(System.Action onForceRelease)
    {
        if (chargeCoroutine != null)
            StopCoroutine(chargeCoroutine);

        chargeCoroutine = StartCoroutine(ChargeAnimationMonitor(onForceRelease));
    }
    private IEnumerator ChargeAnimationMonitor(System.Action onForceRelease)
    {
        while (true)
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            //Debug.Log($"Current State: {stateInfo.fullPathHash}, Normalized Time: {chargeTimer}");
            chargeTimer += Time.deltaTime;
            if (chargeTimer >= 2f)
            {
                //Debug.Log("Charge�ʵe���F3���A�۰�����I");
                chargeTimer = 0;
                onForceRelease?.Invoke(); // �q���~���j������
                yield break;
            }

            yield return null;
        }
    }
    public void StopChargeMonitor()
    {
        if (chargeCoroutine != null)
        {
            StopCoroutine(chargeCoroutine);
            chargeCoroutine = null;
        }
    }
    //�~�����f
    public void PlayAttack(int index)
    {
        animator.SetTrigger("attack");
        //Debug.Log($"attack{index}");
    }

    public void PlayTrigger(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }

    public void SetBool(string param, bool value)
    {
        animator.SetBool(param, value);
    }

    public void SetFloat(string param, float value)
    {
        animator.SetFloat(param, value);
    }
    public bool IsAttack()
    {
        return stateInfo.IsTag("attack");
    }
    public Animator GetAnimator() => animator;

}
