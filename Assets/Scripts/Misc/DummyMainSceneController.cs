using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DummyMainSceneController : MonoBehaviour
{
    public Transform dummyShip, dummyPlanet;

    public float orbitingSpeed = 10;

    public GameObject removeAdsButton;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("RemoveAds"))
        {
            PlayerPrefs.SetInt("RemoveAds", 0);
        }
        if (PlayerPrefs.GetInt("RemoveAds") == 1) removeAdsButton.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        AdsManager.instance.LoadBannerAd();
    }

    // Update is called once per frame
    void Update()
    {
        dummyShip.RotateAround(dummyPlanet.position, Vector3.forward, orbitingSpeed * Time.deltaTime);
    }

    public void RemoveAds()
    {
        PlayerPrefs.SetInt("RemoveAds", 1);
        AdsManager.instance.HideBannerAd();
        removeAdsButton.SetActive(false);
    }

    public void LoadScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }
}
