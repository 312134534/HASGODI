using UnityEngine;

public class AIBrain : MonoBehaviour
{
    public AIGraph graph;
    public Transform Target;
    public AIInputReader Input;
    public LayerMask wallLayer;
    public Transform isWallPosition;
    public float wallCheckDistance = 1f;
    public LayerMask bulletLayer;
    public float detectRadius = 5f;
    public float dotThreshold = 0.8f;

    public bool FacingRight { get; set; }
    public bool IsHittingWall { get; set; }
    public bool IsOnPlayer;

    [Header("AI更新間隔")]
    public float thinkInterval = 0.01f;
    private float thinkTimer = 0f;

    [Header("牆壁判定冷卻")]
    public float wallCheckCooldown = 0.5f;
    private float wallCheckTimer = 0f;

    private void Start()
    {
        Target = GameObject.FindWithTag("player1")?.transform;

        var entry = graph.nodes.Find(n => n is AIEntryNode) as AIEntryNode;
        if (entry != null) entry.Execute(this);
    }

    private void Update()
    {
        thinkTimer -= Time.deltaTime;
        wallCheckTimer -= Time.deltaTime;

        FacingRight = transform.localScale.x > 0;

        CheckWall();
        IsOnPlayer = GetComponent<PhysicsCheck>().isOnPlayer;
        if (thinkTimer <= 0f)
        {
            thinkTimer = thinkInterval;

            var entry = graph.nodes.Find(n => n is AIEntryNode) as AIEntryNode;
            if (entry != null) entry.Execute(this);
        }
    }

    public void CheckWall()
    {

        Vector2 direction = FacingRight ? Vector2.right : Vector2.left;
        Vector2 boxSize = new Vector2(wallCheckDistance, wallCheckDistance); // 掃描區域的寬高
        float distance = wallCheckDistance;

        RaycastHit2D hit = Physics2D.BoxCast(isWallPosition.position, boxSize, 0f, direction, distance, wallLayer);
        IsHittingWall = hit.collider != null;

    }
    void OnDrawGizmosSelected()
    {
        if (isWallPosition == null) return;

        Gizmos.color = Color.red;
        Vector2 dir = FacingRight ? Vector2.right : Vector2.left;
        Gizmos.DrawLine(isWallPosition.position, isWallPosition.position + (Vector3)dir * wallCheckDistance);
    }

    public bool DetectIncomingBullets()
    {
        Collider2D[] bullets = Physics2D.OverlapCircleAll(isWallPosition.position, detectRadius, bulletLayer);

        foreach (var col in bullets)
        {
            Rigidbody2D bulletRb = col.attachedRigidbody;
            if (bulletRb == null) continue;

            Vector2 toSelf = (Vector2)(transform.position - col.transform.position).normalized;
            Vector2 bulletDir = bulletRb.velocity.normalized;

            float dot = Vector2.Dot(bulletDir, toSelf);
            if (dot > dotThreshold)
            {
                //Debug.Log("子彈正朝我飛來！位置：" + col.transform.position + " dot=" + dot);
                return true;
            }
        }
        return false;
    }
}
