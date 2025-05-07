using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ShipPrototypeController : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;

    Rigidbody shipRigidBody;
    public float movementSpeed = 1, rotationSpeed = 2;

    bool hasStarted = false;
    bool canRotateAroundOrbit = false;
    public bool canMoveForwardOnTap = false;
    Transform currentPlanet;

    //[Header("Target Settings")]
    //[SerializeField] private Transform targetObject;
    //[SerializeField] private Vector3 rotationAxis = Vector3.up;

    //[Header("Orbit Settings")]
    //[SerializeField] private float rotationSpeed = 20f;
    //[SerializeField] private float orbitDistance = 5f;
    //[SerializeField] private float heightOffset = 0f;

    //private Vector3 startingPosition;
    //private float currentAngle = 0f;

    public void OnScreenTap()
    {   if (!canMoveForwardOnTap) return;
        shipRigidBody.velocity = transform.forward * movementSpeed;
        hasStarted = true;
        //canMoveForwardOnTap = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Planet")
        {
            RotateAroundPlanet(other.transform);
        }
    }

    public void RotateAroundPlanet(Transform planetTransform)
    {
        virtualCamera.Follow = planetTransform;
        virtualCamera.LookAt = planetTransform;
        //canRotateAroundOrbit = true;
        //currentPlanet = planetTransform;
        //Debug.Log(planetTransform.name + "entered");
    }

    

    // Start is called before the first frame update
    void Start()
    {
        shipRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canRotateAroundOrbit) return;

        if (currentPlanet == null) return;

        shipRigidBody.isKinematic = !canRotateAroundOrbit;

        //Debug.Log("Update working!");

        //shipRigidBody.velocity = Vector3.zero;
        transform.RotateAround(currentPlanet.position, Vector3.up, rotationSpeed * Time.deltaTime);

        //if (targetObject == null) return;

        //canMoveForwardOnTap = true;

        //// Update the angle based on rotation speed
        //currentAngle += rotationSpeed * Time.deltaTime;

        //// Calculate the new position around the target
        //float x = Mathf.Cos(currentAngle * Mathf.Deg2Rad) * orbitDistance;
        //float z = Mathf.Sin(currentAngle * Mathf.Deg2Rad) * orbitDistance;

        //// Update position to orbit around the target
        //Vector3 newPosition = targetObject.position + new Vector3(x, heightOffset, z);
        //transform.position = newPosition;

        //// Optionally make the object face the direction of travel
        //if (rotationSpeed != 0)
        //{
        //    Vector3 tangent = new Vector3(-z, 0, x).normalized;
        //    transform.forward = rotationSpeed > 0 ? tangent : -tangent;
        //}



    }
}
