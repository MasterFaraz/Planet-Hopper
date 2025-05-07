using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetOrbitingDebrisController : MonoBehaviour
{
    public GameObject[] debrisObjects;
    public Transform planetToRotateAround;
    public float rotationSpeed;
    PlanetController planetController;

    // Start is called before the first frame update
    void Start()
    {
        planetToRotateAround = GetComponentInParent<PlanetController>().transform;
        planetController = planetToRotateAround.GetComponent<PlanetController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(planetToRotateAround.position, Vector3.forward * planetController.rotationDirection,
                GameManager.instance.planetOrbitingSpeed * Time.deltaTime);

        //foreach (var debris in debrisObjects)
        //{
        //    debris.transform.RotateAround(planetToRotateAround.position, Vector3.forward * planetController.rotationDirection,
        //        rotationSpeed * Time.timeScale);
        //}
    }
}
