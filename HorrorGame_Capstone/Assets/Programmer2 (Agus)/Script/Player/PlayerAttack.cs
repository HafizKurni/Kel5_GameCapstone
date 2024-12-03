using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float castCooldown;
    [SerializeField] private float attackColdown;
    [SerializeField] private float damage;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask enemyLayerMask;
    private Animator anim;
    private PlayerMovement playerMovement;
    private float coldownTimer = Mathf.Infinity;

    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireBalls;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.S) && coldownTimer > castCooldown)
        {
            CastAttack();
        }

        if (Input.GetKey(KeyCode.X) && coldownTimer > attackColdown)
        {
            Attack();
        }
        coldownTimer += Time.deltaTime;
    }

    private void CastAttack()
    {
        anim.SetTrigger("CastFire");
        coldownTimer = 0;

        fireBalls[CheckFireball()].transform.position = firePoint.position;
        fireBalls[CheckFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private void Attack()
    {
        anim.SetTrigger("Attack");
        coldownTimer = 0;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(firePoint.position, attackRange, enemyLayerMask);
        foreach (Collider2D enemy in hitEnemies)
        {
            DealDamage(enemy);
        }
    }
    private void DealDamage(Collider2D enemyCollider)
    {
        EnemyHealth enemyHealth = enemyCollider.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }
    }

    private int CheckFireball()
    {
        for (int i = 0; i < fireBalls.Length; i++)
        {
            if (!fireBalls[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(firePoint.position, attackRange);
    }
}
