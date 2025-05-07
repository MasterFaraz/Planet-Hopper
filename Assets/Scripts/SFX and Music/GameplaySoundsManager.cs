using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySoundsManager : MonoBehaviour
{
    public AudioClip buttonClickSound, playerShipTapSound, shipEnterOrbitSound,
        shipEnterOrbitPerfectSound, gameOverSound, powerUpSound, shipDriftingAwaySound;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayPowerUpSound()
    {
        if (PlayerPrefs.GetInt("Sound") == 0) return;

        audioSource.Stop();
        audioSource.clip = powerUpSound;
        audioSource.Play();
    }

    public void PlayButtonClickSound()
    {
        if (PlayerPrefs.GetInt("Sound") == 0) return;

        audioSource.Stop();
        audioSource.clip = buttonClickSound;
        audioSource.Play();
    }

    public void PlayerShipTapSound() {

        if (PlayerPrefs.GetInt("Sound") == 0) return;

        audioSource.Stop();
        audioSource.clip = playerShipTapSound;
        audioSource.Play();
    }

    public void PlayerEnterOrbitSound()
    {

        if (PlayerPrefs.GetInt("Sound") == 0) return;

        audioSource.Stop();
        audioSource.clip = shipEnterOrbitSound;
        audioSource.Play();
    }

    public void PlayerEnterOrbitPerfectSound()
    {

        if (PlayerPrefs.GetInt("Sound") == 0) return;

        audioSource.Stop();
        audioSource.clip= shipEnterOrbitPerfectSound;
        audioSource.Play();
    }

    public void PlayerOverSound() {


        if (PlayerPrefs.GetInt("Sound") == 0) return;

        audioSource.Stop();
        audioSource.clip = gameOverSound;
        audioSource.Play();
    
    }

    bool volumeSetZero = false;
    float currentVol;

    public void PlayShipDriftingAwaySound()
    {

        if (PlayerPrefs.GetInt("Sound") == 0) return;

        if(MusicPlayer.Instance != null)
        {
            //StopAllCoroutines();
            currentVol = MusicPlayer.Instance.GetComponent<AudioSource>().volume;
            MusicPlayer.Instance.GetComponent<AudioSource>().Pause();
            //MusicPlayer.Instance.ChangeVolumeIntensity(0);
            volumeSetZero = true;
        }

        audioSource.Stop();
        audioSource.clip = shipDriftingAwaySound;
        audioSource.Play();

    }


    private void Update()
    {
        if (volumeSetZero)
        {
            if (!audioSource.isPlaying && audioSource.time > 0)
            {
                volumeSetZero = false;
                MusicPlayer.Instance.GetComponent<AudioSource>().Play();
            }
        }
    }




}
