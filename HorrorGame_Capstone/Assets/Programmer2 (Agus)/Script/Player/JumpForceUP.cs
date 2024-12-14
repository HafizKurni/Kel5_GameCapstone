using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpForceUP : MonoBehaviour
{
    [Header("Player Reference")]
    [SerializeField] private CharMovements player;
    [SerializeField] public float jumpForceIncrease = 5f;
    [SerializeField] public float jumpForceDecrease = 2f;

    private EnemyHealth enemyHealth;

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Room") && player != null)
        {
            player.JumpForce -= jumpForceDecrease;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Room") && player != null)
        {
            player.JumpForce += jumpForceDecrease;
        }
    }

    public void AddJumpForceOnBossDeath()
    {
        if (enemyHealth != null && enemyHealth.IsDead)
        {
            player.JumpForce += jumpForceIncrease;
        }
    }
}