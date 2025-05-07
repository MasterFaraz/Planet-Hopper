using UnityEngine;

public class SimpleOrbit : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform targetObject;
    [SerializeField] private Vector3 rotationAxis = Vector3.up;

    [Header("Orbit Settings")]
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private float orbitDistance = 5f;
    [SerializeField] private float heightOffset = 0f;

    private Vector3 startingPosition;
    private float currentAngle = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Planet")
        {
            targetObject = other.transform;
        }
    }

    private void Start()
    {
        if (targetObject == null)
        {
            Debug.LogError("No target object assigned to orbit around!");
            enabled = false;
            return;
        }

        // Store the initial relative position to the target
        startingPosition = transform.position - targetObject.position;

        // Calculate the initial angle
        currentAngle = Mathf.Atan2(startingPosition.z, startingPosition.x) * Mathf.Rad2Deg;

        // Ensure we're at the proper orbit distance
        Vector3 horizontalPosition = new Vector3(startingPosition.x, 0, startingPosition.z).normalized * orbitDistance;
        transform.position = targetObject.position + horizontalPosition + new Vector3(0, heightOffset, 0);
    }

    private void Update()
    {
        if (targetObject == null) return;

        // Update the angle based on rotation speed
        currentAngle += rotationSpeed * Time.deltaTime;

        // Calculate the new position around the target
        float x = Mathf.Cos(currentAngle * Mathf.Deg2Rad) * orbitDistance;
        float z = Mathf.Sin(currentAngle * Mathf.Deg2Rad) * orbitDistance;

        // Update position to orbit around the target
        Vector3 newPosition = targetObject.position + new Vector3(x, heightOffset, z);
        transform.position = newPosition;

        // Optionally make the object face the direction of travel
        if (rotationSpeed != 0)
        {
            Vector3 tangent = new Vector3(-z, 0, x).normalized;
            transform.forward = rotationSpeed > 0 ? tangent : -tangent;
        }
    }

    // Public method to change orbit parameters at runtime
    public void SetOrbitParameters(float newSpeed, float newDistance, float newHeight)
    {
        rotationSpeed = newSpeed;
        orbitDistance = newDistance;
        heightOffset = newHeight;
    }
}