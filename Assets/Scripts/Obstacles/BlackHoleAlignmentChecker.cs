using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleAlignmentChecker : MonoBehaviour
{
    public enum ShipTarget { Planet, Blackhole}

    public ShipTarget shipTarget;

    SpaceshipController spaceShipController;

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            spaceShipController = other.gameObject.GetComponent<SpaceshipController>();
        }

        

        if (spaceShipController != null) {

            if (shipTarget == ShipTarget.Planet)
            {
                spaceShipController.currentBlackHoleTarget = spaceShipController.planetTarget;
            }
            else if (shipTarget == ShipTarget.Blackhole)
            {
                spaceShipController.currentBlackHoleTarget = spaceShipController.blackHoleTarget;
            }

            //Debug.Log(gameObject.name + shipTarget);

        }

    }


    public void SetPlanet()
    {
        shipTarget = ShipTarget.Planet;
        Debug.Log("PlanetSet");
    }

    public void SetBlackHole()
    {
        shipTarget = ShipTarget.Blackhole;
        Debug.Log("BlackHoleSet");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
