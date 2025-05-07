using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipSelectionController : MonoBehaviour
{
    public SpaceshipController parentSpaceShip;
    public GameObject[] spaceShipsToSelectFrom;
    public ParticleSystem[] spaceShipThrusters;

    private ObjectRotator objectRotator;

    int currentShip;

    // Start is called before the first frame update
    void Start()
    {
        currentShip = PlayerPrefs.GetInt("CurrentShip");
        if (currentShip < 0) currentShip = 0;
        else if (currentShip >= spaceShipThrusters.Length) currentShip = spaceShipThrusters.Length- 1;

        foreach(var ship in spaceShipsToSelectFrom) ship.gameObject.SetActive(false);

        spaceShipsToSelectFrom[currentShip].SetActive(true);

        objectRotator = spaceShipsToSelectFrom[currentShip].GetComponentInChildren<ObjectRotator>();

        if (objectRotator != null) {
            objectRotator.canRotate = false;
            parentSpaceShip.currentShipRotator = objectRotator;
        } 

        parentSpaceShip.shipBooster = spaceShipThrusters[currentShip];


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
