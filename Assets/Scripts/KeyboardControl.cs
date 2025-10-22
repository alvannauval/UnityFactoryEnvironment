using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Detects keyboard controls and passes target velocity to the wheel controller
/// </summary>
public class KeyboardControl : MonoBehaviour
{
    public ArticulationWheelController wheelController;

    public float speed = 50f;          // linear speed multiplier
    public float angularSpeed = 20f;   // rotational speed multiplier

    private float targetLinearX;
    private float targetLinearZ;
    private float targetAngularY;

    void Update()
    {
        // Get key input
        targetLinearZ = Input.GetAxisRaw("HolonomicVertical") * speed;    // W/S
        targetLinearX = Input.GetAxisRaw("HolonomicHorizontal") * speed;  // A/D
        targetAngularY = Input.GetAxisRaw("HolonomicRotate") * angularSpeed; // optional, define "Rotate" axis in InputManager
    }

    void FixedUpdate()
    {
        wheelController.SetRobotVelocity(targetLinearX, targetLinearZ, targetAngularY);
    }
}
