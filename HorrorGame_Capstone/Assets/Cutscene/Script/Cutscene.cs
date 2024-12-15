using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cutscene : MonoBehaviour
{
    [Header("Components")]
    public Image photoDisplay;
    public Image subtitleDisplay;
    public Sprite[] photoSprites;
    public Sprite[] subtitleSprites;

    [Header("Settings")]
    public float fadeTime = 2f;
    public float photoDisplayTime = 5f;
    public float subtitleDelay = 1f;
    public float subtitleDisplayTime = 3f;

    private int currentIndex = 0;
    private bool showingSubtitle = false;
    private float timer;

    void Start()
    {
        timer = photoDisplayTime;
        ShowPhoto();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Main Menu");
            return;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("Gameplay2");
            return;
        }

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            if (!showingSubtitle)
            {
                ShowSubtitle();
            }
            else
            {
                NextSlide();
            }
        }
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextSlide();
        }

        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousSlide();
        }
    }

    void ShowPhoto()
    {
        showingSubtitle = false;
        photoDisplay.canvasRenderer.SetAlpha(0.0f);
        subtitleDisplay.canvasRenderer.SetAlpha(0.0f);

        photoDisplay.sprite = photoSprites[currentIndex];
        subtitleDisplay.sprite = null;

        photoDisplay.CrossFadeAlpha(1, fadeTime, false);
        timer = photoDisplayTime + subtitleDelay;
    }

    void ShowSubtitle()
    {
        showingSubtitle = true;
        subtitleDisplay.canvasRenderer.SetAlpha(0.0f);

        subtitleDisplay.sprite = subtitleSprites[currentIndex];
        subtitleDisplay.CrossFadeAlpha(1, fadeTime, false);

        timer = subtitleDisplayTime;
    }

    void NextSlide()
    {
        if (!showingSubtitle)
        {
            ShowSubtitle();
        }
        else
        {
            currentIndex++;
            if (currentIndex >= photoSprites.Length)
            {
                currentIndex = 0;
            }

            ShowPhoto();
        }
    }

    void PreviousSlide()
    {
        if (showingSubtitle)
        {
            ShowPhoto();
        }
        else
        {
            currentIndex--;
            if (currentIndex < 0)
            {
                currentIndex = photoSprites.Length - 1;
            }

            ShowSubtitle();
        }
    }
}