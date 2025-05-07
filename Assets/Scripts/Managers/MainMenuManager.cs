using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Cinemachine;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    //public Transform dummyShip, dummyPlanet;

    public float orbitingSpeed = 10;

    public GameObject removeAdsButton/*, dummyRemoveAdsBtn*/;

    public TextMeshProUGUI highScoreText, lastScoreText, leaderBoardNameAndScoreText;

    public GameObject leaderBoardsPanel, usernamePromptMenu, leaderBoardMenu, shipSkinsRewardsPanel,
        loadingPanel;

    public TMP_InputField nicknameInputField;

    public LeaderboardManager leaderboardManager;

    public GameObject adUnavailableText;

    [Header("Hopper Levels")]
    public int hopperMaxLevel = 10;
    public int XPThresholdForLevelUpgrade = 100;
    public Image nextLevelFillBar;
    float barFillAmountValue;
    float barFillAnimationTime = 3, currentBarFillAmountTime = 0;
    public TextMeshProUGUI currentLevelText, nextLevelText, currentXPText;
    int totalXP, currentLevel, nextLevel;
    [Header("Skins/Levels Rewards")]
    public CinemachineVirtualCamera vCam;
    public GameObject[] shipSkins;
    public TextMeshProUGUI shipNameText;
    public string[] shipNameTexts;
    int currentShip;
    public GameObject selectShipButton, buyAllShipsButton, /*dummyBuyAllShipsButton,*/ selectedShipObject;
    public TextMeshProUGUI unlockShipLevelText;
    [Header("Settings")]
    public GameObject settingsPanel;
    public Image soundFXToggle, musicToggle;
    public MainMenuSoundsManager soundManager;
    [Header("XP Boost")]
    public GameObject XPBoostAdButton;
    public GameObject xPBoostTextObject;


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        //nextLevelFillBar.fillAmount = 0.44f;
        //currentXPText.text = "yoyo";

        Application.targetFrameRate = 60;

        if (!PlayerPrefs.HasKey("Sound")) PlayerPrefs.SetInt("Sound", 1);

        if (!PlayerPrefs.HasKey("Music")) PlayerPrefs.SetInt("Music", 1);

        if (!PlayerPrefs.HasKey("XP")) PlayerPrefs.SetInt("XP", 0);

        if (!PlayerPrefs.HasKey("CurrentLevel")) PlayerPrefs.SetInt("CurrentLevel", 0);

        if (!PlayerPrefs.HasKey("RemoveAds"))
        {
            PlayerPrefs.SetInt("RemoveAds", 0);
        }
        if (PlayerPrefs.GetInt("RemoveAds") == 1) {
            removeAdsButton.SetActive(false);
            //dummyRemoveAdsBtn.SetActive(false);
        } 

        if (!PlayerPrefs.HasKey("HighScore")) PlayerPrefs.SetInt("HighScore", 0);
        if (!PlayerPrefs.HasKey("LastScore")) PlayerPrefs.SetInt("LastScore", 0);

        highScoreText.text = "Best: " + PlayerPrefs.GetInt("HighScore").ToString();
        lastScoreText.text = "Last: " + PlayerPrefs.GetInt("LastScore").ToString();

        if (!PlayerPrefs.HasKey("XPBoost")) PlayerPrefs.SetInt("XPBoost", 0);

        for(int i = 1; i < hopperMaxLevel; i++)
        {
            if (!PlayerPrefs.HasKey("ShipUnlocked" + i))
            {
                PlayerPrefs.SetInt("ShipUnlocked" + i, 0);
            }
        }


        if (!PlayerPrefs.HasKey("CurrentShip"))
        {
            PlayerPrefs.SetInt("CurrentShip", 0);
        }

        currentShip = PlayerPrefs.GetInt("CurrentShip");

        if (PlayerPrefs.HasKey("Username")) {
            SetLeaderboardMenuNameAndScoreText();
            leaderboardManager.UpdateEntry();
        }

        totalXP = PlayerPrefs.GetInt("XP");

        currentLevel = Mathf.FloorToInt(totalXP / XPThresholdForLevelUpgrade);
        if (nextLevel < hopperMaxLevel)
            nextLevel = currentLevel + 1;
        else nextLevel = hopperMaxLevel;

        PlayerPrefs.SetInt("CurrentLevel", currentLevel);

        Debug.Log(nextLevel);

        for (int i = 1; i < nextLevel; i++)
        {
            PlayerPrefs.SetInt("ShipUnlocked" + i, 1);
            Debug.Log("Unlocked ship" + (nextLevel - 1));
        }

        if (PlayerPrefs.GetInt("Sound") == 1) soundFXToggle.gameObject.SetActive(true);
        else soundFXToggle.gameObject.SetActive(false);

        if (PlayerPrefs.GetInt("Music") == 1) musicToggle.gameObject.SetActive(true);
        else musicToggle.gameObject.SetActive(false);

        if(PlayerPrefs.GetInt("XPBoost") == 1)
        {
            XPBoostAdButton.SetActive(false);
            xPBoostTextObject.SetActive(true);
        }
        else
        {
            XPBoostAdButton.SetActive(true);
            xPBoostTextObject.SetActive(false);
        }

    }

    public void ShowAdUnavailableText()
    {
        StopCoroutine(DelayedDisabling(adUnavailableText, 2));

        adUnavailableText.SetActive(true);

        StartCoroutine(DelayedDisabling(adUnavailableText, 2));
    }

    IEnumerator DelayedDisabling(GameObject objectToDisable, float t)
    {
        yield return new WaitForSecondsRealtime(t);

        objectToDisable.SetActive(false);
    }

    public void XPBoostRewardAdClicked()
    {

        Debug.Log("Working");

        PlayerPrefs.SetInt("Reward", 1);
        AdsManager.instance.ShowRewardedAd();
    }

    public void GiveXPBoostReward()
    {
        PlayerPrefs.SetInt("XPBoost", 1);
        xPBoostTextObject.SetActive(true);
        XPBoostAdButton.SetActive(false);
    }

    public void ToggleMusic()
    {
        if(PlayerPrefs.GetInt("Music") == 1)
        {
            PlayerPrefs.SetInt("Music", 0);
            musicToggle.gameObject.SetActive(false);
            MusicPlayer.Instance.StopMusic();
        }
        else
        {
            PlayerPrefs.SetInt("Music", 1);
            musicToggle.gameObject.SetActive(true);
            MusicPlayer.Instance.PlayMusic();
        }

        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            soundManager.PlayButtonClickSound();
        }
    }

    public void ToggleSoundFX()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            PlayerPrefs.SetInt("Sound", 0);
            soundFXToggle.gameObject.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt("Sound", 1);
            soundFXToggle.gameObject.SetActive(true);
        }

        if(PlayerPrefs.GetInt("Sound") == 1)
        {
            soundManager.PlayButtonClickSound();
        }
    }

    void SetLeaderboardMenuNameAndScoreText()
    {
        leaderBoardNameAndScoreText.text = PlayerPrefs.GetString("Username") + " | "
            + PlayerPrefs.GetInt("HighScore") + " (Best Score)";
    }

    public void OpenLeaderBoards()
    {
        //leaderboardManager.LoadEntries();

        leaderBoardsPanel.SetActive(true);

        usernamePromptMenu.SetActive(false);
        leaderBoardMenu.SetActive(false);

        if (!PlayerPrefs.HasKey("Username")) usernamePromptMenu.SetActive(true);
        else leaderBoardMenu.SetActive(true);
    }

    public void SubmitUsernameButton()
    {
        string errorText = "Name must be between 3 to 10 characters!";

        if (nicknameInputField.text.Length > 3 && nicknameInputField.text.Length < 10 
            && nicknameInputField.text != errorText)
        {
            int randomIDTag = Random.Range(1000, 10000);

            PlayerPrefs.SetString("Username", nicknameInputField.text + "#"+ randomIDTag);

            SetLeaderboardMenuNameAndScoreText();

            leaderboardManager.UpdateEntry();
            //leaderboardManager.LoadEntries();

            usernamePromptMenu.SetActive(false);
            leaderBoardMenu.SetActive(true);
        }
        else
        {
            nicknameInputField.text = errorText;
        }
    }

    public void OpenCurrentShip()
    {
        currentShip = PlayerPrefs.GetInt("CurrentShip");

        //foreach(GameObject ship in shipSkins) ship.SetActive(false);
        vCam.Follow = shipSkins[currentShip].transform;
        //vCam.LookAt = shipSkins[currentShip].transform;

        shipSkins[currentShip].SetActive(true);
        shipNameText.text = shipNameTexts[currentShip].ToString();


        selectShipButton.SetActive(false);
        unlockShipLevelText.gameObject.SetActive(false);
        buyAllShipsButton.gameObject.SetActive(false);
        //dummyBuyAllShipsButton.SetActive(false);
        selectedShipObject.SetActive(true);

        //PlayerPrefs.SetInt("CurrentShip", currentShip);
    }


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start working!");

        if (!AdsManager.instance.isBannerLoaded) AdsManager.instance.LoadBannerAd();
        AdsManager.instance.HideBannerAd();
        leaderboardManager.LoadEntries();

        //Managing hopper levels

        currentXPText.text = "XP: " + totalXP;

        if(currentLevel < hopperMaxLevel)
        {
            currentLevelText.text = "" + currentLevel;
            nextLevelText.text = "LVL: " + (nextLevel);

            //Debug.Log((float)(totalXP % XPThresholdForLevelUpgrade) / 100);

            float fillBarAmount = (float)(totalXP % XPThresholdForLevelUpgrade) / XPThresholdForLevelUpgrade;

            Debug.Log(fillBarAmount);

            barFillAmountValue = fillBarAmount;


            //Hide this
            nextLevelFillBar.fillAmount = fillBarAmount;
        }
        else
        {
            currentLevelText.text = "10";

            barFillAmountValue = 1;

            //nextLevelFillBar.fillAmount = 1;
            nextLevelText.gameObject.SetActive(false);
        }

        OpenCurrentShip();

        MusicPlayer.Instance.ChangeVolumeIntensity(0.4f);
        MusicPlayer.Instance.ChangePitch(1f);
        MusicPlayer.Instance.PlayMainMenuMusic();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Updateworking");

        if (currentBarFillAmountTime < barFillAnimationTime)
        {
            currentBarFillAmountTime += Time.deltaTime;

            nextLevelFillBar.fillAmount = barFillAmountValue * (currentBarFillAmountTime / barFillAnimationTime);
        }

        //dummyShip.RotateAround(dummyPlanet.position, Vector3.forward, orbitingSpeed * Time.deltaTime);
    }

    public void OpenShipSkins()
    {
        shipSkinsRewardsPanel.SetActive(true);
        OpenCurrentShip();
    }

    public void OpenShipSkinsWithNextReward()
    {
        shipSkinsRewardsPanel.SetActive(true);

        //foreach (var ship in shipSkins) ship.SetActive(false);

        if (nextLevel < hopperMaxLevel)
            currentShip = nextLevel;
        else currentShip = hopperMaxLevel - 1;

        if (currentLevel < shipSkins.Length)
        {
            vCam.Follow = shipSkins[currentShip].transform;
            //vCam.LookAt = shipSkins[currentShip].transform;

            shipSkins[currentShip].SetActive(true);
            shipNameText.text = shipNameTexts[currentShip].ToString();
        }
        else {
            vCam.Follow = shipSkins[currentShip].transform;
            //vCam.LookAt = shipSkins[currentShip].transform;

            shipSkins[shipSkins.Length - 1].SetActive(true);
            shipNameText.text = shipNameTexts[currentShip].ToString();
        } 

        if(PlayerPrefs.GetInt("ShipUnlocked"+currentShip) == 1)
        {
            selectShipButton.gameObject.SetActive(true);
            unlockShipLevelText.gameObject.SetActive(false);
            buyAllShipsButton.gameObject.SetActive(false);
            //dummyBuyAllShipsButton.SetActive(false);
        }
        else
        {
            selectShipButton.gameObject.SetActive(false);
            unlockShipLevelText.gameObject.SetActive(true);
            unlockShipLevelText.text = "Unlock at Level " + nextLevel;
            buyAllShipsButton.gameObject.SetActive(true);
            //dummyBuyAllShipsButton.SetActive(true);
        }
    }

    public void SelectCurrentShip()
    {
        if (currentShip > 0)
        {
            if (PlayerPrefs.GetInt("ShipUnlocked" + (currentShip)) == 1)
            {
                PlayerPrefs.SetInt("CurrentShip", currentShip);
            }
        }
        else PlayerPrefs.SetInt("CurrentShip", currentShip);

        //shipSkinsRewardsPanel.SetActive(false);

        selectShipButton.SetActive(false);
    }

    public void NextShipSkin()
    {
        //foreach(var ship in shipSkins) ship.SetActive(false);


        currentShip++;
        if(currentShip >= shipSkins.Length)
        {
            currentShip = shipSkins.Length - 1;
        }

        vCam.Follow = shipSkins[currentShip].transform;
        //vCam.LookAt = shipSkins[currentShip].transform;

        shipSkins[currentShip].SetActive(true);
        shipNameText.text = shipNameTexts[currentShip].ToString();
        Debug.Log(currentShip);

        if (currentShip > 0)
        {
            Debug.Log(currentShip + 1);

            if (PlayerPrefs.GetInt("ShipUnlocked" + (currentShip)) == 1)
            {
                selectShipButton.SetActive(true);
                unlockShipLevelText.gameObject.SetActive(false);
                buyAllShipsButton.gameObject.SetActive(false);
                //dummyBuyAllShipsButton.SetActive(false);

                

            }
            else
            {
                unlockShipLevelText.text = "Unlock at Level " + (int)(currentShip);
                selectShipButton.SetActive(false);
                unlockShipLevelText.gameObject.SetActive(true);
                buyAllShipsButton.gameObject.SetActive(true);
                //dummyBuyAllShipsButton.SetActive(true);
            }
        }
        else
        {
            selectShipButton.SetActive(true);
            unlockShipLevelText.gameObject.SetActive(false);
            buyAllShipsButton.gameObject.SetActive(false);
            //dummyBuyAllShipsButton.SetActive(false);
        }

        if (PlayerPrefs.GetInt("CurrentShip") == currentShip)
        {
            selectShipButton.SetActive(false);
            selectedShipObject.SetActive(true);
        }
        else
        {
            selectedShipObject.SetActive(false);
        }
    }

    public void PreviousShip()
    {

        //foreach (var ship in shipSkins) ship.SetActive(false);

        currentShip--;
        if (currentShip < 0)
        {
            currentShip = 0;
        }

        vCam.Follow = shipSkins[currentShip].transform;
        //vCam.LookAt = shipSkins[currentShip].transform;

        shipSkins[currentShip].SetActive(true);
        shipNameText.text = shipNameTexts[currentShip].ToString();
        Debug.Log(currentShip);

        if (currentShip > 0)
        {
            if (PlayerPrefs.GetInt("ShipUnlocked" + (int)(currentShip)) == 1)
            {
                selectShipButton.SetActive(true);
                unlockShipLevelText.gameObject.SetActive(false);
                buyAllShipsButton.gameObject.SetActive(false);
                //dummyBuyAllShipsButton.SetActive(false);

                
            }
            else
            {
                Debug.Log("Locked");
                unlockShipLevelText.text = "Unlock at Level " + (currentShip);
                selectShipButton.SetActive(false);
                unlockShipLevelText.gameObject.SetActive(true);
                buyAllShipsButton.gameObject.SetActive(true);
                //dummyBuyAllShipsButton.SetActive(true);
            }
        }
        else
        {
            selectShipButton.SetActive(true);
            unlockShipLevelText.gameObject.SetActive(false);
            buyAllShipsButton.gameObject.SetActive(false);
            //dummyBuyAllShipsButton.SetActive(false);
        }

        if (PlayerPrefs.GetInt("CurrentShip") == currentShip)
        {
            selectShipButton.SetActive(false);
            selectedShipObject.SetActive(true);
        }
        else
        {
            selectedShipObject.SetActive(false);
        }
    }

    public void OpenShipSkinWithCurrentReward()
    {
        shipSkinsRewardsPanel.SetActive(true);

        //foreach (var ship in shipSkins) ship.SetActive(false);
        
        currentShip = currentLevel;

        if (currentLevel < shipSkins.Length)
        {
            vCam.Follow = shipSkins[currentShip].transform;
            //vCam.LookAt = shipSkins[currentShip].transform;

            shipSkins[currentShip].SetActive(true);
        }
        else {

            vCam.Follow = shipSkins[currentShip].transform;
            //vCam.LookAt = shipSkins[currentShip].transform;

            shipSkins[shipSkins.Length - 1].SetActive(true);
            shipNameText.text = shipNameTexts[currentShip].ToString();
        } 

        if (PlayerPrefs.GetInt("ShipUnlocked" + currentShip) == 1)
        {
            selectShipButton.gameObject.SetActive(true);
            unlockShipLevelText.gameObject.SetActive(false);
            buyAllShipsButton.gameObject.SetActive(false);
            //dummyBuyAllShipsButton.SetActive(false);
        }
        else
        {
            selectShipButton.gameObject.SetActive(false);
            unlockShipLevelText.gameObject.SetActive(true);
            buyAllShipsButton.gameObject.SetActive(true);
            //dummyBuyAllShipsButton.SetActive(true);
        }
    }

    public void BuyAllShips()
    {
        for (int i = 1; i < hopperMaxLevel; i++)
        {
            PlayerPrefs.SetInt("ShipUnlocked" + i, 1);
        }

        buyAllShipsButton.SetActive(false);
        //dummyBuyAllShipsButton.SetActive(false);
        selectShipButton.gameObject.SetActive(true);
        unlockShipLevelText.gameObject.SetActive(false);
    }

    public void RemoveAds()
    {
        PlayerPrefs.SetInt("RemoveAds", 1);
        AdsManager.instance.HideBannerAd();
        removeAdsButton.SetActive(false);
        //dummyRemoveAdsBtn.SetActive(false);
    }

    public void LoadScene(int sceneId)
    {
        loadingPanel.SetActive(true);
        SceneManager.LoadSceneAsync(sceneId);
    }

    public void BounceButton(GameObject gameObject)
    {
        Bounce(gameObject, 1.2f, 0.2f);
    }

    //Button bounce effect
    public void Bounce(GameObject target, float bounceScale = 1.2f, float duration = 0.2f)
    {
        StartCoroutine(BounceCoroutine(target.transform, bounceScale, duration));
    }

    private IEnumerator BounceCoroutine(Transform targetTransform, float bounceScale, float duration)
    {
        targetTransform.localScale = Vector3.one;
        Vector3 originalScale = targetTransform.localScale;
        Vector3 targetScale = originalScale * bounceScale;
        float halfDuration = duration / 2f;
        float time = 0f;

        // Scale up
        while (time < halfDuration)
        {
            float t = time / halfDuration;
            targetTransform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            time += Time.deltaTime;
            yield return null;
        }
        targetTransform.localScale = targetScale;

        time = 0f;

        // Scale back down
        while (time < halfDuration)
        {
            float t = time / halfDuration;
            targetTransform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            time += Time.deltaTime;
            yield return null;
        }
        targetTransform.localScale = originalScale;
    }
}
