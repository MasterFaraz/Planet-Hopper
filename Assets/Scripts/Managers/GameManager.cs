using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector]
    public bool canCamFollowPlayer = true;
    [HideInInspector]
    public bool isHorizontal = false, isVertical = true;

    [Header("UI Elements")]
    public GameObject XPHolder;
    public TextMeshProUGUI xPText, gameOverPanelScoreText, gameOverPanelBestScoreText;
    public Text powerupText, pickupIncomingText;
    public GameObject xpHolderPerfectStreakOutline;

    //public Slider orbitSpeedSlider;
    //public Image switchPlanetImage;
    [HideInInspector]
    public int currentPlanet = 0;
    int score, totalXP, currentLevel, xpBoost = 1;
    public GameObject gameOverText;

    [Header("Player/Ship References")]
    public GameObject playerExplosion;
    public GameObject player;
    //[HideInInspector]
    //public float proceduralSpeedIncrement = 0;

    [Header("Procedural Difficulty Settings")]
    public float maximumOrbitSpeed = 80f;
    public float maximumOscillationSpeed = 2f, maximumOrbitDecrementAmount = 30f;
    public float proceduralOrbitSpeedBoostAmount = 0.1f, proceduralPlanetOscillationSpeedBoostAmount = 0.1f,
        proceduralOrbitDecrementAmount = 5, orbitDecrementPercentage = 0;
    public int giveBoostAfterEachNumberPlanets = 5;
    public float planetOrbitingSpeed = 20f, planetOscillationSpeed = 1.5f;

    //public Material orbitGlowMaterial;


    [Header("Other Managers")]
    public PlanetPoolingController planetPoolingController;
    public GameplayUIManager gameplayUIManager;
    public GameplayScoreManager gameplayScoreManager;
    public GameplaySoundsManager gameplaySoundsManager;

    [Header("Planet References")]
    public GameObject currentPlanetGameObject;
    public float revivalMoveSpeed = 5;
    bool moveShipTowardsPlanetForRevival;
    int planetDestroyCounter = 0;
    public GameObject currentPlanetToDestroy;





    [HideInInspector]
    public bool scoreAlreadyAdded = false, freeReviveAlreadyGiven = false, firstPickupTextAlreadyShown = false;

    [Header("Environment Settings")]
    public Material backgroundNormalMaterial;
    public Material backGroundPerfectMaterial;
    public MeshRenderer backgroundRenderer;



    [HideInInspector]
    public bool driftedAway = false;

    int deathCount = 0;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(gameObject); }

        if (PlayerPrefs.GetInt("XPBoost") == 1) xpBoost = 2;

        if (!PlayerPrefs.HasKey("Death")) PlayerPrefs.SetInt("Death", 0);

        deathCount = PlayerPrefs.GetInt("Death");
    }

    // Start is called before the first frame update
    void Start()
    {
        //if (!PlayerPrefs.HasKey("XP")) PlayerPrefs.SetInt("XP", 0);
        totalXP = PlayerPrefs.GetInt("XP");

        currentLevel = PlayerPrefs.GetInt("CurrentLevel");

        score = 0;
        xPText.text = score.ToString();

        if (AdsManager.instance != null)
        {
            AdsManager.instance.LoadInterstitialAd();
            AdsManager.instance.LoadRewardedAd();
            //Debug.Log(AdsManager.instance.isBannerLoaded);
            if (!AdsManager.instance.isBannerLoaded) AdsManager.instance.LoadBannerAd();
            AdsManager.instance.ShowBannerAd();
        }

        if (MusicPlayer.Instance != null)
        {
            MusicPlayer.Instance.ChangeVolumeIntensity(0.45f);
            MusicPlayer.Instance.ChangePitch(1f);
            MusicPlayer.Instance.PlayGameplayMusic();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (moveShipTowardsPlanetForRevival)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position,
                currentPlanetGameObject.transform.position, revivalMoveSpeed * Time.deltaTime);
        }
    }

    public void ChangeBackgroundPerfect()
    {

        //FadeToMaterial(backGroundPerfectMaterial, backgroundRenderer);

        backgroundRenderer.material = backGroundPerfectMaterial;
        player.GetComponent<SpaceshipController>().shipTrail =
            player.GetComponent<SpaceshipController>().perfectTrail;
        xpHolderPerfectStreakOutline.SetActive(true);
    }

    //public float transitionDuration = 1.0f;

    //public void FadeToMaterial(Material targetMaterial, Renderer targetRenderer)
    //{
    //    StartCoroutine(FadeMaterialCoroutine(targetRenderer, targetMaterial));
    //}

    //private IEnumerator FadeMaterialCoroutine(Renderer renderer, Material targetMaterial)
    //{
    //    Material startMaterial = renderer.material;
    //    Material blendedMaterial = new Material(startMaterial); // create a temporary blend material

    //    renderer.material = blendedMaterial;

    //    float time = 0f;

    //    while (time < transitionDuration)
    //    {
    //        time += Time.deltaTime;
    //        float t = time / transitionDuration;

    //        // Interpolate each color channel (assuming both materials have _Color)
    //        if (startMaterial.HasProperty("_Color") && targetMaterial.HasProperty("_Color"))
    //        {
    //            Debug.Log("Has Property");

    //            Color startColor = startMaterial.color;
    //            Color endColor = targetMaterial.color;
    //            blendedMaterial.color = Color.Lerp(startColor, endColor, t);
    //        }

    //        yield return null;
    //    }

    //    // Ensure final material is exactly the target
    //    renderer.material = targetMaterial;
    //}

    public void ChangeBackgroundNormal()
    {

        //FadeToMaterial(backgroundNormalMaterial, backgroundRenderer);

        backgroundRenderer.material = backgroundNormalMaterial;
        player.GetComponent<SpaceshipController>().shipTrail =
            player.GetComponent<SpaceshipController>().normalTrail;
        xpHolderPerfectStreakOutline.SetActive(false);
    }

    public void ReviveShip()
    {
        driftedAway = false;

        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        currentPlanetGameObject.GetComponent<Collider>().enabled = true;
        currentPlanetGameObject.GetComponent<PlanetController>().planetDone = false;
        currentPlanetGameObject.GetComponent<PlanetController>().alreadyGameOverCalled = true;
        moveShipTowardsPlanetForRevival = true;
        player.GetComponent<Collider>().enabled = true;
        Time.timeScale = 1;
    }

    public void AddPlanet()
    {
        planetPoolingController.AddAPlanet();
    }

    public void UpdateXPTextOnPlanetOrbitEnter(int bonusMultiplier)
    {
        moveShipTowardsPlanetForRevival = false;

        if (bonusMultiplier > 0)
        {
            score += 1 * bonusMultiplier * xpBoost;
            //totalXP += 1 * bonusMultiplier;
        }
        else
        {
            score += 1 * xpBoost;
            //totalXP += 1;
        }

        currentPlanet += 1;
        //PlayerPrefs.SetInt("XP", totalXP);
        xPText.text = score.ToString();

        Debug.Log(currentPlanet % giveBoostAfterEachNumberPlanets);

        if (currentPlanet % giveBoostAfterEachNumberPlanets == 0)
        {

            if (!(planetOrbitingSpeed + proceduralOrbitSpeedBoostAmount > maximumOrbitSpeed))
                planetOrbitingSpeed += proceduralOrbitSpeedBoostAmount;
            else planetOrbitingSpeed = maximumOrbitSpeed;


            if (!(planetOscillationSpeed + proceduralPlanetOscillationSpeedBoostAmount > maximumOscillationSpeed))
                planetOscillationSpeed += proceduralPlanetOscillationSpeedBoostAmount;
            else planetOscillationSpeed = maximumOscillationSpeed;

            if (orbitDecrementPercentage < maximumOrbitDecrementAmount)
            {
                orbitDecrementPercentage += proceduralOrbitDecrementAmount;
            }
        }



    }

    //public void OrbitGlowMaterialEnter()
    //{
    //    Color targetColor = Color.green;

    //    orbitGlowMaterial.color = new Color(targetColor.r, targetColor.g, targetColor.b, 0.1f);
    //}

    //public void OrbitGlowMaterialExit()
    //{
    //    Color targetColor = Color.red;

    //    orbitGlowMaterial.color = new Color(targetColor.r, targetColor.g, targetColor.b, 0.1f);

    //}

    //public void GreenLightSwitching()
    //{
    //    switchPlanetImage.color = Color.green;
    //}

    //public void RedLightSwitching()
    //{
    //    switchPlanetImage.color = Color.red;
    //}

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }

    bool gameOver = false;

    public void GameOver()
    {
        if (gameOver) return;

        if (MusicPlayer.Instance != null)
        {
            MusicPlayer.Instance.ChangeVolumeIntensity(0.45f);
            MusicPlayer.Instance.ChangePitch(1f);
        }


        Time.timeScale = 1.0f;
        GameObject explosionEffect = Instantiate(playerExplosion);
        explosionEffect.transform.position = player.transform.position;
        explosionEffect.GetComponent<ParticleSystem>().Play();
        player.SetActive(false);
        gameOverText.SetActive(true);
        gameplayUIManager.gameplayPanel.SetActive(false);
        Destroy(explosionEffect, 5);

        totalXP += score;
        PlayerPrefs.SetInt("XP", totalXP);

        int levelBoost = (int)Mathf.Floor(score / 100);

        if (levelBoost > 0 && currentLevel < 10)
        {
            if (currentLevel + levelBoost > 10)
                PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel") + levelBoost);
            else PlayerPrefs.SetInt("CurrentLevel", 10);
            //Enable Level Boost Object
        }

        //Play sound
        if (driftedAway)
            gameplaySoundsManager.PlayerOverSound();
        else gameplaySoundsManager.PlayShipDriftingAwaySound();

        StartCoroutine(DelayedGameOverPanelDisplay());

        gameOver = true;
    }

    [HideInInspector]
    public int reviveChance = 1, freeRevive = 0;

    public void ShipDriftingAway()
    {
        if (reviveChance == 0 && freeRevive == 0)
        {
            //GameOver();
            return;
        }

        driftedAway = true;

        if (freeRevive == 1)
        {
            gameplayUIManager.freeReviveText.SetActive(true);
            gameplayUIManager.watchAdToReviveText.SetActive(false);
        }
        else
        {
            gameplayUIManager.freeReviveText.SetActive(false);
            gameplayUIManager.watchAdToReviveText.SetActive(true);
        }

        gameplaySoundsManager.PlayShipDriftingAwaySound();

        player.GetComponent<Collider>().enabled = false;
        player.GetComponent<Rigidbody>().velocity = Vector3.up * 1;
        Time.timeScale = 0.5f;
        gameplayUIManager.gameplayPanel.SetActive(false);
        gameplayUIManager.ShowReviveUI();
        //reviveChance -= 1;
    }

    IEnumerator DelayedGameOverPanelDisplay()
    {
        PlayerPrefs.SetInt("LastScore", score);
        if (score > PlayerPrefs.GetInt("HighScore")) PlayerPrefs.SetInt("HighScore", score);

        gameOverPanelScoreText.text = "Score: " + score.ToString();
        gameOverPanelBestScoreText.text = "Best Score: " + PlayerPrefs.GetInt("HighScore").ToString();

        yield return new WaitForSecondsRealtime(2f);

        if (deathCount >= 5)
        {
            if (AdsManager.instance != null)
                AdsManager.instance.ShowInterstitialAd();
        }
        else PlayerPrefs.SetInt("Death", deathCount + 1);

        Debug.Log("Death count: " + deathCount);

        gameplayUIManager.GameOver();

        scoreAlreadyAdded = true;
    }



    public void DestroyPlanet()
    {
        if (planetDestroyCounter >= 3)
        {
            GameObject tempPlanet = currentPlanetToDestroy.GetComponent<PlanetController>()
                .nextPlanet;

            Destroy(currentPlanetToDestroy);

            currentPlanetToDestroy = tempPlanet;
            //planetDestroyCounter--;
        }
        planetDestroyCounter++;
    }

    public void SaveXPAndScores()
    {
        totalXP += score;
        PlayerPrefs.SetInt("XP", totalXP);

        PlayerPrefs.SetInt("LastScore", score);
        if (score > PlayerPrefs.GetInt("HighScore")) PlayerPrefs.SetInt("HighScore", score);
    }

    public void PowerUpIncreaseOrbitSize()
    {
        powerupText.gameObject.SetActive(false);
        pickupIncomingText.gameObject.SetActive(false);
        currentPlanetGameObject.GetComponent<PlanetController>().nextPlanet.GetComponent<PlanetController>()
            .planetsHolder.GetComponent<PlanetRandomizer>().IncreaseRingAndOrbitSizeByPercentage(15);
        gameplaySoundsManager.PlayPowerUpSound();
        powerupText.text = "Powerup: orbit size increased.";
        powerupText.gameObject.SetActive(true);
        StartCoroutine(DisableGameobjectInTime(powerupText.gameObject, 2));
    }

    public void PowerUpAdd10XPBonus()
    {
        powerupText.gameObject.SetActive(false);
        pickupIncomingText.gameObject.SetActive(false);
        score += 10;
        xPText.text = score.ToString();
        gameplaySoundsManager.PlayPowerUpSound();
        powerupText.text = "Powerup: +10 XP.";
        powerupText.gameObject.SetActive(true);
        StartCoroutine(DisableGameobjectInTime(powerupText.gameObject, 2));
    }

    public void PowerUpFreeRevive()
    {
        powerupText.gameObject.SetActive(false);
        pickupIncomingText.gameObject.SetActive(false);
        freeRevive = 1;
        gameplaySoundsManager.PlayPowerUpSound();
        powerupText.text = "Powerup: Free Revive.";
        powerupText.gameObject.SetActive(true);
        StartCoroutine(DisableGameobjectInTime(powerupText.gameObject, 2));
    }

    public void ShowIncomingPowerUpText()
    {
        if (firstPickupTextAlreadyShown) return;

        firstPickupTextAlreadyShown = true;
        pickupIncomingText.gameObject.SetActive(true);
        StartCoroutine(DisableGameobjectInTime(pickupIncomingText.gameObject, 2));
    }

    IEnumerator DisableGameobjectInTime(GameObject gameObject, float t)
    {
        yield return new WaitForSecondsRealtime(t);
        gameObject.SetActive(false);
    }
}
