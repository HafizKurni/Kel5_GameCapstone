using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class HealingCamp : MonoBehaviour
{
    public GameObject interactPrompt;
    public GameObject Player;
    public float healthValue = 2f;
    public float interactionRange = 2f;
    public float InvincibleTime = 2f;

    private PlayerInput playerInput;
    private PlayerHealth playerHealth;
    private Rigidbody2D playerRigidbody;
    private float originalGravityScale;
    private CharMovements charMovements;

    private void Start()
    {
        charMovements = Player.GetComponent<CharMovements>();
        playerHealth = Player.GetComponent<PlayerHealth>();
        playerRigidbody = Player.GetComponent<Rigidbody2D>();
        originalGravityScale = playerRigidbody.gravityScale;
        interactPrompt.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactPrompt.SetActive(false);
        }
    }

    private void Interact()
    {
        StartCoroutine(InCamp());
    }

    private IEnumerator InCamp()
    {
        // Menonaktifkan komponen SpriteRenderer dan CapsuleCollider2D
        SpriteRenderer spriteRenderer = Player.GetComponent<SpriteRenderer>();
        CapsuleCollider2D capsuleCollider = Player.GetComponent<CapsuleCollider2D>();

        if (spriteRenderer != null) spriteRenderer.enabled = false;
        if (capsuleCollider != null) capsuleCollider.enabled = false;

        if (charMovements != null)
        {
            charMovements.enabled = false;
        }

        // Menonaktifkan gravitas
        if (playerRigidbody != null)
        {
            playerRigidbody.gravityScale = 0;
        }

        float elapsedTime = 0f;
        while (elapsedTime < InvincibleTime)
        {
            if (playerHealth != null)
            {
                playerHealth.IncreaseHealth(healthValue * Time.deltaTime); // Tambahkan kesehatan sedikit demi sedikit
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Mengaktifkan kembali komponen setelah waktu habis
        if (spriteRenderer != null) spriteRenderer.enabled = true;
        if (capsuleCollider != null) capsuleCollider.enabled = true;

        // Mengembalikan gravitas
        if (playerRigidbody != null)
        {
            playerRigidbody.gravityScale = originalGravityScale;
        }

        if (charMovements != null)
        {
            charMovements.enabled = true;

        }
    }
}