using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    private const string COINS_KEY = "PlayerCoins";

    public int Coins { get; private set; }

    [SerializeField] private TextMeshProUGUI coinText;

    private void Start()
    {
        Coins = PlayerPrefs.GetInt(COINS_KEY, 0);
        UpdateCoinUI();
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        SaveCoins();
        UpdateCoinUI();
    }

    public bool SpendCoins(int amount)
    {
        if (Coins >= amount)
        {
            Coins -= amount;
            SaveCoins();
            UpdateCoinUI();
            return true;
        }
        return false;
    }

    private void SaveCoins()
    {
        PlayerPrefs.SetInt(COINS_KEY, Coins);
        PlayerPrefs.Save();
    }

    public void ResetCoins()
    {
        Coins = 0;
        SaveCoins();
        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = $"{Coins}";
        }
    }
}
