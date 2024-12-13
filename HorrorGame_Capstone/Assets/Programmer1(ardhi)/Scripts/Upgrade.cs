using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private HealthPotions healthPotions;

    // Slider dan Button untuk Weapon dan Ultimate
    [SerializeField] private Slider weaponDamageSlider;
    [SerializeField] private Slider ultimateCooldownSlider;

    [SerializeField] private Button weaponUpgradeButton;
    [SerializeField] private Button ultimateUpgradeButton;
    [SerializeField] private Button healthPotionUpgradeButton;

    // Horizontal Grid Layout untuk HealthPotion
    [SerializeField] private Transform indicatorPanel;
    [SerializeField] private Sprite activeDot;
    [SerializeField] private Sprite inactiveDot;

    // Weapon Properties
    [SerializeField] private float weaponDamageIncrement;
    [SerializeField] private float maxWeaponDamage;
    private int weaponLevel = 0;
    private int maxWeaponLevel = 10;

    // Ultimate Properties
    [SerializeField] private float ultimateCooldownDecrement;
    [SerializeField] private float minUltimateCooldown;
    private int ultimateLevel = 0;
    private int maxUltimateLevel = 5;

    // HealthPotion Properties
    [SerializeField] private float healthPotionIncrement;
    [SerializeField] private float maxHealthPotionValue;
    private int healthPotionLevel = 0;
    private int maxHealthPotionLevel = 5;

    private void Start()
    {
        UpdateUI();

        weaponUpgradeButton.onClick.AddListener(OnWeaponUpgradeButtonClicked);
        ultimateUpgradeButton.onClick.AddListener(OnUltimateUpgradeButtonClicked);
        healthPotionUpgradeButton.onClick.AddListener(OnHealthPotionUpgradeButtonClicked);
    }

    private void UpdateUI()
    {
        // Memperbarui slider untuk weapon
        weaponDamageSlider.value = weaponLevel;
        weaponDamageSlider.maxValue = maxWeaponLevel;

        // Memperbarui slider untuk ultimate
        ultimateCooldownSlider.value = ultimateLevel;
        ultimateCooldownSlider.maxValue = maxUltimateLevel;

        // Memperbarui indikator untuk health potion
        UpdateHealthIndicator(healthPotionLevel, maxHealthPotionLevel);

        // Menonaktifkan tombol jika level maksimum tercapai
        weaponUpgradeButton.interactable = weaponLevel < maxWeaponLevel;
        ultimateUpgradeButton.interactable = ultimateLevel < maxUltimateLevel;
        healthPotionUpgradeButton.interactable = healthPotionLevel < maxHealthPotionLevel;

        Debug.Log($"Weapon Level: {weaponLevel}, Damage: {playerAttack.damage}");
        Debug.Log($"Ultimate Level: {ultimateLevel}, Cooldown: {playerAttack.ultimateCD}");
        Debug.Log($"HealthPotion Level: {healthPotionLevel}, Heal Value: {healthPotions.healValue}");
    }

    public void UpdateHealthIndicator(int currentLevel, int maxLevel)
    {
        for (int i = 0; i < maxLevel; i++)
        {
            Transform dot = indicatorPanel.GetChild(i);
            dot.GetComponent<Image>().sprite = i < currentLevel ? activeDot : inactiveDot;
        }
    }

    public void UpgradeWeapon()
    {
        if (weaponLevel < maxWeaponLevel && playerAttack.damage < maxWeaponDamage)
        {
            weaponLevel++;
            playerAttack.damage += weaponDamageIncrement;
            playerAttack.damage = Mathf.Clamp(playerAttack.damage, 0, maxWeaponDamage);
            UpdateUI();
        }
    }

    public void UpgradeUltimate()
    {
        if (ultimateLevel < maxUltimateLevel && playerAttack.ultimateCD > minUltimateCooldown)
        {
            ultimateLevel++;
            playerAttack.ultimateCD -= ultimateCooldownDecrement;
            playerAttack.ultimateCD = Mathf.Clamp(playerAttack.ultimateCD, minUltimateCooldown, Mathf.Infinity);
            UpdateUI();
        }
    }

    public void UpgradeHealthPotion()
    {
        if (healthPotionLevel < maxHealthPotionLevel && healthPotions.healValue < maxHealthPotionValue)
        {
            healthPotionLevel++;
            healthPotions.healValue += healthPotionIncrement;
            healthPotions.healValue = Mathf.Clamp(healthPotions.healValue, 0, maxHealthPotionValue);
            UpdateUI();
        }
    }

    private void OnWeaponUpgradeButtonClicked()
    {
        UpgradeWeapon();
    }

    private void OnUltimateUpgradeButtonClicked()
    {
        UpgradeUltimate();
    }

    private void OnHealthPotionUpgradeButtonClicked()
    {
        UpgradeHealthPotion();
    }
}
