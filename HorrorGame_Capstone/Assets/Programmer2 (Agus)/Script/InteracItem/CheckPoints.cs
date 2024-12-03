using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CheckPoints : MonoBehaviour
{
    private static CheckPoints lastCheckpoint;
    PlayerHealth player;
    public Transform respawnPoint;

    public SpriteRenderer spriteRenderer;
    public Color activeColor = Color.yellow;
    public Color inactiveColor = Color.white;
    public float colorChangeSpeed = 1.0f;

    private Color targetColor;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        Deactivate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (lastCheckpoint != null && lastCheckpoint != this)
            {
                lastCheckpoint.Deactivate();
            }

            lastCheckpoint = this;
            player.UpdateCheckPoint(respawnPoint.position);
            Activate();
        }
    }

    private void Activate()
    {
        targetColor = activeColor;
    }

    private void Deactivate()
    {
        targetColor = inactiveColor;
    }

    private void Update()
    {
        // Mengubah warna sprite secara bertahap
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, targetColor, colorChangeSpeed * Time.deltaTime);
        }
    }
}