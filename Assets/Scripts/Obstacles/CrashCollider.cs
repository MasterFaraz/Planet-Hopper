using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashCollider : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public float movementSpeedIfAsteroid = 5f;

    int rotationDir = 1;

    public bool isAsteroid = false;
    public bool isDebris = false, doNotRotate = false;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.SetActive(false);
            GameManager.instance.GameOver();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        if(!isAsteroid && !doNotRotate)
            rotationDir = GetComponentInParent<PlanetController>().rotationDirection;
    }

    // Update is called once per frame
    void Update()
    {
        if(!doNotRotate)
            transform.Rotate(rotationDir*Vector3.forward, rotationSpeed * Time.deltaTime);
        if(isAsteroid)
            transform.position += transform.forward * movementSpeedIfAsteroid * Time.deltaTime;
    }
}
