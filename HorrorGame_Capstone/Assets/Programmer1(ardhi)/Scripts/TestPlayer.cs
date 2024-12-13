using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CoinManager coinManager = GetComponent<CoinManager>();
            Debug.Log("Koin saat ini: " + coinManager.Coins);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CoinManager coinManager = GetComponent<CoinManager>();
            coinManager.AddCoins(50);
            Debug.Log("Koin saat ini: " + coinManager.Coins);
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            CoinManager coinManager = GetComponent<CoinManager>();
            coinManager.ResetCoins();
            Debug.Log("Koin saat ini: " + coinManager.Coins);
        }
    }
}
