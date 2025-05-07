using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;
using UnityEngine.Events;

public class PlanetController : MonoBehaviour
{
    //public Vector3 camDistanceOffset = new Vector3(5,5,-10);
    public int rotationDirection = 1;
    [HideInInspector]
    public float orbitalRotationSpeed = 20;
    public float obritingSpeedMultiplier = 1, oscillationSpeedMultiplier = 1;

    public float verticalDistance = 5;
    public float horizontalDistance = 5;

    public bool planetDone = false;

    public float camSizeForOrbitOffset = 5f;
    float camSizeForOrbit, originalCamSize;
    float transitionSmoothValue = 2.5f;
    [HideInInspector]
    public bool canSetCamSize = false;

    //public Transform spaceShipLookAtObject;

    public PlanetAlignmentCheckerController planetPerfectAlignmentChecker;
    //public GameObject planetMissDetector;

    bool isSpiral = false;
    public SpaceshipController spaceShipController;

    bool movePlanetToCenter = false;
    public float centerMoveSpeed = 3;

    public UnityEvent toDoOnPlayerEnter;

    public float maxStallingTime = 8f;
    float currentStalledTime = 0;

    public GameObject planetsHolder, blackHole;

    public float shipOrbitPulsateDistance = 2f;

    [Header("--------------------------Obstacles---------------------------")]
    public bool isPlanetWithAsteroids = false;
    public bool isPlanetWithDebris = false;
    public bool isPlanetWithStrongGravity = false;
    public bool isPlanetWithBlackHole = false;
    public float horizontalOscilationRangeMin, horizontalOscilationRangeMax, horizontalOscilationSpeed = 3f;
    bool isMovingRight = true, isMovingLeft = false;
    bool makeIndicatorOrangeOnPlayerEnter = false;
    public GameObject nextPlanet, previousPlanet;
    public GameObject orbitingDebris;
    public float strongGravityPullStrength;
    public Transform startPositionInCaseOfBlackHole, nextPlanetStartPosition, blackHolePosition;
    public GameObject blackHoleAlignmentHolder;
    public BlackHoleAlignmentChecker blackHoleLeftChecker, blackHoleRightChecker;
    

    public void DestroyAllPlacementCheckers()
    {
        //{
        //    foreach (var placementChecker in placementCheckers)
        //    {
        //        if(placementChecker != null)
        //            Destroy(placementChecker.gameObject);
        //    }
        //}
        //SetPlanetForOrbit();
    }

    public void SetOrangeIndication()
    {
        makeIndicatorOrangeOnPlayerEnter = true;
    }

    //public void SetUpPlanetsForHorizontallyMoving()
    //{
    //    SetOrangeIndication();
    //    DestroyAllPlacementCheckers();
    //    if (previousPlanet != null)
    //    {
    //        previousPlanet.GetComponent<PlanetController>().DestroyAllPlacementCheckers();
    //        previousPlanet.GetComponent<PlanetController>().SetOrangeIndication();
    //    }
    //    //if (nextPlanet != null)
    //    //    nextPlanet.GetComponent<PlanetController>().DestroyAllPlacementCheckers();
    //}

    public void SetUpPlanetForAsteroids()
    {
        isPlanetWithAsteroids = true;
        SetOrangeIndication();
        DestroyAllPlacementCheckers();
        //if (nextPlanet != null)
        //{
        //    nextPlanet.GetComponent<PlanetController>().DestroyAllPlacementCheckers();
        //    //nextPlanet.GetComponent<PlanetController>().SetOrangeIndication();
        //}
        //if (nextPlanet != null)
        //    nextPlanet.GetComponent<PlanetController>().DestroyAllPlacementCheckers();
    }

    public void SetUpPlanetForOrbitingDebris()
    {
        if(orbitingDebris != null)
            orbitingDebris.SetActive(true);

        if (previousPlanet != null)
        {
            previousPlanet.GetComponent<PlanetController>().DestroyAllPlacementCheckers();
            previousPlanet.GetComponent<PlanetController>().SetOrangeIndication();
        }
    }

