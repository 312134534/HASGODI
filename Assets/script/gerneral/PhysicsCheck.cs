using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    public bool isGround;
    public bool isOnPlayer;
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    public float checkCircle;
    void Update()
    {
        Check();
        CheckOnPlayer();
    }
    private void Check()
    {
        isGround = Physics2D.OverlapCircle(transform.position, checkCircle, groundLayer);
    }
    private void CheckOnPlayer()
    {
        isOnPlayer = Physics2D.OverlapCircle(transform.position, checkCircle, playerLayer);
    }
}
