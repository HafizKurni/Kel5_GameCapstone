using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f;
    public float chaseRange = 5f;
    public float attackRange = 1.5f;
    public float returnToPatrolDistance = 7f;

    [Header("Waypoints")]
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;

    [Header("References")]
    public Animator animator;
    public Transform player;

    [Header("Scale Settings")]
    public Vector3 enemyScale = new Vector3(1f, 1f, 1f);

    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float range;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private float distanceCollider;
    private float cooldownTimer = Mathf.Infinity;

    private bool isChasing = false;
    private bool isAttacking = false;

    private void Awake()
    {
        transform.localScale = enemyScale;
    }

    private void Update()
    {
        if (IsPlayerInRange())
        {
            if (!isChasing)
            {
                isChasing = true;
            }

            if (Vector2.Distance(transform.position, player.position) <= attackRange)
            {
                AttackPlayer();
            }
            else
            {
                ChasePlayer();
            }
        }
        else if (isChasing && Vector2.Distance(transform.position, player.position) > returnToPatrolDistance)
        {
            isChasing = false;
        }

        if (!isChasing)
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if (waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        MoveTowards(targetWaypoint);

        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Loop through waypoints
        }

        animator.SetBool("Moving", true);
    }

    private void ChasePlayer()
    {
        if (!isAttacking) // Hanya bergerak jika tidak sedang menyerang
        {
            MoveTowards(player);
        }
        animator.SetBool("Moving", !isAttacking);
    }

    private void MoveTowards(Transform target)
    {
        Vector2 direction = (target.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Flip sprite based on direction
        if (direction.x < 0)
            transform.localScale = new Vector3(-enemyScale.x, enemyScale.y, enemyScale.z);
        else
            transform.localScale = enemyScale;
    }

    private bool IsPlayerInRange()
    {
        return Vector2.Distance(transform.position, player.position) < chaseRange;
    }

    private void AttackPlayer()
    {
        cooldownTimer += Time.deltaTime;
        if (checkPlayer() && !isAttacking)
        {
            if (cooldownTimer >= attackCooldown)
            {
                isAttacking = true;
                cooldownTimer = 0;

                animator.SetTrigger("Attack");
                DamageEnemyToPlayer();
            }
        }
        else if (isAttacking && !checkPlayer())
        {
            isAttacking = false; // Reset attack state if player is out of range
        }
        else if (isAttacking && cooldownTimer >= attackCooldown)
        {
            isAttacking = false; // Reset setelah cooldown
        }
    }

    private bool checkPlayer()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * distanceCollider,
        new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }

    private void DamageEnemyToPlayer()
    {
        if (checkPlayer())
        {
            HealthSystem playerHealth = player.GetComponent<HealthSystem>();
            playerHealth.TakeDamage(damage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange); // Jangkauan pengejaran
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange); // Jangkauan serangan
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * distanceCollider,
        new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}