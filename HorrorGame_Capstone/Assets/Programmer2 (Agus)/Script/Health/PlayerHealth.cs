using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float startingHealth = 100f;
    [SerializeField] private float deathAnimationDuration = 1f;
    [SerializeField] private Image healthBar;
    private Animator anim;
    public float currentHealth { get; private set; }
    public bool IsDead { get; private set; }
    Vector2 checkPointPos;

    void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        IsDead = false;
        UpdateHealthSlider();
    }
    private void Start()
    {
        checkPointPos = transform.position;
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
            DisablePlayerMovement();
            StartCoroutine(HandleDeath());
        }
    }

    public void AddHealth(float value)
    {
        if (IsDead) return;
        currentHealth = Mathf.Clamp(currentHealth + value, 0, startingHealth);
        UpdateHealthSlider();
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

        Respawn();
    }

    private void DisablePlayerMovement()
    {
        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.enabled = false;
        }
    }

    public void UpdateCheckPoint(Vector2 pos)
    {
        checkPointPos = pos;
    }
    void Respawn()
    {
        IsDead = false;
        transform.position = checkPointPos;
        anim.SetTrigger("Respawn");
        currentHealth = startingHealth;

        UpdateHealthSlider();
    }
}