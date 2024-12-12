using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BosAttack : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private EnemyHealth enemyHealth; // Referensi ke EnemyHealth
    [SerializeField] private float damageMultiplier = 2f;
    [SerializeField] private int minionCount = 5;
    [SerializeField] private float summonCooldown = 10f;
    [SerializeField] private GameObject minionPrefab;

    private bool isFlying = false;
    private bool minionSummoned = false;

    void Start()
    {
        // Pastikan enemyHealth diatur melalui inspector atau dalam metode lain
    }

    void Update()
    {
        if (enemyHealth.currentHealth <= 20 * 0.5f && !minionSummoned)
        {
            StartCoroutine(SummonMinions());
        }

        if (enemyHealth.currentHealth <= 10 * 0.5f)
        {
            IncreaseBossDamage();
        }

        if (isFlying)
        {
            Fly();
        }
    }

    // Mengambil damage dari skrip EnemyHealth
    public void TakeDamage(float damage)
    {
        enemyHealth.TakeDamage(damage);
    }

    private IEnumerator SummonMinions()
    {
        minionSummoned = true;

        // Hapus semua minion yang ada (jika diperlukan)
        foreach (GameObject minion in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(minion);
        }

        // Summon minion
        for (int i = 0; i < minionCount; i++)
        {
            Instantiate(minionPrefab, transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(summonCooldown);

        // Mengizinkan summon lagi jika bos masih hidup
        if (enemyHealth.currentHealth > 0)
        {
            minionSummoned = false;
        }
    }

    private void IncreaseBossDamage()
    {
        
    }

    private void Fly()
    {
        transform.position += new Vector3(0, 1 * Time.deltaTime, 0);
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}