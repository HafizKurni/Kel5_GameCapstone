using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.VirtualTexturing;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] musicSounds, SFXSounds;
    public AudioSource musicSource, sfxSource;

    private const string musicVolumeKey = "MusicVolume";
    private const string sfxVolumeKey = "SFXVolume";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        // Memutar musik sesuai dengan scene saat ini
        PlayMusicByScene(SceneManager.GetActiveScene().name);

        musicSource.volume = PlayerPrefs.GetFloat(musicVolumeKey, 1f);
        sfxSource.volume = PlayerPrefs.GetFloat(sfxVolumeKey, 1f);

        // Menambahkan listener untuk perubahan scene
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Menghapus listener saat AudioManager dihancurkan
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicByScene(scene.name);
    }

    private void PlayMusicByScene(string sceneName)
    {
        switch (sceneName)
        {
            case "MainMenu":
                PlayMusic("MainMenu"); // Ganti dengan nama musik tema menu Anda
                break;
            case "Ganeplay2":
                PlayMusic("GamePlay"); // Ganti dengan nama musik permainan Anda
                break;
            default:
                break;
        }
    }
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaysSFX(string name)
    {
        Sound s = Array.Find(SFXSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void ToggleMusic()
    {
        musicSource.mute= !musicSource.mute;
    }
    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void MusicVolume(float Volume)
    {
        musicSource.volume = Volume;
        PlayerPrefs.SetFloat(musicVolumeKey, Volume);
        PlayerPrefs.Save();
    }
    public void SFXVolume(float Volume)
    {
        sfxSource.volume = Volume;
        PlayerPrefs.SetFloat(sfxVolumeKey, Volume);
        PlayerPrefs.Save();
    }
}
