using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipOrbitController : MonoBehaviour
{
    private Rigidbody rb;
    private Transform planetTransform;
    private bool isOrbiting = false;
    bool isFirstTime = true;

    [Header("Orbit Settings")]
    [SerializeField] private float orbitSpeed = 30f;
    [SerializeField] private float orbitHeight = 10f;
    [SerializeField] private float orbitEntrySpeed = 5f;
    [SerializeField] private float stabilizationTime = 1.5f;
    [SerializeField] private Vector3 orbitNormal;

    private float orbitProgress = 0f;
    
    private float stabilizationTimer = 0f;
    private Vector3 originalVelocity;

    private void Awake()
    {
        isFirstTime = true;
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Planet") && !isOrbiting)
        {
            StartOrbit(other.transform);
        }
    }

    private void StartOrbit(Transform planet)
    {
        planetTransform = planet;
        isOrbiting = true;

        // Store the ship's original velocity for blending
        originalVelocity = rb.velocity;

        // Calculate the orbit normal (perpendicular to current position relative to planet and velocity)
        Vector3 relativePosition = transform.position - planetTransform.position;
        orbitNormal = Vector3.Cross(relativePosition.normalized, rb.velocity.normalized).normalized;

        // If the cross product is near zero (ship is moving directly toward/away from planet)
        // use an arbitrary perpendicular vector
        if (orbitNormal.magnitude < 0.1f)
        {
            orbitNormal = Vector3.Cross(relativePosition, Vector3.up).normalized;
            // If still problematic, try another direction
            if (orbitNormal.magnitude < 0.1f)
                orbitNormal = Vector3.Cross(relativePosition, Vector3.right).normalized;
        }

        stabilizationTimer = 0f;
    }

    private void FixedUpdate()
    {
        if (!isOrbiting || planetTransform == null) return;

        // Handle the transition into orbit
        if (stabilizationTimer < stabilizationTime)
        {
            stabilizationTimer += Time.fixedDeltaTime;
            float t = stabilizationTimer / stabilizationTime;

            // Get current position relative to planet
            Vector3 relativePosition = transform.position - planetTransform.position;
            float currentDistance = relativePosition.magnitude;

            // Calculate target orbit position
            float targetDistance = planetTransform.localScale.x / 2f + orbitHeight;
            Vector3 normalizedPosition = relativePosition.normalized;
            Vector3 targetPosition = planetTransform.position + normalizedPosition * targetDistance;

            // Move toward the orbit path
            transform.position = Vector3.Lerp(transform.position, targetPosition, t * orbitEntrySpeed * Time.fixedDeltaTime);

            // Gradually reduce original velocity and increase orbital velocity
            rb.velocity = Vector3.Lerp(originalVelocity, Vector3.zero, t);

            // Orient the ship to face the direction of travel
            if (rb.velocity.magnitude > 0.1f)
            {
                transform.forward = rb.velocity.normalized;
            }
        }
        else
        {
            // Regular orbit after stabilization
            PerformOrbit();
        }
    }

    private void PerformOrbit()
    {
        // Update orbit progress
        orbitProgress += orbitSpeed * Time.fixedDeltaTime;

        // Get current position relative to planet
        Vector3 relativePosition = transform.position - planetTransform.position;

        // Calculate target distance from planet center
        float targetDistance = planetTransform.localScale.x / 2f + orbitHeight;

        // Calculate the orbit axis (perpendicular to orbit normal and current relative position)
        Vector3 orbitAxis = Vector3.Cross(orbitNormal, relativePosition.normalized);

        // Rotate the relative position around the orbit normal
        Quaternion rotation = Quaternion.AngleAxis(orbitSpeed * Time.fixedDeltaTime, orbitNormal);
        Vector3 newRelativePosition = rotation * relativePosition.normalized * targetDistance;

        // Calculate new position and set it
        Vector3 newPosition = planetTransform.position + newRelativePosition;
        rb.MovePosition(newPosition);

        // Calculate the direction of movement for the ship to face
        Vector3 orbitDirection = Vector3.Cross(orbitNormal, newRelativePosition.normalized);

        // Make the ship face the direction of orbit
        transform.forward = orbitDirection;
    }

    // Optional: Add a way to exit orbit
    public void ExitOrbit()
    {
        if (isOrbiting)
        {
            isOrbiting = false;
            // Add an exit velocity tangential to the orbit
            rb.velocity = transform.forward * orbitSpeed;
        }
        else if (isFirstTime)
        {
            isOrbiting = false;
            // Add an exit velocity tangential to the orbit
            rb.velocity = transform.forward * orbitSpeed;
            isFirstTime = false;
        }
    }
}
