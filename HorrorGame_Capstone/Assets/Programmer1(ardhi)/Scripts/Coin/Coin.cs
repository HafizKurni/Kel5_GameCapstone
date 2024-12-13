using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int coinValue = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CoinManager coinManager = other.GetComponent<CoinManager>();

            if (coinManager != null)
            {
                coinManager.AddCoins(coinValue);

                Destroy(gameObject);
            }
        }
    }
}
