using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    private static CheckPoints lastCheckpoint;
    PlayerHealth player;
    public Transform respawnPoint;

    public GameObject activeObject;
    public GameObject passiveObject;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
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
            if (activeObject != null)
            {
                activeObject.SetActive(true);
            }
            if (passiveObject != null)
            {
                passiveObject.SetActive(false);
            }
        }
    }
    private void Deactivate()
    {
        if (passiveObject != null)
        {
            passiveObject.SetActive(true);
        }
        if (activeObject != null)
        {
            activeObject.SetActive(false);
        }
    }
}
