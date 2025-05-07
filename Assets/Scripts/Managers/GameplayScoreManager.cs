using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayScoreManager : MonoBehaviour
{
    public float camShakeIntensity = 1f, camShakeDuration = 0.25f, perfectTextBounceHeight = 50f, 
        perfectTextBounceDuration = 0.5f;

    private Vector3 originalPosition;
    private Coroutine shakeCoroutine;

    int perfectMultiplier = 0;

    public GameObject perfectBonusStreakHolder, xPBoostText;
    public TextMeshProUGUI perfectText;
    Vector3 streakHolderInitialPosition;
    public int bonusStreakHolderAnimationTargetPosition = 100;
    

    public void AddScore()
    {
        if (MusicPlayer.Instance != null)
        {
            if (MusicPlayer.Instance.GetComponent<AudioSource>().volume > 0.45f)
            {
                FadeAudio(0.45f, MusicPlayer.Instance.GetComponent<AudioSource>(), 2f);
                FadePitch(1f, MusicPlayer.Instance.GetComponent<AudioSource>(), 0.45f);

                //MusicPlayer.Instance.ChangeVolumeIntensity(0.45f);
                //MusicPlayer.Instance.ChangePitch(1f);
            }
        }
        

        GameManager.instance.XPHolder.transform.DOScale(1.25f, 0.25f).OnComplete(() => {

            GameManager.instance.XPHolder.transform.DOScale(1, 0.25f);
        
        });

        perfectMultiplier = 0;
        GameManager.instance.UpdateXPTextOnPlanetOrbitEnter(perfectMultiplier);
    }

    public void AddScorePerfect()
    {
        StopAllCoroutines();

        if (MusicPlayer.Instance != null)
        {
            if (MusicPlayer.Instance.GetComponent<AudioSource>().volume < 0.55f)
            {
                FadeAudio(0.55f, MusicPlayer.Instance.GetComponent<AudioSource>(), 2f);
                FadePitch(1.15f, MusicPlayer.Instance.GetComponent<AudioSource>(), 0.55f);

                //MusicPlayer.Instance.ChangeVolumeIntensity(0.55f);
                //MusicPlayer.Instance.ChangePitch(1.15f);
            }
        }
        

        
        perfectBonusStreakHolder.SetActive(false);
        perfectMultiplier += 1;
        ShakeCamera(camShakeDuration, camShakeIntensity);

        int xPBoost = 1;
        if (PlayerPrefs.GetInt("XPBoost") == 1) {
            xPBoost = 2;
            xPBoostText.SetActive(true);
        }

        GameManager.instance.UpdateXPTextOnPlanetOrbitEnter(perfectMultiplier);

        //if (perfectMultiplier < 2) return;

        perfectText.text = "Direct Hit! STREAK x" + perfectMultiplier + "! +" + perfectMultiplier * xPBoost + " XP";

        perfectBonusStreakHolder.SetActive(true);
        perfectBonusStreakHolder.transform.localPosition = streakHolderInitialPosition;
        //perfectBonusStreakHolder.GetComponent<DOTweenAnimation>().DOPlay();

        //perfectBonusStreakHolder.transform.DOMoveY(bonusStreakHolderAnimationTargetPosition, 0.5f);

        

        Debug.Log("Perfect!");

        BounceText(perfectText, perfectTextBounceHeight, perfectTextBounceDuration);

        StartCoroutine(DelayedObjectDisabling(perfectBonusStreakHolder, 2.5f));
    }

    public void FadeAudio(float targetVolume, AudioSource audioSource, float duration)
    {
        //audioSource.volume = targetVolume;
        StartCoroutine(FadeVolumeCoroutine(targetVolume, audioSource, duration));
    }

    // Coroutine to handle the fade
    private IEnumerator FadeVolumeCoroutine(float targetVolume, AudioSource audioSource, float duration)
    {
        Debug.Log("FadeVolumeCoroutineWorking");
        

        float startVolume = audioSource.volume;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            Debug.Log(audioSource.gameObject.name);

            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = targetVolume; // Ensure it ends exactly on target
    }

    public void FadePitch(float targetPitch, AudioSource audioSource, float duration)
    {
        StartCoroutine(FadePitchCoroutine(targetPitch, audioSource, duration));
    }

    // Coroutine to handle the fade
    private IEnumerator FadePitchCoroutine(float targetPitch, AudioSource audioSource, float duration)
    {
        float startPitch = audioSource.pitch;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            audioSource.pitch = Mathf.Lerp(startPitch, targetPitch, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        audioSource.pitch = targetPitch; // Ensure it ends exactly on target
    }


    IEnumerator DelayedObjectDisabling(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }


    public void BounceText(TextMeshProUGUI textMeshPro, float bounceHeight = 30f, float duration = 0.5f)
    {
        if (textMeshPro == null) return;
        StartCoroutine(BounceRoutine(textMeshPro, bounceHeight, duration));
    }

    private IEnumerator BounceRoutine(TextMeshProUGUI textMeshPro, float bounceHeight, float duration)
    {
        Vector3 originalPosition = textMeshPro.transform.localPosition;
        Vector3 targetPosition = originalPosition + new Vector3(0, bounceHeight, 0);
        Color originalColor = textMeshPro.color;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float bounce = Mathf.Sin(t * Mathf.PI);
            textMeshPro.transform.localPosition = Vector3.Lerp(originalPosition, targetPosition, bounce);
            textMeshPro.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1 - t); // Fading effect
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        textMeshPro.transform.localPosition = originalPosition;
    }


    // Call this method to shake the camera
    public void ShakeCamera(float duration, float intensity)
    {
        // Store the original position if not already shaking
        if (shakeCoroutine == null)
            originalPosition = Camera.main.transform.localPosition;

        // Stop any existing shake
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        // Start a new shake coroutine
        shakeCoroutine = StartCoroutine(Shake(duration, intensity));
    }

    private IEnumerator Shake(float duration, float intensity)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Calculate a random offset based on intensity
            Vector3 randomOffset = Random.insideUnitSphere * intensity;

            // Apply the random offset to the camera position
            Camera.main.transform.localPosition = originalPosition + randomOffset;

            // Increment the elapsed time
            elapsed += Time.deltaTime;

            yield return null;
        }

        // Reset to original position when done
        Camera.main.transform.localPosition = originalPosition;
        shakeCoroutine = null;

        //yield return new WaitForSecondsRealtime(2.5f);
        //perfectBonusStreakHolder.SetActive(false);

    }

    // Start is called before the first frame update
    void Start()
    {
        streakHolderInitialPosition = perfectBonusStreakHolder.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
