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
    public Animator anim;
    private PlayerMovement playerMovement;
    private bool IsAttacking;
    private float coldownTimer = Mathf.Infinity;

    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireBalls;

    public int NoOfClick = 0;
    float lastClikTime = 0;
    public float maxComboDelay = 0.9f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.deltaTime - lastClikTime > maxComboDelay)
        {
            NoOfClick = 0;
        }

        if (Input.GetKey(KeyCode.S) && coldownTimer > castCooldown)
        {
            CastAttack();
        }

        if (Input.GetKey(KeyCode.X) && coldownTimer > attackColdown)
        {
            lastClikTime = Time.deltaTime;
            NoOfClick++;
            if (NoOfClick == 1)
            {
                anim.SetBool("Attack 1", true);
            }
            NoOfClick = Mathf.Clamp(NoOfClick, 0, 3);
        }
        coldownTimer += Time.deltaTime;
    }
    public void Return1()
    {
        if (NoOfClick >= 2)
        {
            anim.SetBool("Attack 2", true);
        }
        else
        {
            anim.SetBool("Attack 1", false);
            NoOfClick = 0;
        }
    }
    public void Return2()
    {
        if (NoOfClick >= 3)
        {
            anim.SetBool("Attack 3", true);
        }
        else
        {
            anim.SetBool("Attack 2", false);
            NoOfClick = 0;
        }
    }
    public void Return3()
    {
         anim.SetBool("Attack 1", false);
         anim.SetBool("Attack 2", false);
         anim.SetBool("Attack 3", false);
         NoOfClick = 0;
    }
    public void CastAttack()
    {
        anim.SetTrigger("CastFire");
        coldownTimer = 0;

        fireBalls[CheckFireball()].transform.position = firePoint.position;
        fireBalls[CheckFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    public void Attack()
    {
        IsAttacking = true;
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
