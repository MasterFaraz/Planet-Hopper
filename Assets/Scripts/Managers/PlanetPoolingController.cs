using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlanetPoolingController : MonoBehaviour
{
    public GameObject[] planetsToPoolFrom;

    public Transform lastPlanetInScene;
    bool isVertical, isHorizontal;

    float AdditionalSpaceBetweenPlanets = 0, asteroidSpawnSpace = 5, blackHoleSpawnSpace = 5;

    public GameObject powerUpObject;

    public int maxPowerUpsToSpawn = 10;
    public int powerUpsSpawned = 0;
    public float powerUpSpawnChanceOutOfHundred = 50;

    [Header("-------------------------Obstacles---------------------------")]
    public GameObject asteroidSpawnerObject;
    public GameObject blackHoleSpawnerObject;
    public bool asteroid, horizontallyMoving, debris, blackHole, strongGravity;
    public float asteroidChanceOutOfHundred, horizontallyMovingChanceOutofHundred, DebrisChanceOutofHundred,
        blackHoleChanceOutOfHundred, strongGravityChanceOutOfHundred;
    int blackHolePosition, planetsAdded = 0;
    bool alreadyAddedObstacleThisRound;

    void SpawnAsteroidSpawner(Transform prevPlanet)
    {
        prevPlanet.GetComponent<PlanetController>().isPlanetWithAsteroids = true;
        GameObject asteroidSpawner = Instantiate(asteroidSpawnerObject);
        asteroidSpawner.transform.position = prevPlanet.transform.position;
        AdditionalSpaceBetweenPlanets += asteroidSpawnSpace;
        asteroidSpawner.transform.position = new Vector3(/*asteroidSpawner.transform.position.x*/0,
        asteroidSpawner.transform.position.y + AdditionalSpaceBetweenPlanets,
        asteroidSpawner.transform.position.z);
    }


    void SpawnBlackHole(Transform prevPlanet)
    {
        prevPlanet.GetComponent<PlanetController>().isPlanetWithBlackHole = true;
        GameObject blackHoleSpawner = Instantiate(blackHoleSpawnerObject);
        blackHoleSpawner.transform.position = prevPlanet.transform.position;
        AdditionalSpaceBetweenPlanets += blackHoleSpawnSpace;
        blackHoleSpawner.transform.position = new Vector3(/*blackHoleSpawner.transform.position.x*/0,
        blackHoleSpawner.transform.position.y + AdditionalSpaceBetweenPlanets,
        blackHoleSpawner.transform.position.z);
        prevPlanet.GetComponent<PlanetController>().blackHolePosition = blackHoleSpawner.
            GetComponent<BlackHoleSpawner>().blackHole.transform;

        int random = Random.Range(0, 10);

        if (random < 5)
        {
            blackHoleSpawner.GetComponent<BlackHoleSpawner>().isLeft = true;
            blackHoleSpawner.GetComponent<BlackHoleSpawner>().isRight = false;
        }
        else
        {
            blackHoleSpawner.GetComponent<BlackHoleSpawner>().isLeft = false;
            blackHoleSpawner.GetComponent<BlackHoleSpawner>().isRight = true;
        }

        blackHoleSpawner.GetComponent<BlackHoleSpawner>().SetPosition();


        if (blackHoleSpawner.GetComponent<BlackHoleSpawner>().isLeft)
            blackHolePosition = 1;
        else if (blackHoleSpawner.GetComponent<BlackHoleSpawner>().isRight)
            blackHolePosition = -1;
    }

    public void CheckDebrisChanceOutOfHundred(GameObject planetWithDebris)
    {
        int randomInt = Random.Range(0, 100);

        if(randomInt <= DebrisChanceOutofHundred)
        {
            planetWithDebris.GetComponent<PlanetController>().isPlanetWithDebris = true;
            planetWithDebris.GetComponent<PlanetController>().SetUpPlanetForOrbitingDebris();
            alreadyAddedObstacleThisRound = true;
        }

        //switch (horizontallyMovingChanceOutofHundred)
        //{
        //    case 0: return;
        //    case 10:
        //        if (randomInt <= 10)
        //        {
                    
        //            return;
        //        }
        //        break;
        //    case 20:
        //        if (randomInt <= 20)
        //        {
        //            planetWithDebris.GetComponent<PlanetController>().isPlanetWithDebris = true;
        //            planetWithDebris.GetComponent<PlanetController>().SetUpPlanetForOrbitingDebris();
        //            alreadyAddedObstacleThisRound = true;
        //            return;
        //        }
        //        break;
        //    case 50:
        //        if (randomInt <= 50)
        //        {
        //            planetWithDebris.GetComponent<PlanetController>().isPlanetWithDebris = true;
        //            planetWithDebris.GetComponent<PlanetController>().SetUpPlanetForOrbitingDebris();
        //            alreadyAddedObstacleThisRound = true;
        //            return;
        //        }
        //        break;
        //    case 100:
        //        if (randomInt <= 100)
        //        {
        //            planetWithDebris.GetComponent<PlanetController>().isPlanetWithDebris = true;
        //            planetWithDebris.GetComponent<PlanetController>().SetUpPlanetForOrbitingDebris();
        //            alreadyAddedObstacleThisRound = true;
        //            return;
        //        }
        //        break;

        //}
    }

    public void CheckStrongGravityChanceOutOfHundred(GameObject planetWithStrongGravity)
    {
        int randomInt = Random.Range(0, 100);
        switch (strongGravityChanceOutOfHundred)
        {
            case 0: return;
            case 10:
                if (randomInt <= 10)
                {
                    planetWithStrongGravity.GetComponent<PlanetController>().isPlanetWithStrongGravity = true;
                    planetWithStrongGravity.GetComponent<PlanetController>().SetUpPlanetForStrongGravity();
                    alreadyAddedObstacleThisRound = true;
                    return;
                }
                break;
            case 20:
                if (randomInt <= 20)
                {
                    planetWithStrongGravity.GetComponent<PlanetController>().isPlanetWithStrongGravity = true;
                    planetWithStrongGravity.GetComponent<PlanetController>().SetUpPlanetForStrongGravity();
                    alreadyAddedObstacleThisRound = true;
                    return;
                }
                break;
            case 50:
                if (randomInt <= 50)
                {
                    planetWithStrongGravity.GetComponent<PlanetController>().isPlanetWithStrongGravity = true;
                    planetWithStrongGravity.GetComponent<PlanetController>().SetUpPlanetForStrongGravity();
                    alreadyAddedObstacleThisRound = true;
                    return;
                }
                break;
            case 100:
                if (randomInt <= 100)
                {
                    planetWithStrongGravity.GetComponent<PlanetController>().isPlanetWithStrongGravity = true;
                    planetWithStrongGravity.GetComponent<PlanetController>().SetUpPlanetForStrongGravity();
                    alreadyAddedObstacleThisRound = true;
                    return;
                }
                break;

        }
    }

    //public void CheckHorizontallyMovingChanceOutOfHundred(GameObject planetToMove)
    //{
    //    int randomInt = Random.Range(0, 100);
    //    switch (horizontallyMovingChanceOutofHundred)
    //    {
    //        case 0: return;
    //        case 10:
    //            if (randomInt <= 10)
    //            {
    //                planetToMove.GetComponent<PlanetController>().isHorizontallyMoving = true;
    //                planetToMove.GetComponent <PlanetController>().SetUpPlanetsForHorizontallyMoving();
    //                alreadyAddedObstacleThisRound = true;
    //                return;
    //            }
    //            break;
    //        case 20:
    //            if (randomInt <= 20)
    //            {
    //                planetToMove.GetComponent<PlanetController>().isHorizontallyMoving = true;
    //                planetToMove.GetComponent<PlanetController>().SetUpPlanetsForHorizontallyMoving();
    //                alreadyAddedObstacleThisRound = true;
    //                return;
    //            }
    //            break;
    //        case 50:
    //            if (randomInt <= 50)
    //            {
    //                planetToMove.GetComponent<PlanetController>().isHorizontallyMoving = true;
    //                planetToMove.GetComponent<PlanetController>().SetUpPlanetsForHorizontallyMoving();
    //                alreadyAddedObstacleThisRound = true;
    //                return;
    //            }
    //            break;
    //        case 100:
    //            if (randomInt <= 100)
    //            {
    //                planetToMove.GetComponent<PlanetController>().isHorizontallyMoving = true;
    //                planetToMove.GetComponent<PlanetController>().SetUpPlanetsForHorizontallyMoving();
    //                alreadyAddedObstacleThisRound = true;
    //                return;
    //            }
    //            break;

    //    }
    //}

    public void CheckAsteroidChanceOutOfHundred(GameObject planetBeforeAsteroids, 
        GameObject planetAfterAsteroids)
    {   
        int randomInt = Random.Range(0, 100);

        if(randomInt <= asteroidChanceOutOfHundred)
        {
            SpawnAsteroidSpawner(planetBeforeAsteroids.transform);
            planetBeforeAsteroids.GetComponent<PlanetController>().SetUpPlanetForAsteroids();
            alreadyAddedObstacleThisRound = true;
        }

        //switch (asteroidChanceOutOfHundred)
        //{
        //    case 0: return;
        //    case 10: 
        //        if(randomInt <= 10)
        //        {
                    
        //            return;
        //        }
        //        break;
        //    case 20:
        //        if (randomInt <= 20)
        //        {
        //            SpawnAsteroidSpawner(planetBeforeAsteroids.transform);
        //            planetBeforeAsteroids.GetComponent<PlanetController>().SetUpPlanetForAsteroids();
        //            alreadyAddedObstacleThisRound = true;
        //            return;
        //        }
        //        break;
        //    case 50:
        //        if (randomInt <= 50)
        //        {
        //            SpawnAsteroidSpawner(planetBeforeAsteroids.transform);
        //            planetBeforeAsteroids.GetComponent<PlanetController>().SetUpPlanetForAsteroids();
        //            alreadyAddedObstacleThisRound = true;
        //            return;
        //        }
        //        break;
        //    case 100:
        //        if (randomInt <= 100)
        //        {
        //            SpawnAsteroidSpawner(planetBeforeAsteroids.transform);
        //            planetBeforeAsteroids.GetComponent<PlanetController>().SetUpPlanetForAsteroids();
        //            alreadyAddedObstacleThisRound = true;
        //            return;
        //        }
        //        break;

        //}

    }

    public void CheckBlackHoleChanceOutOfHundred(GameObject planetBeforeBlackHole,
        GameObject planetAfterBlackHole)
    {
        int randomInt = Random.Range(0, 100);

        if(randomInt <= blackHoleChanceOutOfHundred)
        {
            SpawnBlackHole(planetBeforeBlackHole.transform);
            planetBeforeBlackHole.GetComponent<PlanetController>().SetUpPlanetForBlackHole(blackHolePosition);
            alreadyAddedObstacleThisRound = true;
        }

        //switch (blackHoleChanceOutOfHundred)
        //{
        //    case 0: return;
        //    case 10:
        //        if (randomInt <= 10)
        //        {
                    
        //            return;
        //        }
        //        break;
        //    case 20:
        //        if (randomInt <= 20)
        //        {
        //            SpawnBlackHole(planetBeforeBlackHole.transform);
        //            planetBeforeBlackHole.GetComponent<PlanetController>().SetUpPlanetForBlackHole(blackHolePosition);
        //            alreadyAddedObstacleThisRound = true;
        //            return;
        //        }
        //        break;
        //    case 50:
        //        if (randomInt <= 50)
        //        {
        //            SpawnBlackHole(planetBeforeBlackHole.transform);
        //            planetBeforeBlackHole.GetComponent<PlanetController>().SetUpPlanetForBlackHole(blackHolePosition);
        //            alreadyAddedObstacleThisRound = true;
        //            return;
        //        }
        //        break;
        //    case 100:
        //        if (randomInt <= 100)
        //        {
        //            SpawnBlackHole(planetBeforeBlackHole.transform);
        //            planetBeforeBlackHole.GetComponent<PlanetController>().SetUpPlanetForBlackHole(blackHolePosition);
        //            alreadyAddedObstacleThisRound = true;
        //            return;
        //        }
        //        break;

        //}

    }



    public void AddAPlanet()
    {
        planetsAdded += 1;

        int randomIndex = Random.Range(0, planetsToPoolFrom.Length);

        GameObject newPlanet = Instantiate(planetsToPoolFrom[randomIndex]);

        newPlanet.transform.position = lastPlanetInScene.transform.position;

        lastPlanetInScene.GetComponent<PlanetController>().nextPlanet = newPlanet;
        newPlanet.GetComponent<PlanetController>().previousPlanet = lastPlanetInScene.gameObject;

        if (asteroid && !alreadyAddedObstacleThisRound)
        {
            CheckAsteroidChanceOutOfHundred(lastPlanetInScene.gameObject, newPlanet);
        }
        
        if(blackHole && !alreadyAddedObstacleThisRound)
        {
            CheckBlackHoleChanceOutOfHundred(lastPlanetInScene.gameObject, newPlanet);
        }

        //if(horizontallyMoving && !alreadyAddedObstacleThisRound)
        //{
        //    CheckHorizontallyMovingChanceOutOfHundred(lastPlanetInScene.gameObject);
        //}

        if (debris && !alreadyAddedObstacleThisRound) { 
            CheckDebrisChanceOutOfHundred(newPlanet.gameObject);
        }

        if(strongGravity && !alreadyAddedObstacleThisRound)
        {
            CheckStrongGravityChanceOutOfHundred (newPlanet.gameObject);
        }

        alreadyAddedObstacleThisRound = false;

        if (isVertical)
        {
            newPlanet.transform.position = new Vector3(newPlanet.transform.position.x,
            newPlanet.transform.position.y + newPlanet.GetComponent<PlanetController>().verticalDistance
            /*+AdditionalSpaceBetweenPlanets*/,
            newPlanet.transform.position.z);

            AdditionalSpaceBetweenPlanets = 0;
        }
        //else if (isHorizontal)
        //{
        //    newPlanet.transform.position = new Vector3(newPlanet.transform.position.x 
        //        + newPlanet.GetComponent<PlanetController>().verticalDistance,
        //    newPlanet.transform.position.y,
        //    newPlanet.transform.position.z);
        //}

        if(planetsAdded % 5 == 0)
        {
            if (asteroid && asteroidChanceOutOfHundred < 30)
            {
                asteroidChanceOutOfHundred += 2;
            }

            if (blackHole && blackHoleChanceOutOfHundred < 30)
            {
                blackHoleChanceOutOfHundred += 2;
            }

            if (debris && DebrisChanceOutofHundred < 30)
            {
                DebrisChanceOutofHundred += 2;
            }

            if (strongGravity && strongGravityChanceOutOfHundred < 30)
            {
                strongGravityChanceOutOfHundred += 2;
            }
        }

        
        if(powerUpsSpawned < maxPowerUpsToSpawn && GameManager.instance.currentPlanet > 10)
        {
            float random = Random.Range(0, 100);

            if(random < 25)
            {
                GameObject powerUp = Instantiate(powerUpObject);
                powerUp.transform.position = new Vector3(0,
                    newPlanet.transform.position.y + 5, newPlanet.transform.position.z);

                powerUpsSpawned++;
            }

            
        }

        


        //lastPlanetInScene.GetComponent<PlanetController>().placementCheckers[0].
        //   GetComponent<PlanetAlignmentCheckerController>().SetPlanetForShipCrash();

        lastPlanetInScene = newPlanet.transform;
    }

    IEnumerator DelayedSettingPlanetForShipCrash(GameObject newPlanet)
    {
        yield return new WaitForSecondsRealtime(2);
        
    }

    

    // Start is called before the first frame update
    void Start()
    {
        isVertical = GameManager.instance.isVertical;
        isHorizontal = GameManager.instance.isHorizontal; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
