using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float startingHealth = 100f;
    [SerializeField] private float deathAnimationDuration = 1f;
    [SerializeField] private GameObject lootPrefab;
    [SerializeField] private float lootDelay = 0.5f;
    [SerializeField] private Vector3 lootOffset = new Vector3(0, 0.5f, 0);
    private Animator anim;
    public float currentHealth { get; private set; }
    public bool IsDead { get; private set; }

    [Header("UI Elements")]
    [SerializeField] private Image healthBar;
    void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        IsDead = false;
        UpdateHealthSlider();
    }

    public void TakeDamage(float damage)
    {
        if (IsDead) return;

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
        UpdateHealthSlider();

        if (currentHealth > 0)
        {
            anim.SetTrigger("Hurt");
        }
        else
        {
            IsDead = true;
            anim.SetTrigger("Die");
            StartCoroutine(HandleDeath());
        }
    }

    private void UpdateHealthSlider()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / startingHealth;
        }
    }

    private IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(deathAnimationDuration);
        yield return new WaitForSeconds(lootDelay);
        DropLoot();
        gameObject.SetActive(false);
    }

    private void DropLoot()
    {
        if (lootPrefab != null)
        {
            Vector3 lootPosition = transform.position + lootOffset;
            Instantiate(lootPrefab, lootPosition, Quaternion.identity);
        }
    }
}