using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UnityEngine.Events;

public class SpaceshipController : MonoBehaviour
{
    [HideInInspector]
    public float orbitalRotationSpeed = 20;
    public float axisRotationSpeed = 1, movementSpeed = 5;

    [HideInInspector]
    public bool canMoveOnTap = true, isOrbitingPlanet = false, gravitateTowardsPlanet = false, 
        isOrbitingPlanetWithBlackHole = false, moveTowardsBlackHoleOrPlanet = false, isMoving = false;


    [HideInInspector]
    public Transform planetOrbiting;
    [HideInInspector]
    public PlanetController planet;

    Rigidbody rb;

    [HideInInspector]
    public PlanetAlignmentCheckerController currentplanetAlignmentChecker;

    //public CinemachineVirtualCamera virtualCamera;

    public SpaceShipCamFollowController camController;

    public Vector3 leftRotation, rightRotation;

    public ParticleSystem shipExhaust, shipBooster;
    public GameObject shipTrail, normalTrail, perfectTrail;

    [HideInInspector]
    public Transform currentBlackHoleTarget, blackHoleTarget, planetTarget;

    public UnityEvent toDoOnTap;

    [HideInInspector]
    public ObjectRotator currentShipRotator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveTowardsBlackHoleOrPlanet)
        {
            //Debug.Log("Trying to move to blackhole target step 2");

            transform.position = Vector3.MoveTowards(transform.position, currentBlackHoleTarget.position,
                (movementSpeed/2) * Time.deltaTime);
        }

        if (isMoving)
        {
            //rb.velocity = Vector3.up * movementSpeed * Time.deltaTime;
            //transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
        }

        if (!isOrbitingPlanet || planetOrbiting == null) return;

        //transform.RotateAround(planetOrbiting.position,
        //    Vector3.forward * planet.rotationDirection, 
        //    planet.orbitalRotationSpeed * Time.deltaTime);

        if (isOrbitingPlanet)
        {
            Vector3 previousPosition = transform.position;

            transform.RotateAround(planetOrbiting.position,
            Vector3.forward * planet.rotationDirection,
            planet.orbitalRotationSpeed * Time.deltaTime);

            Vector3 tangent =(transform.position - previousPosition).normalized;

            Vector3 currentEulerAngles = transform.eulerAngles;

            float xRotation = Quaternion.LookRotation(tangent).eulerAngles.x;

            transform.rotation = Quaternion.Euler(xRotation, currentEulerAngles.y, currentEulerAngles.z);

            if (currentShipRotator != null) {
                currentShipRotator.canRotate = true;
            }
        }

        //Debug.Log(orbitalRotationSpeed);
        //transform.Rotate(Vector3.up, axisRotationSpeed);

        //if(planet != null)
        //{
        //    if(planet.isPlanetWithStrongGravity)
        //        transform.position += Vector3.MoveTowards(transform.position,
        //        planetOrbiting.position, planet.strongGravityPullStrength * Time.deltaTime);
        //}


        //transform.LookAt(new Vector3(transform.position.x, transform.position.y, transform.position.z));

        

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Planet")
        {
            //if(other.GetComponent<PlanetController>().isHorizontallyMoving)
                //transform.parent = other.transform;
            normalTrail.SetActive(false);
            perfectTrail.SetActive(false);
            GameManager.instance.currentPlanetGameObject = other.gameObject;
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.tag == "Planet")
    //    {
    //        virtualCamera.LookAt = transform;
    //        virtualCamera.Follow = transform;
    //    }
    //}

    public void OnScreenGameplayTap()
    {
        if (!canMoveOnTap) return;

        //Play sound effect
        GameManager.instance.gameplaySoundsManager.PlayerShipTapSound();

        rb.isKinematic = false;
        if(planet != null)
        {
            planet.GetComponent<Collider>().enabled = false;
            transform.parent = null;
            if (planet.GetComponent<PlanetController>().isPlanetWithBlackHole)
            {
                moveTowardsBlackHoleOrPlanet = true;
                planet.blackHoleAlignmentHolder.SetActive(false);
            }

            planet.GetComponent<PlanetController>().canSetCamSize = true;
        }
        
        if(GameManager.instance.isVertical && !isOrbitingPlanetWithBlackHole)
        {
            rb.velocity = Vector3.up * movementSpeed;
            //isMoving = true;
        }
            
        //else if(GameManager.instance.isHorizontal)
        //    rb.velocity = Vector3.right * movementSpeed;

        GameManager.instance.DestroyPlanet();
        GameManager.instance.AddPlanet();

        
        canMoveOnTap = false;
        isOrbitingPlanet = false;
        planetOrbiting = null;
        if(planet!= null)
            planet.GetComponent<PlanetController>().spaceShipController = null;
        planet = null;
        gravitateTowardsPlanet = false;
        //camController.RecalculateOffset();
        camController.canFollow = true;

        //GameManager.instance.switchPlanetImage.color = Color.red;

        toDoOnTap.Invoke();

        RotateShipVertically();
    }

    public void RotateShipVertically()
    {
        //DOTween.Kill(gameObject);

        DOTween.Clear();

        if (currentShipRotator != null)
        {
            currentShipRotator.canRotate = false;
        }
        //float animationSpeed = 10f / orbitalRotationSpeed;

        shipExhaust.Stop();
        //shipBooster.gameObject.SetActive(true);
        shipBooster.Clear();
        shipBooster.Play();
        shipTrail.SetActive(true);
        //shipTrail.SetActive(false);

        //shipTrail.SetActive(false);
        if(currentShipRotator!= null)
            currentShipRotator.transform.DOLocalRotate(Vector3.zero, 0f);

        transform.DOLocalRotate(new Vector3(-90, transform.localRotation.y, transform.localRotation.z),
            0.25f)
            .OnComplete(() => {

                shipBooster.Stop();
                shipBooster.gameObject.SetActive(false);
                //shipTrail.SetActive(true);

                Debug.Log("Vertical Alignment");
        });

        
    }

    public void RotateShipHorizontally(int direction)
    {
        //DOTween.Clear();

        //DOTween.Kill(gameObject);

        //float animationSpeed = 10f / orbitalRotationSpeed;

        //shipBooster.Stop();


        if (direction > 0) 
            transform.DOLocalRotate(leftRotation,
                0.25f)
                .OnComplete(() => {

                    shipBooster.Stop();
                    shipExhaust.Clear();
                    shipExhaust.Play();
                    shipTrail.SetActive(false);

                    Debug.Log("Horizontal Alignment");
                });
        else if (direction < 0)
            transform.DOLocalRotate(rightRotation,
                0.25f).OnComplete(() => {

                    shipBooster.Stop();
                    shipExhaust.Clear();
                    shipExhaust.Play();
                    shipTrail.SetActive(false);

                    Debug.Log("Horizontal Alignment");
                });
    }

    public void AlignWithOrbitRotator(Transform PlanetRotator)
    {
        transform.parent = PlanetRotator;
        transform.DOLocalRotate(Vector3.up, 0.5f);
        transform.parent = null;
    }
}
