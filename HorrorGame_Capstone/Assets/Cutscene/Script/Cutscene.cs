using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cutscene : MonoBehaviour
{
    public Image image1;
    public Sprite[] SpriteArray; 
    private int currentImage = 0;
    public float fadeTime = 2f;
    public bool fadefinished = false;

    private float deltaTime = 0.0f;
    public float timer1 = 5.0f;
    private float timer1Remaining = 5.0f;
    public bool timer1IsRunning = true;

    // Start is called before the first frame update
    void Start()
    {
        image1.canvasRenderer.SetAlpha(0.0f);
        image1.sprite = SpriteArray[currentImage];
        image1.CrossFadeAlpha(1, fadeTime, false);

        timer1IsRunning = false;
        timer1Remaining = timer1; // timer1 harus lebih besar dari fadeTime
    }

    // Update is called once per frame
    void Update()
    {
        // Handle input untuk kembali ke "menu level"
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Main Menu");
            return;
        }

        // Handle input untuk pindah ke "gameplay2"
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("Gameplay2");
            return;
        }

        // Timer logic
        if (timer1IsRunning)
        {
            if (timer1Remaining > 0)
            {
                timer1Remaining -= Time.deltaTime;
                image1.CrossFadeAlpha(1, fadeTime, false);
            }
            else
            {
                Debug.Log("Timer1 has finished!");
                timer1Remaining = timer1;
                fadefinished = true;
                timer1IsRunning = false;
                image1.CrossFadeAlpha(0, fadeTime, false);
            }
        }

        // Handle navigasi sprite menggunakan tombol
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("Next image.");
            currentImage++;
            if (currentImage >= SpriteArray.Length)
                currentImage = 0;
            Fade();
        }

        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("Previous image.");
            currentImage--;
            if (currentImage < 0)
                currentImage = 0;
            Fade();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            timer1IsRunning = !timer1IsRunning;
        }
    }

    void Fade()
    {
        image1.canvasRenderer.SetAlpha(0.0f);
        image1.sprite = SpriteArray[currentImage];
        timer1Remaining = timer1;
        timer1IsRunning = true;
    }
}
