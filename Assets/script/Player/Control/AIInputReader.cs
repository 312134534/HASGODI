using System;
using UnityEngine;

public class AIInputReader : MonoBehaviour, IInputReader
{
    public Vector2 move { get; set; } // �i�ѥ~������

    public event Action JumpEvent;
    public event Action CloseCombatEvent;
    public event Action DodgeEvent;
    public event Action RangedAttackEvent;
    public event Action StartChargeEvent;
    public event Action EndChargeEvent;

    // ���}��k���`�I��Ĳ�o�ƥ�
    public void Jump() => JumpEvent?.Invoke();
    public void CloseCombat() => CloseCombatEvent?.Invoke();
    public void Dodge() => DodgeEvent?.Invoke();
    public void RangedAttack()
    {
        RangedAttackEvent?.Invoke();
        StartChargeEvent?.Invoke();
        EndChargeDelayed();
    }
    public void SetMove(Vector2 vector2)
    {
        move = vector2;
    }
    public void Enable()
    {
        enabled = true;
    }
    private async void EndChargeDelayed()
    {
        await System.Threading.Tasks.Task.Delay(500);
        EndChargeEvent?.Invoke();
    }
}