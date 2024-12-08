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
    [SerializeField] private GameObject pauseScreen; // GameObject PausePanel
    [SerializeField] private CanvasGroup pauseCanvasGroup; // CanvasGroup PausePanel

    void Start()
    {
        totalHealthBar.fillAmount = playerHealth.currentHealth / 10;
        // Pastikan PausePanel dalam kondisi nonaktif di awal
        PauseGame(false);
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
        Time.timeScale = 1; // Pastikan waktu berjalan normal sebelum memuat scene baru
        SceneManager.LoadScene(sceneName);
    }
}