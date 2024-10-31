using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    [SerializeField] private float deathAnimationDuration = 1f;
    [SerializeField] private GameObject lootPrefab;
    [SerializeField] private float lootDelay = 0.5f;
    [SerializeField] private Vector3 lootOffset = new Vector3(0, 0.5f, 0);
    private Animator anim;
    public float currentHealth { get; private set; }
    public bool IsDead { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        IsDead = false;
    }

    public void TakeDamage(float _damage)
    {
        if (IsDead) return; // Cek apakah sudah mati

        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("Hurt");
        }
        else
        {
            IsDead = true;
            anim.SetTrigger("Die");
            if (GetComponent<PlayerMovement>() != null)
            {
                GetComponent<PlayerMovement>().enabled = false; // Hanya jika ada komponen PlayerMovement
            }
            StartCoroutine(HandleDeath());
        }
    }

    public void HealAmount(float _value)
    {
        if (IsDead) return;
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    private IEnumerator HandleDeath()
    {
        // Tunggu selama durasi animasi kematian
        yield return new WaitForSeconds(deathAnimationDuration);
        yield return new WaitForSeconds(lootDelay); // Delay sebelum loot dikeluarkan
        DropLoot();
        gameObject.SetActive(false);
    }

    private void DropLoot()
    {
        if (lootPrefab != null)
        {
            // Menghitung posisi titik spawn loot, menggunakan posisi tengah enemy
            Vector3 lootPosition = transform.position + lootOffset;

            // Instantiate lootPrefab pada posisi yang sudah dihitung
            Instantiate(lootPrefab, lootPosition, Quaternion.identity);
        }
    }
}