using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetAlignmentCheckerController : MonoBehaviour
{
    public PlanetController planet;
    [HideInInspector]
    public bool alreadyScoreAdded;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (alreadyScoreAdded) return;

            GameManager.instance.gameplaySoundsManager.PlayerEnterOrbitPerfectSound();

            GameManager.instance.gameplayScoreManager.AddScorePerfect();
            alreadyScoreAdded = true;

            planet.planetsHolder.GetComponent<PlanetRandomizer>().ChangeOrbitRingColorOnPlayerEnterPerfect();

            GetComponent<Collider>().enabled = false;
            //planet.GetComponent<Collider>().enabled = false;
        }

        
    }


    // Start is called before the first frame update
    void Start()
    {
        //if (nextPlanet != null)
        //    nextPlanet.SetPlanetForShipCrash();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
