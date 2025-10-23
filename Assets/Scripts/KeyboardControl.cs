using UnityEngine;

/// <summary>
/// Detects keyboard controls and passes RAW input axes to the Articulation wheel and clamp controllers.
/// </summary>

public class KeyboardControl : MonoBehaviour
{
    public ArticulationWheelController wheelController;

    private float rawLinearX;
    private float rawLinearZ;
    private float rawAngularY;


    void Update()
    {
        // Get raw key input axes
        rawLinearZ = Input.GetAxisRaw("HolonomicVertical");   // W/S
        rawLinearX = Input.GetAxisRaw("HolonomicHorizontal"); // A/D
        rawAngularY = Input.GetAxisRaw("HolonomicRotate");    // optional, define "Rotate" axis

    }

    void FixedUpdate()
    {
        wheelController.SetRobotInput(rawLinearX, rawLinearZ, rawAngularY);
    }

}
