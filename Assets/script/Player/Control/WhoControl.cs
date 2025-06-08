using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[DisallowMultipleComponent]
public class WhoControl : MonoBehaviour, IInputReader
{
    public PlayerType playerType;//�bgenerate�ɳB�z
    public Vector2 move { get; private set; }
    public event Action JumpEvent;
    public event Action CloseCombatEvent;
    public event Action DodgeEvent;
    public event Action RangedAttackEvent;
    public event Action StartChargeEvent;
    public event Action EndChargeEvent;

    public ActionControll inputActions;
    private void Awake()
    {
        inputActions = new ActionControll();
    }
    public void Update()
    {
        GetInputVector();
    }
    public void Enable()
    {
        inputActions.Enable();
        DecideAction();
    }
    private void OnDisable()
    {
        DisableAction();
        inputActions.Disable();
    }
    //�V�q��
    private void GetInputVector()
    {
        switch(playerType)
        {
            case PlayerType.player1:
                move = inputActions.Player1.Move.ReadValue<Vector2>();
                break;
            case PlayerType.player2:
                move = inputActions.Player2.Move.ReadValue<Vector2>();
                break;
        }
    }
    //�ƥ���
    private void DecideAction()
    {
        switch (playerType)
        {
            case PlayerType.player1:
                inputActions.Player1.Jump.started += OnJump;
                inputActions.Player1.CloseCombat.started += OnCloseCombat;
                inputActions.Player1.Dodge.started += OnDodge;
                inputActions.Player1.RangedAttack.started += OnRangedAttack;
                inputActions.Player1.RangedAttack.started += OnStartCharge;
                inputActions.Player1.RangedAttack.canceled += OnEndCharge;
                break;
            case PlayerType.player2:
                inputActions.Player2.Jump.started += OnJump;
                inputActions.Player2.CloseCombat.started += OnCloseCombat;
                inputActions.Player2.Dodge.started += OnDodge;
                inputActions.Player2.RangedAttack.started += OnRangedAttack;
                inputActions.Player2.RangedAttack.started += OnStartCharge;
                inputActions.Player2.RangedAttack.canceled += OnEndCharge;
                break;
        }
    }
    public void DisableAction()
    {
        switch (playerType)
        {
            case PlayerType.player1:
                inputActions.Player1.Jump.started -= OnJump;
                inputActions.Player1.CloseCombat.started -= OnCloseCombat;
                inputActions.Player1.Dodge.started -= OnDodge;
                inputActions.Player1.RangedAttack.started -= OnRangedAttack;
                inputActions.Player1.RangedAttack.started -= OnStartCharge;
                inputActions.Player1.RangedAttack.canceled -= OnEndCharge;
                break;
            case PlayerType.player2:
                inputActions.Player2.Jump.started -= OnJump;
                inputActions.Player2.CloseCombat.started -= OnCloseCombat;
                inputActions.Player2.Dodge.started -= OnDodge;
                inputActions.Player2.RangedAttack.started -= OnRangedAttack;
                inputActions.Player2.RangedAttack.started -= OnStartCharge;
                inputActions.Player2.RangedAttack.canceled -= OnEndCharge;
                break;
        }
    }
    //��ԧ�������
    private void OnCloseCombat(InputAction.CallbackContext context)
    {
        CloseCombatEvent?.Invoke();
    }
    //���D����
    private void OnJump(InputAction.CallbackContext context)
    {
        JumpEvent?.Invoke();
    }
    //�{�ױ���
    private void OnDodge(InputAction.CallbackContext context)
    {
        DodgeEvent?.Invoke();
    }
    //����
    private void OnRangedAttack(InputAction.CallbackContext context)
    {
        Debug.Log("test ranged attack");
        RangedAttackEvent?.Invoke();
    }
    //���O����
    private void OnStartCharge(InputAction.CallbackContext context)
    {
        StartChargeEvent?.Invoke();
    }
    private void OnEndCharge(InputAction.CallbackContext context)
    {
        EndChargeEvent?.Invoke();
    }
}
