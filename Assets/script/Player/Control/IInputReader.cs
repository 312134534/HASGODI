using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputReader
{
    Vector2 move { get; }
    event Action JumpEvent;
    event Action CloseCombatEvent;
    event Action DodgeEvent;
    event Action RangedAttackEvent;
    event Action StartChargeEvent;
    event Action EndChargeEvent;
}
