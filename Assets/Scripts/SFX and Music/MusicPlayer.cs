using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance;

    public AudioClip mainMenuMusic, gameplayMusic;

    public void PlayMainMenuMusic()
    {
        if (PlayerPrefs.GetInt("Music") == 0) return;

        musicSource.Stop();
        musicSource.clip = mainMenuMusic;
        musicSource.Play();
    }

    public void PlayGameplayMusic()
    {
        if (PlayerPrefs.GetInt("Music") == 0) return;

        musicSource.Stop();
        musicSource.clip = gameplayMusic;
        musicSource.Play();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    AudioSource musicSource;

    // Start is called before the first frame update
    void Start()
    {
        musicSource = GetComponent<AudioSource>();

        PlayMusic();
    }

    public void PlayMusic()
    {
        if(PlayerPrefs.GetInt("Music") == 1)
            musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void ChangeVolumeIntensity(float intensity)
    {
        if (PlayerPrefs.GetInt("Music") == 0) return;

        musicSource.volume = intensity;
    }

    public void ChangePitch(float pitch)
    {
        if (PlayerPrefs.GetInt("Music") == 0) return;

        musicSource.pitch = pitch;
    }
}