    public void SetUpPlanetForBlackHole(int LeftOrRight)
    {
        blackHoleAlignmentHolder.SetActive(true);

        if(LeftOrRight < 0)
        {
            Debug.Log("attempted to set left");
            blackHoleLeftChecker.SetPlanet();
            blackHoleRightChecker.SetBlackHole();
        }
        else if(LeftOrRight > 0)
        {
            blackHoleLeftChecker.SetBlackHole();
            blackHoleRightChecker.SetPlanet();
        }

        if(nextPlanet != null)
        {
            nextPlanetStartPosition = nextPlanet.GetComponent<PlanetController>().startPositionInCaseOfBlackHole;
        }

        DestroyAllPlacementCheckers();
        SetOrangeIndication();
    }

    public void SetUpPlanetForStrongGravity()
    {
        isSpiral = true;
    }


    // Start is called before the first frame update
    void Awake()
    {
        originalCamSize = Camera.main.orthographicSize;

        camSizeForOrbit = GetComponent<SphereCollider>().radius + camSizeForOrbitOffset;

        //if (isHorizontallyMoving)
        //{
        //    SetUpPlanetsForHorizontallyMoving();
        //}
        if (isPlanetWithDebris)
        {
            SetUpPlanetForOrbitingDebris();
        }
        if (isPlanetWithAsteroids) {
            SetUpPlanetForAsteroids();
        }
            

        if (isPlanetWithStrongGravity) SetUpPlanetForStrongGravity();

        if(isPlanetWithBlackHole)
        {
            //SetUpPlanetForBlackHole(1);       
        }
        //placementCheckers = GetComponentsInChildren<PlanetAlignmentCheckerController>();
    }

    public void SetCameSize()
    {
        canSetCamSize = true;
    }

    [HideInInspector]
    public bool alreadyGameOverCalled = false;

    bool gameOverSoundAlreadyPlayed = false;

    // Update is called once per frame
    void Update()
    {

        if (spaceShipController != null) {

            currentStalledTime += Time.deltaTime;

            if(currentStalledTime >= maxStallingTime)
            {
                GameManager.instance.driftedAway = true;
                
                if(!gameOverSoundAlreadyPlayed)
                {
                    GameManager.instance.gameplaySoundsManager.PlayShipDriftingAwaySound();
                    gameOverSoundAlreadyPlayed = true;
                }
                

                isSpiral = true;
                planetsHolder.SetActive(false);
                blackHole.SetActive(true);

                spaceShipController.canMoveOnTap = false;

                obritingSpeedMultiplier = 3.5f;
            }
        
        }




        //if (planetDone) return;

        if (!planetDone && !alreadyGameOverCalled)
        {
            if(GameManager.instance.player.transform.position.y > transform.position.y + 3f)
            {
                if(GameManager.instance.reviveChance > 0 || GameManager.instance.freeRevive > 0)
                {
                    GameManager.instance.ShipDriftingAway();
                    GameManager.instance.currentPlanetGameObject = this.gameObject;
                    alreadyGameOverCalled = true;
                }
                else GameManager.instance.GameOver();

                
            }

            if(Vector3.Distance(GameManager.instance.player.transform.position, transform.position) 
                <= shipOrbitPulsateDistance)
            {
                //Debug.Log(Vector3.Distance(GameManager.instance.player.transform.position, transform.position));
                planetsHolder.GetComponent<PlanetRandomizer>().orbitRing.GetComponent<DOTweenAnimation>().DOPlay();
            }
        }

        orbitalRotationSpeed = GameManager.instance.planetOrbitingSpeed * obritingSpeedMultiplier;
        horizontalOscilationSpeed = GameManager.instance.planetOscillationSpeed * oscillationSpeedMultiplier;

        if (!movePlanetToCenter) {

            if (isMovingRight)
            {
                if (transform.position.x <= horizontalOscilationRangeMax)
                    transform.position += Vector3.right * horizontalOscilationSpeed * Time.deltaTime;
                else
                {
                    isMovingLeft = true;
                    isMovingRight = false;
                }

            }
            else if (isMovingLeft)
            {
                if (transform.position.x > horizontalOscilationRangeMin)
                    transform.position -= Vector3.right * horizontalOscilationSpeed * Time.deltaTime;
                else
                {
                    isMovingLeft = false;
                    isMovingRight = true;
                }


            }

        }
        else if (movePlanetToCenter && transform.position.x != 0) {
        
            transform.position = Vector3.MoveTowards(transform.position, 
                new Vector3(0, transform.position.y, transform.position.z), centerMoveSpeed * Time.deltaTime);
        }


        if (!canSetCamSize) return;
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize,
            camSizeForOrbit, transitionSmoothValue * Time.deltaTime);



