using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Applies omnidirectional force to a robot with ArticulationBody
/// </summary>
[RequireComponent(typeof(ArticulationBody))]
public class ArticulationWheelController : MonoBehaviour
{
    public ArticulationBody robotBody;
    
    // NOTE: Force/Torque Multipliers will need significant tuning
    public float forceMultiplier = 1250f; 
    public float torqueMultiplier = 50f;

    private Vector3 targetVelocity;
    private float targetAngularY;

    void Start()
    {
        if (robotBody == null)
            robotBody = GetComponent<ArticulationBody>();
    }

    /// <summary>
    /// Called from KeyboardControl
    /// </summary>
    public void SetRobotVelocity(float x, float z, float angularY)
    {
        // x is sideways (strafe), z is forward/backward
        targetVelocity = new Vector3(x, 0f, z); 
        targetAngularY = angularY;
    }

    void FixedUpdate()
    {
        // --- 1. Apply force for linear movement ---
        if (targetVelocity.magnitude > 0f)
        {
            // TRANSFORM THE VELOCITY/FORCE VECTOR FROM LOCAL SPACE TO WORLD SPACE
            // This is the CRITICAL change: it ensures that when you press 'A', the robot
            // moves to its local left (transform.right) and 'W' moves it forward (transform.forward).
            Vector3 localForceDirection = targetVelocity.normalized;
            Vector3 worldForce = transform.TransformDirection(localForceDirection) * forceMultiplier;
            
            // Apply the force at the body's center of mass
            robotBody.AddForce(worldForce, ForceMode.Force);
        }

        // --- 2. Apply torque for rotation ---
        if (Mathf.Abs(targetAngularY) > 0f)
        {
            // Apply torque around the robot's UP axis (local Y)
            Vector3 torque = transform.up * targetAngularY * torqueMultiplier;
            robotBody.AddTorque(torque, ForceMode.Force);
        }
    }
}