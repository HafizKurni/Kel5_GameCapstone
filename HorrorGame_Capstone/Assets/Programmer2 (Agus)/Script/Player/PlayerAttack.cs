using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Basic Attack")]
    [SerializeField] private float damage;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private float comboAdvanceSpeed = 0.1f;

    [Header("Skill Attack")]
    [SerializeField] private float skillCD;
    private float skillCooldownTimer = 0f;

    [Header("Ultimate Attack")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] slash;
    [SerializeField] private float ultimateAnimationDuration = 1f;
    [SerializeField] private float ultimateCD;
    [SerializeField] private float advanceInterval = 0.2f;
    private float ultimateCooldownTimer = 0f;

    private Animator anim;
    private PlayerMovement playerMovement;
    private bool IsAttacking;
    private float coldownTimer = Mathf.Infinity;
    private PlayerInput playerInput;
    public int NoOfClick = 0;
    float lastClikTime = 0;
    public float maxComboDelay = 0.9f;

    void Start()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerInput = new PlayerInput(); // Inisialisasi PlayerInput
        playerInput.Player.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        skillCooldownTimer -= Time.deltaTime;
        ultimateCooldownTimer -= Time.deltaTime;

        if (Time.time - lastClikTime > maxComboDelay)
        {
            NoOfClick = 0;
        }

        if (playerInput.Player.CastUltimate.triggered && ultimateCooldownTimer <= 0)
        {
            StartCoroutine(CastUltimate());
        }

        if (playerInput.Player.CastSkill.triggered && skillCooldownTimer <= 0)
        {
            CastSkill();
        }
        if (playerInput.Player.Attack.triggered)
        {
            lastClikTime = Time.time;
            NoOfClick++;
            if (NoOfClick == 1)
            {
                anim.SetBool("Attack 1", true);
                AudioManager.instance.PlaysSFX("MC_Sword");
                AdvanceCombo();
            }
            NoOfClick = Mathf.Clamp(NoOfClick, 0, 3);
        }
    }

    private void AdvanceCombo()
    {
        // Maju perlahan ke depan
        Vector2 moveDirection = new Vector2(Mathf.Sign(transform.localScale.x), 0);
        transform.Translate(moveDirection * comboAdvanceSpeed);
    }

    public void Return1()
    {
        if (NoOfClick >= 2)
        {
            anim.SetBool("Attack 2", true);
            AudioManager.instance.PlaysSFX("MC_Sword");
            AdvanceCombo();
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
            AudioManager.instance.PlaysSFX("MC_Sword");
            AdvanceCombo();
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

    private IEnumerator CastUltimate()
    {
        anim.SetTrigger("CastUlti");
        AudioManager.instance.PlaysSFX("MC_Ulti");

        ultimateCooldownTimer = ultimateCD;

        // Maju setiap beberapa detik selama animasi ultimate
        float elapsedTime = 0f;
        while (elapsedTime < ultimateAnimationDuration)
        {
            AdvanceCombo();
            elapsedTime += advanceInterval;
            yield return new WaitForSeconds(advanceInterval);
        }

        // Summon fireball setelah animasi
        int fireballIndex = CheckFireball();
        if (fireballIndex >= 0)
        {
            slash[fireballIndex].transform.position = firePoint.position;
            slash[fireballIndex].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
        }
    }

    public void Attack()
    {
        IsAttacking = true;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(firePoint.position, attackRange, enemyLayerMask);
        foreach (Collider2D enemy in hitEnemies)
        {
            DealDamage(enemy);
        }
    }
    public void CastSkill()
    {
        anim.SetTrigger("CastSkill");
        AudioManager.instance.PlaysSFX("MC_Skill");

        skillCooldownTimer = skillCD;
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
        for (int i = 0; i < slash.Length; i++)
        {
            if (!slash[i].activeInHierarchy)
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
