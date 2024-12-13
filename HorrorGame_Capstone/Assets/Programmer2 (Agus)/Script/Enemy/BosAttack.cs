using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BosAttack : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Ranged Attack")]
    [SerializeField] private List<Transform> firepoints;
    [SerializeField] private GameObject[] fireballs;

    [Header("Summon Enemy Parameters")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private float summonCooldown;
    private float summonCooldownTimer = Mathf.Infinity;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    [Header("Scaling Parameters")]
    [SerializeField] private float scaleSpeed = 1.0f;
    [SerializeField] private Color giantColor = Color.red;

    [Header("Movement Parameters")]
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private float moveSpeed = 2.0f;
    private int currentWaypointIndex = 0;

    //References
    private Animator anim;
    private EnemyHealth enemyHealth;
    private SpriteRenderer spriteRenderer;
    private GameObject player;
    private enum BossMode { Normal, Giant }
    private BossMode currentMode;
    private bool isScaling = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentMode = BossMode.Normal;
        UpdateScale();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        summonCooldownTimer += Time.deltaTime;

        if (enemyHealth.currentHealth <= enemyHealth.startingHealth / 2 && currentMode == BossMode.Normal)
        {
            SwitchToGiantMode();
        }

        if (PlayerInSight())
        {
            FlipTowardsPlayer(); // Flip bos ke arah pemain
            if (currentMode == BossMode.Normal)
            {
                HandleMovement(); // Handle movement towards waypoints
                if (cooldownTimer >= attackCooldown)
                {
                    cooldownTimer = 0;
                    anim.SetTrigger("rangedAttack");
                }
            }
            else
            {
                if (summonCooldownTimer >= summonCooldown)
                {
                    SummonEnemies();
                    summonCooldownTimer = 0;
                }
            }
        }

        if (isScaling)
        {
            transform.localScale += Vector3.one * scaleSpeed * Time.deltaTime;
            if (transform.localScale.x >= 2) 
            {
                isScaling = false;
                UpdateScale();
            }
        }
    }

    private void FlipTowardsPlayer()
    {
        if (player.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1); 
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void HandleMovement()
    {
        if (currentWaypointIndex < waypoints.Count)
        {
            Transform targetWaypoint = waypoints[currentWaypointIndex];
            float step = moveSpeed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, step);

            if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                currentWaypointIndex++;
            }
        }
        else
        {
            currentWaypointIndex = 0;
        }
    }


    private void RangedAttack()
    {
        cooldownTimer = 0;
        foreach (var firepoint in firepoints)
        {
            FireFromPoint(firepoint);
        }
    }

    private void FireFromPoint(Transform firepoint)
    {
        Vector3 directionToPlayer = (player.transform.position - firepoint.position).normalized;
        firepoint.right = directionToPlayer;

        int fireballIndex = FindFireball();
        if (fireballIndex >= 0)
        {
            fireballs[fireballIndex].transform.position = firepoint.position;
            fireballs[fireballIndex].transform.rotation = Quaternion.identity;
            fireballs[fireballIndex].GetComponent<EnemyProjectiles>().ActivateProjectile();
        }
    }

    private void SummonEnemies()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }


    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
    private void SwitchToGiantMode()
    {
        currentMode = BossMode.Giant;
        spriteRenderer.color = giantColor;
        UpdateScale();
    }

    private void UpdateScale()
    {
        if (currentMode == BossMode.Normal)
        {
            transform.localScale = new Vector3(1, 1, 1); 
        }
        else if (currentMode == BossMode.Giant)
        {
            transform.localScale = new Vector3(2, 2, 2); 
        }
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}