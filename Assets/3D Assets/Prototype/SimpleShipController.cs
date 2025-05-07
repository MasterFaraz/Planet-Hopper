using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
//using Unity.VisualScripting;
using UnityEngine;

public class SimpleShipController : MonoBehaviour
{
    public Transform targetToOrbitAround;
    public float rotationSpeed = 20f;
    public Vector3 shipAngle = new Vector3(0, 90, 0);
    public float selfRotationSpeed = 2f;
    bool canRotate = false;


    Rigidbody rb;



    //[Header("Orbit Settings")]
    //[SerializeField] private Transform targetToOrbitAround;
    //[SerializeField] private Vector3 rotationAxis = Vector3.up;
    ////[SerializeField] private float rotationSpeed = 30f;

    //[Header("Angle Settings")]
    //[SerializeField] private Vector3 fixedAngle = new Vector3(0, 90, 0);
    //[SerializeField] private bool maintainUpDirection = true;
    //[SerializeField] private Vector3 upDirection = Vector3.up;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void StartRotating()
    {
        canRotate = true;
    }

    void ApplyPerpendicularRotation()
    {
        Vector3 directionFromPlanet = transform.position - targetToOrbitAround.position;

        Vector3 orbitDirection = Vector3.Cross(Vector3.left, directionFromPlanet).normalized;

        transform.rotation = Quaternion.Euler(orbitDirection);
    }


    // Update is called once per frame
    void Update()
    {
        if(canRotate)
        {
            rb.isKinematic = true;

            //float distance = Vector3.Distance(transform.position, targetToOrbitAround.position);

            transform.RotateAround(targetToOrbitAround.position, -Vector3.forward, rotationSpeed * Time.deltaTime);

            //ApplyPerpendicularRotation();

            //Vector3 directionToTarget = targetToOrbitAround.position - transform.position;
            //Vector3 directionToTransform = transform.position - targetToOrbitAround.position;
            //float angle = Vector3.Angle(directionToTarget, directionToTransform);

            //Debug.Log(angle);

            //Vector3 currentRotation = new Vector3(transform.rotation.x,transform.rotation.y, transform.rotation.z);

            transform.Rotate(shipAngle*rotationSpeed);

            //transform.rotation = Quaternion.Euler();

            //Vector3 directionToTarget = transform.position - targetToOrbitAround.position;
            //transform.position = targetToOrbitAround.position + directionToTarget.normalized * distance;

            //// Apply the fixed angle
            //transform.rotation = Quaternion.Euler(fixedAngle);

            //// If maintaining up direction
            //if (maintainUpDirection)
            //{
            //    // Make sure the specified up direction is pointing in the correct direction while preserving the fixed angle
            //    Vector3 dirToTarget = targetToOrbitAround.position - transform.position;
            //    Quaternion lookRot = Quaternion.LookRotation(transform.forward, upDirection);
            //    transform.rotation = Quaternion.Euler(fixedAngle.x, fixedAngle.y, fixedAngle.z) *
            //                         Quaternion.Euler(0, 0, lookRot.eulerAngles.z);
            //}


        }

    }


}
