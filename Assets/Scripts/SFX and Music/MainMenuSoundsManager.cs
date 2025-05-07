using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSoundsManager : MonoBehaviour
{
    public AudioClip buttonClickSound, nextPrevShipButtonSound, selectShipButtonSound;
    AudioSource audioSource;
    public void PlayButtonClickSound()
    {
        if (PlayerPrefs.GetInt("Sound") == 0) return;

        audioSource.Stop();
        audioSource.clip = buttonClickSound;
        audioSource.Play();
    }

    public void PlayNextPrevShipButtonSound()
    {
        if (PlayerPrefs.GetInt("Sound") == 0) return;

        audioSource.Stop();
        audioSource.clip = nextPrevShipButtonSound;
        audioSource.Play();
    }

    public void PlaySelectShipButtonSound()
    {
        if (PlayerPrefs.GetInt("Sound") == 0) return;

        audioSource.Stop();
        audioSource.clip = selectShipButtonSound;
        audioSource.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
