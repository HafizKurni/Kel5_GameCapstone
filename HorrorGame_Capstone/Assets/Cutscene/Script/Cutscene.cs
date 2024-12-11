using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cutscene : MonoBehaviour
{
    [Header("Components")]
    public Image photoDisplay;         // Gambar utama untuk foto
    public Image subtitleDisplay;      // Gambar untuk subtitle
    public Sprite[] photoSprites;      // Array untuk foto
    public Sprite[] subtitleSprites;   // Array untuk subtitle

    [Header("Settings")]
    public float fadeTime = 2f;         // Durasi animasi fade
    public float photoDisplayTime = 5f; // Waktu menampilkan foto
    public float subtitleDelay = 1f;    // Waktu jeda sebelum subtitle muncul
    public float subtitleDisplayTime = 3f; // Waktu menampilkan subtitle

    private int currentIndex = 0;       // Indeks elemen yang sedang ditampilkan
    private bool showingSubtitle = false; // Status apakah subtitle sedang ditampilkan
    private float timer;                // Timer untuk transisi otomatis

    void Start()
    {
        timer = photoDisplayTime;
        ShowPhoto(); // Mulai dengan menampilkan foto pertama
    }

    void Update()
    {
        // Skip slideshow dengan tombol ESC (kembali ke menu utama)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Main Menu");
            return;
        }

        // Mulai gameplay dengan tombol RETURN
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("Gameplay2");
            return;
        }

        // Timer untuk transisi otomatis
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            if (!showingSubtitle)
            {
                ShowSubtitle(); // Tampilkan subtitle setelah foto selesai
            }
            else
            {
                NextSlide(); // Pindah ke slide berikutnya setelah subtitle selesai
            }
        }

        // Kontrol manual untuk next slide
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextSlide();
        }

        // Kontrol manual untuk previous slide
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousSlide();
        }
    }

    void ShowPhoto()
    {
        // Menampilkan foto
        showingSubtitle = false;
        photoDisplay.canvasRenderer.SetAlpha(0.0f);
        subtitleDisplay.canvasRenderer.SetAlpha(0.0f);

        photoDisplay.sprite = photoSprites[currentIndex];
        subtitleDisplay.sprite = null; // Kosongkan subtitle saat foto ditampilkan

        photoDisplay.CrossFadeAlpha(1, fadeTime, false);
        timer = photoDisplayTime + subtitleDelay; // Waktu total sebelum subtitle muncul
    }

    void ShowSubtitle()
    {
        // Menampilkan subtitle di atas foto
        showingSubtitle = true;
        subtitleDisplay.canvasRenderer.SetAlpha(0.0f);

        subtitleDisplay.sprite = subtitleSprites[currentIndex]; // Menampilkan subtitle sesuai indeks
        subtitleDisplay.CrossFadeAlpha(1, fadeTime, false);

        timer = subtitleDisplayTime; // Reset timer untuk subtitle
    }

    void NextSlide()
    {
        if (!showingSubtitle)
        {
            // Jika saat ini menampilkan foto, langsung tampilkan subtitle
            ShowSubtitle();
        }
        else
        {
            // Jika saat ini menampilkan subtitle, pindah ke foto berikutnya
            currentIndex++;
            if (currentIndex >= photoSprites.Length)
            {
                currentIndex = 0; // Ulang dari awal jika sudah mencapai akhir
            }

            ShowPhoto();
        }
    }

    void PreviousSlide()
    {
        if (showingSubtitle)
        {
            // Jika saat ini menampilkan subtitle, kembali ke foto
            ShowPhoto();
        }
        else
        {
            // Jika saat ini menampilkan foto, pindah ke subtitle foto sebelumnya
            currentIndex--;
            if (currentIndex < 0)
            {
                currentIndex = photoSprites.Length - 1; // Kembali ke foto terakhir jika sudah di slide pertama
            }

            ShowSubtitle();
        }
    }
}