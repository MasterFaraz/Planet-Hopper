using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlanetRandomizer: MonoBehaviour
{
    public GameObject[] planetsToRandomizeFrom;
    public bool chooseFromEditor = false;
    public int editorPlanetIndex = 0;
    public float planetMinimumScale = 0.9f, planetMaximumScale = 1.3f;

    public SpriteRenderer orbitRing, orbitRingRipple;
    public Color ringNormalColor, ringShipOrbitColor;

    public SphereCollider planetCollider;

    float orbitRingSize, orbitRingOriginalSize, planetColliderSize, planetColliderOriginalSize;

    public float minimumOrbitPercentage = 70;

    bool colorAlreadyChanged = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach(var planet in planetsToRandomizeFrom) planet.gameObject.SetActive(false);

        if (!chooseFromEditor)
        {
            int random = Random.Range(0, planetsToRandomizeFrom.Length);
            planetsToRandomizeFrom[random].SetActive(true);
        }
        else
        {
            planetsToRandomizeFrom[editorPlanetIndex].SetActive(true);
        }

        float randomScale = Random.Range(planetMinimumScale, planetMaximumScale);
        transform.localScale = Vector3.one * randomScale;

        orbitRingOriginalSize = orbitRing.transform.localScale.x;
        planetColliderOriginalSize = planetCollider.radius;

        planetColliderSize = planetColliderOriginalSize;
        orbitRingSize = orbitRingOriginalSize;

        DecreaseRingAndOrbitSizeByPercentage(GameManager.instance.orbitDecrementPercentage);

        orbitRing.color = ringNormalColor;
        orbitRingRipple.color = ringNormalColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void ChangeOrbitRingColorOnPlayerEnter()
    {
        if (colorAlreadyChanged) return;

        if (orbitRingRipple != null) {
            //orbitRingRipple.gameObject.SetActive(true);
            //orbitRingRipple.color = ringShipOrbitColor;
        }
        

        orbitRing.color = ringShipOrbitColor;
        orbitRing.GetComponent<DOTweenAnimation>().DORestart();
        orbitRing.GetComponent <DOTweenAnimation>().DOKill();
        colorAlreadyChanged = true;
        Debug.Log("Tried");

        GameManager.instance.ChangeBackgroundNormal();
    }
    public void ChangeOrbitRingColorOnPlayerEnterPerfect()
    {
        if (colorAlreadyChanged) return;

        if (orbitRingRipple != null)
        {
            orbitRingRipple.gameObject.SetActive(true);
            orbitRingRipple.color = ringShipOrbitColor;
        }

        orbitRing.color = ringShipOrbitColor;
        orbitRing.GetComponent<DOTweenAnimation>().DOKill();
        orbitRing.GetComponent<DOTweenAnimation>().DOKill();
        colorAlreadyChanged = true;
        Debug.Log("Tried");

        GameManager.instance.ChangeBackgroundPerfect();
    }

    public void DecreaseRingAndOrbitSizeByPercentage(float percentage)
    {
        orbitRingSize -= (percentage * orbitRingOriginalSize) / 100;
        planetColliderSize -= (percentage * planetColliderOriginalSize) / 100;

        orbitRing.transform.localScale = Vector3.one * orbitRingSize;
        planetCollider.radius = planetColliderSize;

        Debug.Log(percentage + " decreased. New percentage: " + orbitRingSize / orbitRingOriginalSize);
    }

    public void IncreaseRingAndOrbitSizeByPercentage(float percentage)
    {
        orbitRingSize += (percentage * orbitRingOriginalSize) / 100;
        planetColliderSize += (percentage * planetColliderOriginalSize) / 100;

        orbitRing.transform.localScale = Vector3.one * orbitRingSize;
        planetCollider.radius = planetColliderSize;
    }
}
