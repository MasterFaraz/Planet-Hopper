using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetOrbiter2 : MonoBehaviour
{
    private Rigidbody rb;
    private Transform planetTransform;
    private bool isOrbiting = false;

    [Header("Orbit Settings")]
    [SerializeField] private float orbitSpeed = 30f;
    [SerializeField] private float sideToOrientToPlanet = -1f; // -1 for left side, 1 for right side

    [Header("Slingshot Settings")]
    [SerializeField] private float slingshotForce = 50f;
    [SerializeField] private float slingshotBoost = 1.5f;

    private float orbitAngle = 0f;
    public Vector3 orbitAxis = Vector3.up;
    private Vector3 orbitCenterToShip; // Vector from planet center to ship at entry
    private float actualOrbitDistance; // Distance calculated at entry point
    private Vector3 originalVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Planet") && !isOrbiting)
        {
            BeginOrbit(other.transform);
        }
    }

    private void BeginOrbit(Transform planet)
    {
        // Store initial state
        planetTransform = planet;
        originalVelocity = rb.velocity;
        isOrbiting = true;

        // Calculate the vector from planet to ship and store the distance
        orbitCenterToShip = transform.position - planetTransform.position;
        actualOrbitDistance = orbitCenterToShip.magnitude;

        // Calculate orbit plane
        Vector3 perpendicular = Vector3.Cross(orbitCenterToShip, rb.velocity);

        // If the ship is moving directly toward or away from the planet, choose an arbitrary perpendicular
        if (perpendicular.magnitude < 0.1f)
        {
            perpendicular = Vector3.Cross(orbitCenterToShip, Vector3.up);
            if (perpendicular.magnitude < 0.1f)
                perpendicular = Vector3.Cross(orbitCenterToShip, Vector3.right);
        }

        orbitAxis = perpendicular.normalized;

        // Calculate initial orbit angle
        Vector3 flattenedPosition = Vector3.ProjectOnPlane(orbitCenterToShip, orbitAxis);
        orbitAngle = Mathf.Atan2(Vector3.Dot(Vector3.Cross(Vector3.forward, flattenedPosition), orbitAxis),
                                Vector3.Dot(Vector3.forward, flattenedPosition)) * Mathf.Rad2Deg;

        // Immediately orient the ship to have its side facing the planet
        OrientShipToPlanet();

        // Disable physics while orbiting
        rb.isKinematic = true;
    }

    private void OrientShipToPlanet()
    {
        // Calculate direction to the planet
        Vector3 toPlanet = planetTransform.position - transform.position;
        Vector3 normalizedToPlanet = toPlanet.normalized;

        // Calculate the forward direction - perpendicular to both axis and toPlanet
        Vector3 forwardDir = Vector3.Cross(orbitAxis, normalizedToPlanet) * sideToOrientToPlanet;

        // Apply the rotation to have the side face the planet
        transform.rotation = Quaternion.LookRotation(forwardDir, orbitAxis);
    }

    private void Update()
    {
        if (!isOrbiting || planetTransform == null) return;

        // Update the orbit angle
        orbitAngle += orbitSpeed * Time.deltaTime;

        // Calculate the new position while maintaining the original distance
        Quaternion rotation = Quaternion.AngleAxis(orbitSpeed * Time.deltaTime, orbitAxis);
        orbitCenterToShip = rotation * orbitCenterToShip;

        // Set the new position
        transform.position = planetTransform.position + orbitCenterToShip;

        // Orient the ship to have its side face the planet
        OrientShipToPlanet();
    }

    // Call this function to perform the slingshot maneuver
    public void Slingshot()
    {
        //if (!isOrbiting) return;

        // Re-enable physics
        rb.isKinematic = false;

        // Get the current orbital velocity direction
        Vector3 orbitDirection = transform.forward;

        // Apply force in the forward direction of the ship
        float speedBoost = originalVelocity.magnitude * slingshotBoost;
        rb.velocity = orbitDirection * (slingshotForce + speedBoost);

        // Add some rotation to simulate breaking free
        rb.AddTorque(Random.insideUnitSphere * slingshotForce * 0.1f, ForceMode.Impulse);

        // Reset orbiting state
        isOrbiting = false;
        planetTransform = null;
    }

    // Optional method to adjust orbit speed at runtime
    public void AdjustOrbitSpeed(float newSpeed)
    {
        orbitSpeed = newSpeed;
    }
}
