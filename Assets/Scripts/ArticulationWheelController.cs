using UnityEngine;

/// <summary>
/// Applies omnidirectional force to a robot with ArticulationBody
/// </summary>
[RequireComponent(typeof(ArticulationBody))]
public class ArticulationWheelController : MonoBehaviour
{
    public ArticulationBody robotBody;
    
    // NOTE: Force/Torque Multipliers will need significant tuning
    // These are the ONLY two public variables remaining for tuning the robot's movement.
    
    public float forceMultiplier = 1250*50f; 
    public float torqueMultiplier = 50*20f;

    private Vector3 rawTargetVelocity; // Stores raw input axes (x, 0, z)
    private float rawTargetAngularY;   // Stores raw angular input

    void Start()
    {
        if (robotBody == null)
            robotBody = GetComponent<ArticulationBody>();
    }

    /// <summary>
    /// Called from KeyboardControl. Sets the raw input axes.
    /// </summary>
    public void SetRobotInput(float x, float z, float angularY)
    {
        // x is sideways (strafe), z is forward/backward
        rawTargetVelocity = new Vector3(x, 0f, z); 
        rawTargetAngularY = angularY;
    }

    void FixedUpdate()
    {
        // --- 1. Apply force for linear movement ---
        // We use the squared magnitude for a tiny bit of efficiency, 
        // as we only care if it's non-zero.
        if (rawTargetVelocity.sqrMagnitude > 0f) 
        {
            // Transform the normalized input vector from local space to world space, 
            // then scale by the forceMultiplier.
            Vector3 localForceDirection = rawTargetVelocity.normalized;
            Vector3 worldForce = transform.TransformDirection(localForceDirection) * forceMultiplier;
            
            // Apply the force at the body's center of mass
            robotBody.AddForce(worldForce, ForceMode.Force);
        }

        // --- 2. Apply torque for rotation ---
        if (Mathf.Abs(rawTargetAngularY) > 0f)
        {
            // Apply torque around the robot's UP axis (local Y), scaled by the input 
            // magnitude and the torqueMultiplier.
            Vector3 torque = transform.up * rawTargetAngularY * torqueMultiplier;
            robotBody.AddTorque(torque, ForceMode.Force);
        }
    }
}