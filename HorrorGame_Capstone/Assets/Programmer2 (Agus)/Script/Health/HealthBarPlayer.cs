using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBarPlayer : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image totalHealthBar;
    [SerializeField] private Image currentHealthBar;

    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private CanvasGroup pauseCanvasGroup;

    [Header("UI Buttons")]
    [SerializeField] private Button lanjutButton; // Referensi ke tombol "Lanjut btn"

    void Start()
    {
        totalHealthBar.fillAmount = playerHealth.currentHealth / 10;
        PauseGame(false);

        // Assign fungsi resume ke tombol "Lanjut btn"
        if (lanjutButton != null)
        {
            lanjutButton.onClick.AddListener(() => PauseGame(false));
        }
    }

    void Update()
    {
        currentHealthBar.fillAmount = playerHealth.currentHealth / 10;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle pause status
            bool isPaused = pauseScreen.activeInHierarchy;
            PauseGame(!isPaused);
        }
    }

    public void PauseGame(bool status)
    {
        pauseScreen.SetActive(status);

        if (pauseCanvasGroup != null)
        {
            pauseCanvasGroup.alpha = status ? 1 : 0;
            pauseCanvasGroup.interactable = status;
            pauseCanvasGroup.blocksRaycasts = status;
        }

        Time.timeScale = status ? 0 : 1;
    }

    // Fungsi untuk memuat scene tertentu
    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }
}