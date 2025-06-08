using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [Header("膀セ把计")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCD = 0.2f;
    [SerializeField] private float lastJump = -Mathf.Infinity;

    [Header("皗磷把计")]
    [SerializeField] private float dodgeForce = 25f;
    [SerializeField] private float dodgeDuration = 0.15f; // ㎝笆礶妓
    [SerializeField] private float dodegeCosPower = 30f;
    [SerializeField] private AnimationClip dodgeAnimationClip;

    [Header("集揽干纕把计")]
    [SerializeField] private float airControlLerp = 5f; // 北い硉干纕眏

    private AttackBase attack;
    private IInputReader inputReader;
    private Rigidbody2D rb;
    private Character character;
    private Vector2 moveVector;
    private PhysicsCheck physicsCheck;
    private Animator animator;
    private AfterimageSpawner afterimageSpawner;
    private bool isDodging = false;
    private bool isFrozen = false;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        if(gameObject.tag == "player2" && PlayerPrefs.GetInt("is all player") == 0)
        {
            inputReader = GetComponent<AIInputReader>();
        }
        else
        {
            inputReader = GetComponent<WhoControl>();
        }
        attack = GetComponent<AttackBase>();
        animator = GetComponent<Animator>();
        character = GetComponent<Character>();
        afterimageSpawner = GetComponent<AfterimageSpawner>();
        inputReader.JumpEvent += OnJump;
        inputReader.CloseCombatEvent += attack.CloseCombat;
        inputReader.DodgeEvent += Ondodge;
        inputReader.RangedAttackEvent += attack.RangedAttack;
        inputReader.StartChargeEvent += attack.StartCharge;
        inputReader.EndChargeEvent += attack.ReleaseCharge;
    }

    private void Update()
    {
        moveVector = inputReader.move;
    }

    private void FixedUpdate()
    {
        if (isDodging) return;

        Move();

        if (!physicsCheck.isGround)
        {
            float targetVelocityX = speed * moveVector.x * Time.fixedDeltaTime;
            float compensatedX = Mathf.Lerp(rb.velocity.x, targetVelocityX, airControlLerp * Time.fixedDeltaTime);
            rb.velocity = new Vector2(compensatedX, rb.velocity.y);
        }
    }

    private void OnDisable()
    {
        inputReader.JumpEvent -= OnJump;
        inputReader.CloseCombatEvent -= attack.CloseCombat;
    }
    //簿笆
    private void Move()
    {
        if (!animator.GetBool("isAttack") && !isFrozen) rb.velocity = new Vector2(speed * Time.deltaTime * moveVector.x, rb.velocity.y);
        if (moveVector.x != 0) transform.localScale = new Vector3(moveVector.x > 0 ? 1 : -1, 1, 1);
    }
    //铬臘
    private void OnJump()
    {
        if (Time.time - lastJump < jumpCD || isFrozen) return;
        if(physicsCheck.isGround) 
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        lastJump = Time.time;
    }
    //皗磷
    public void Ondodge()
    {
        if (isDodging || animator.GetBool("isAttack")) return;

        StartCoroutine(DodgeRoutine());
    }

    private IEnumerator DodgeRoutine()
    {
        if (character.currentPower - dodegeCosPower < 0) yield break;
        isDodging = true;
        animator.SetTrigger("dodge");
        character.TakePower(dodegeCosPower);
        character.SetTemporaryInvulnerability(dodgeDuration);

        float dodgeDirection = transform.localScale.x > 0 ? 1f : -1f;
        rb.velocity = new Vector2(dodgeDirection * dodgeForce, rb.velocity.y);

        afterimageSpawner.StartSpawning();
        yield return new WaitForSeconds(dodgeDuration);
        afterimageSpawner.StopSpawning();

        isDodging = false;
    }
    //阑
    public void FreezeForSeconds(float duration)
    {
        if (!isFrozen)
            StartCoroutine(FreezeCoroutine(duration));
    }

    private IEnumerator FreezeCoroutine(float duration)
    {
        isFrozen = true;
        animator.SetBool("isPowerZero", true);
        moveVector = Vector2.zero;
        rb.velocity = new Vector2(0, rb.velocity.y);

        yield return new WaitForSeconds(duration);
        animator.SetBool("isPowerZero", false);
        GetComponent<Character>().currentPower = GetComponent<Character>().maxPower;
        isFrozen = false;
    }

}
