using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayUIManager : MonoBehaviour
{
    public GameObject pausePanel, gameplayPanel, gameOverPanel;

    public GameObject revivePanel;
    public Image reviveButtonImage;
    float currentReviveTimer, maxReviveTimer = 3;
    bool doReviveImageFillAnimation = false;
    public Text tapToLaunchText;

    public GameObject watchAdToReviveText, freeReviveText;

    [HideInInspector]
    public bool isFirstLaunch = false;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("FirstLaunch")) PlayerPrefs.SetInt("FirstLaunch", 0);
        if(PlayerPrefs.GetInt("FirstLaunch") < 2)
        {
            isFirstLaunch = true;
            tapToLaunchText.text = "Tap to Launch!";
            tapToLaunchText.gameObject.SetActive(true);
            PlayerPrefs.SetInt("FirstLaunch", PlayerPrefs.GetInt("FirstLaunch") + 1);
        }
        else isFirstLaunch = false;
    }

    public void ShowTapToSwitchPlanetTextObject()
    {
        tapToLaunchText.text = "Tap to land on the next planet!";
        tapToLaunchText.gameObject.SetActive(true);
    }

    public void ShowReviveUI()
    {
        revivePanel.SetActive(true);
        currentReviveTimer = maxReviveTimer;
        doReviveImageFillAnimation=true;
    }

    public void OnRevivePressed()
    {
        if(GameManager.instance.freeRevive > 0)
        {
            ReviveShip();
            GameManager.instance.freeRevive = 0;
            //GameManager.instance.freeReviveAlreadyGiven = true;
            return;
        }

        if(AdsManager.instance != null)
        {
            if (AdsManager.instance.IsRewardedAdAwailable())
            {
                doReviveImageFillAnimation = false;
                if (AdsManager.instance != null)
                    AdsManager.instance.ShowRewardedAd();
                PlayerPrefs.SetInt("Reward", 0);
            }
            else
            {
                if (AdsManager.instance != null)
                    AdsManager.instance.LoadRewardedAd();
            }

        }
            
    }

    public void ReviveShip()
    {
        doReviveImageFillAnimation = false;
        revivePanel.SetActive(false);
        gameplayPanel.SetActive(true);
        GameManager.instance.ReviveShip();
    }

    private void Update()
    {
        if (doReviveImageFillAnimation)
        {
            if(currentReviveTimer > 0)
            {
                reviveButtonImage.fillAmount = (currentReviveTimer / maxReviveTimer);
                currentReviveTimer -= Time.deltaTime * (1/Time.timeScale);
            }
            else
            {
                revivePanel.SetActive(false);
                doReviveImageFillAnimation = false;
                GameManager.instance.GameOver();
            }
        }
    }

    public void NoThanksGameOver()
    {
        revivePanel.SetActive(false);
        doReviveImageFillAnimation = false;
        GameManager.instance.GameOver();
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        gameplayPanel.SetActive(false);
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        gameplayPanel.SetActive(true);
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        pausePanel.SetActive(false);
        gameplayPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        Time.timeScale = 1;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;

        PlayerPrefs.SetInt("XPBoost", 0);

        if (!GameManager.instance.scoreAlreadyAdded)
            GameManager.instance.SaveXPAndScores();
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;

        PlayerPrefs.SetInt("XPBoost", 0);

        if (!GameManager.instance.scoreAlreadyAdded)
            GameManager.instance.SaveXPAndScores();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
