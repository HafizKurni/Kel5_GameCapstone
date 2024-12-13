using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float startingHealth = 100f;
    [SerializeField] private float maxHealth;
    [SerializeField] private float deathAnimationDuration = 1f;
    [SerializeField] private Image healthBar;
    [SerializeField] private float invincibilityDuration = 2f;
    private Animator anim;
    private CharMovements charMovements;
    private SpriteRenderer spriteRenderer;
    public float currentHealth { get; private set; }
    public bool IsDead { get; private set; }
    Vector2 checkPointPos;
    private bool isInvincible;

    void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        charMovements = GetComponent<CharMovements>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        IsDead = false;
        isInvincible = false;
        UpdateHealthSlider();
    }
    private void Start()
    {
        checkPointPos = transform.position;
    }

    public void TakeDamage(float damage)
    {
        if (IsDead || isInvincible) return;

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
        UpdateHealthSlider();

        if (currentHealth > 0)
        {
            anim.SetTrigger("Hurt");
        }
        else
        {
            IsDead = true;
            StartCoroutine(HandleDeath());
        }
    }

    public void AddHealth(float value)
    {
        if (IsDead) return;
        currentHealth = Mathf.Clamp(currentHealth + value, 0, startingHealth);
        UpdateHealthSlider();
    }
    public void IncreaseHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
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
        DisablePlayerMovement();
        StartCoroutine(Respawn());
    }

    private void DisablePlayerMovement()
    {
        if (charMovements != null)
        {
            charMovements.enabled = false;
        }
    }

    private void EnablePlayerMovementt()
    {
        if (charMovements != null)
        {
            charMovements.enabled = true;
        }
    }

    public void UpdateCheckPoint(Vector2 pos)
    {
        checkPointPos = pos;
    }
    private IEnumerator Respawn()
    {
        IsDead = false;
        transform.position = checkPointPos;
        currentHealth = startingHealth;
        EnablePlayerMovementt();
        UpdateHealthSlider();

        isInvincible = true;
        StartCoroutine(InvincibilityFlash());
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
        SetAlpha(1f);
    }

    private IEnumerator InvincibilityFlash()
    {
        float flashDuration = 0.1f;
        float elapsed = 0f;

        while (elapsed < invincibilityDuration)
        {
            SetAlpha(0f);
            yield return new WaitForSeconds(flashDuration);
            SetAlpha(1f);
            yield return new WaitForSeconds(flashDuration);
            elapsed += flashDuration * 2;
        }
    }

    private void SetAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

}