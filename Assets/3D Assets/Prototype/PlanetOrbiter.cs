using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlanetOrbiter : MonoBehaviour
{
    private Rigidbody rb;
    private Transform planetTransform;
    private bool isOrbiting = false;

    [Header("Orbit Settings")]
    [SerializeField] private float orbitSpeed = 30f;
    [SerializeField] private float orbitDistance = 10f;
    [SerializeField] private float transitionSpeed = 2f;
    [SerializeField] private float sideToOrientToPlanet = -1f; // -1 for left side, 1 for right side

    [Header("Slingshot Settings")]
    [SerializeField] private float slingshotForce = 50f;
    [SerializeField] private float slingshotBoost = 1.5f;

    private float orbitAngle = 0f;
    private Vector3 orbitAxis = Vector3.up;
    private float transitionProgress = 0f;
    private Vector3 entryPosition;
    private Quaternion entryRotation;
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
        entryPosition = transform.position;
        entryRotation = transform.rotation;
        originalVelocity = rb.velocity;
        isOrbiting = true;
        transitionProgress = 0f;

        // Calculate orbit plane
        Vector3 toPlanet = planetTransform.position - transform.position;
        Vector3 perpendicular = Vector3.Cross(toPlanet, rb.velocity);

        // If the ship is moving directly toward or away from the planet, choose an arbitrary perpendicular
        if (perpendicular.magnitude < 0.1f)
        {
            perpendicular = Vector3.Cross(toPlanet, Vector3.up);
            if (perpendicular.magnitude < 0.1f)
                perpendicular = Vector3.Cross(toPlanet, Vector3.right);
        }

        orbitAxis = perpendicular.normalized;

        // Calculate initial orbit angle
        Vector3 initialOrbitPos = transform.position - planetTransform.position;
        float initialAngleRad = Mathf.Atan2(initialOrbitPos.z, initialOrbitPos.x);
        orbitAngle = initialAngleRad * Mathf.Rad2Deg;

        // Disable physics while orbiting
        rb.isKinematic = true;
    }

    private void Update()
    {
        if (!isOrbiting || planetTransform == null) return;

        // Transition smoothly into orbit
        if (transitionProgress < 1f)
        {
            transitionProgress += Time.deltaTime * transitionSpeed;
            transitionProgress = Mathf.Clamp01(transitionProgress);

            // Calculate the target orbit position
            Vector3 toPlanet = planetTransform.position - entryPosition;
            Vector3 normalizedToPlanet = toPlanet.normalized;
            Vector3 targetPosition = planetTransform.position - normalizedToPlanet * orbitDistance;

            // Interpolate position
            transform.position = Vector3.Lerp(entryPosition, targetPosition, transitionProgress);

            // Update direction to planet for orientation
            toPlanet = planetTransform.position - transform.position;
            normalizedToPlanet = toPlanet.normalized;

            // Calculate orientation - the ship's side should face the planet
            Vector3 forwardDir = Vector3.Cross(orbitAxis, normalizedToPlanet) * sideToOrientToPlanet;
            Quaternion targetRotation = Quaternion.LookRotation(forwardDir, orbitAxis);

            // Interpolate rotation
            transform.rotation = Quaternion.Slerp(entryRotation, targetRotation, transitionProgress);
        }
        else
        {
            // Regular orbit after transition
            PerformOrbit();
        }
    }

    private void PerformOrbit()
    {
        // Update the orbit angle
        orbitAngle += orbitSpeed * Time.deltaTime;

        // Calculate position in orbit
        float x = Mathf.Cos(orbitAngle * Mathf.Deg2Rad) * orbitDistance;
        float z = Mathf.Sin(orbitAngle * Mathf.Deg2Rad) * orbitDistance;

        // Create the position in the correct orbit plane
        Vector3 orbitPosition = new Vector3(x, 0, z);

        // Reorient the orbit position to match the orbit axis
        Quaternion orbitRotation = Quaternion.FromToRotation(Vector3.up, orbitAxis);
        orbitPosition = orbitRotation * orbitPosition;

        // Set final position
        transform.position = planetTransform.position + orbitPosition;

        // Calculate direction to the planet
        Vector3 toPlanet = planetTransform.position - transform.position;
        Vector3 normalizedToPlanet = toPlanet.normalized;

        // Calculate the forward direction - perpendicular to both up and toPlanet
        Vector3 forwardDir = Vector3.Cross(orbitAxis, normalizedToPlanet) * sideToOrientToPlanet;

        // Apply the rotation to have the side face the planet
        transform.rotation = Quaternion.LookRotation(forwardDir, orbitAxis);
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

    // Optional method to adjust orbit parameters at runtime
    public void AdjustOrbit(float newSpeed, float newDistance)
    {
        orbitSpeed = newSpeed;
        orbitDistance = newDistance;
    }
}