        if (isSpiral)
        {
            //Debug.Log(isSpiral);
            if (spaceShipController != null)
            {
                //Debug.Log(spaceShipController.name);
                spaceShipController.transform.position = Vector3.MoveTowards(spaceShipController.transform.position,
                    transform.position, strongGravityPullStrength * Time.deltaTime);
            }
        }

        //if (spaceShipLookAtObject != null)
        //    spaceShipLookAtObject.RotateAround(transform.position,
        //        Vector3.forward * rotationDirection, orbitalRotationSpeed * Time.deltaTime);
    }


   
    float PositionXFromTransform(Transform otherObject)
    {
        float difference = transform.position.x - otherObject.position.x;
        return difference;
    }

    float PositionYFromTransform(Transform otherObject)
    {
        float difference = transform.position.y - otherObject.position.y;
        return difference;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            spaceShipController = other.gameObject.GetComponent<SpaceshipController>();
            if (!isPlanetWithDebris) {
                if(previousPlanet != null)
                {
                    if (previousPlanet.GetComponent<PlanetController>().rotationDirection == 1)
                    {
                        rotationDirection = -1;
                    }
                    else rotationDirection = 1;
                }
                //if (PositionXFromTransform(spaceShipController.transform) >= 0f) rotationDirection = -1;
                //else rotationDirection = 1;
            }
            if (isPlanetWithBlackHole)
            {
                spaceShipController.blackHoleTarget = blackHolePosition;
                spaceShipController.planetTarget = nextPlanetStartPosition;
                //spaceShipController.isOrbitingPlanetWithBlackHole = true;
            }
            
            spaceShipController.moveTowardsBlackHoleOrPlanet = false;
            spaceShipController.isOrbitingPlanetWithBlackHole = false;
            

            if (isPlanetWithAsteroids)
            {
                //nextPlanet.GetComponent<PlanetController>().SetPlanetForOrbit();
            }

            //if (placementCheckers[0] != null) { 
            //    if(nextPlanet != null)
            //    {
            //        nextPlanet.GetComponent<PlanetController>().SetPlanetForShipCrash();
            //    }
            //}
            
            Debug.Log("Planet Entered");
            spaceShipController.planet = this;
            if (planetDone) return;

            

            currentStalledTime = 0;

            if(GameManager.instance.gameplayUIManager.isFirstLaunch && GameManager.instance.currentPlanet < 3)
                GameManager.instance.gameplayUIManager.ShowTapToSwitchPlanetTextObject();

            spaceShipController.isMoving = false;
            movePlanetToCenter = true;
            spaceShipController.transform.parent = transform;
            planetPerfectAlignmentChecker.gameObject.SetActive(false);
            SetCameSize();
            spaceShipController.orbitalRotationSpeed = orbitalRotationSpeed;
            spaceShipController.planetOrbiting = transform;
            spaceShipController.isOrbitingPlanet = true;
            spaceShipController.canMoveOnTap = true;
            //spaceShipController.GetComponent<Rigidbody>().isKinematic = true;
            spaceShipController.GetComponent<Rigidbody>().velocity = Vector3.zero;
            planetDone = true;
            if(!planetPerfectAlignmentChecker.GetComponent<PlanetAlignmentCheckerController>().alreadyScoreAdded)
            {
                //Playsound effect
                GameManager.instance.gameplaySoundsManager.PlayerEnterOrbitSound();
                GameManager.instance.gameplayScoreManager.AddScore();

            }


            spaceShipController.RotateShipHorizontally(rotationDirection);

            if (makeIndicatorOrangeOnPlayerEnter)
            {
                //GameManager.instance.switchPlanetImage.color = Color.yellow;
                //spaceShipController.transform.parent = transform;
            }
            else
                //GameManager.instance.switchPlanetImage.color = Color.red;

            GetComponent<Collider>().enabled = false;

            toDoOnPlayerEnter.Invoke();
            //GameManager.instance.OrbitGlowMaterialEnter();
        }
            
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //GameManager.instance.switchPlanetImage.color = Color.red;

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //if(isHorizontallyMoving) GameManager.instance.switchPlanetImage.color= Color.yellow;
            //GameManager.instance.RedLightSwitching();
        }
    }
}
