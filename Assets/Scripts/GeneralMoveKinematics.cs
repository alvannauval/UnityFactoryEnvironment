using UnityEngine;

public class GeneralMoveKinematics : MonoBehaviour
{
    // Enum to select the movement axis in the Inspector
    public enum MovementAxis { X_Axis, Y_Axis, Z_Axis }

    [Header("Movement Setup")]
    public MovementAxis axisOfMovement = MovementAxis.Z_Axis;
    public string inputAxisName = "ViseAxis1";
    public float speed = 2f;
    
    // New parameter to invert the direction
    public bool invertMovement = false; 

    [Header("Limit Setup (Relative to Start)")]
    public float maxForwardDistance = 5f; 
    public float maxBackwardDistance = 5f; 

    private Rigidbody rb;
    private float startingPosition; // Stores the starting coordinate of the selected axis

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found. Disabling script.");
            enabled = false;
            return;
        }

        // Store the initial position based on the selected axis
        if (axisOfMovement == MovementAxis.X_Axis)
            startingPosition = transform.position.x;
        else if (axisOfMovement == MovementAxis.Y_Axis)
            startingPosition = transform.position.y;
        else // Z_Axis
            startingPosition = transform.position.z;
    }

    void FixedUpdate()
    {
        // 1. Get the raw input
        float move = Input.GetAxis(inputAxisName);

        // 2. APPLY INVERSION: If invertMovement is true, multiply the input by -1
        if (invertMovement)
        {
            move *= -1f;
        }

        // 3. Determine the movement vector based on the selected axis
        Vector3 movement = Vector3.zero;
        if (axisOfMovement == MovementAxis.X_Axis)
            movement = new Vector3(move, 0, 0);
        else if (axisOfMovement == MovementAxis.Y_Axis)
            movement = new Vector3(0, move, 0);
        else // Z_Axis
            movement = new Vector3(0, 0, move);

        // 4. Calculate the new global position *before* moving
        Vector3 newPosition = transform.position + movement * Time.fixedDeltaTime * speed;

        // --- GENERALIZED LOCAL LIMITATION LOGIC ---

        // 5. Calculate the absolute global limits for the selected axis
        // The limits themselves do not change based on inversion, as they are relative to the start.
        float minGlobalLimit = startingPosition - maxBackwardDistance;
        float maxGlobalLimit = startingPosition + maxForwardDistance;

        // 6. Clamp the position of the selected axis
        if (axisOfMovement == MovementAxis.X_Axis)
            newPosition.x = Mathf.Clamp(newPosition.x, minGlobalLimit, maxGlobalLimit);
        else if (axisOfMovement == MovementAxis.Y_Axis)
            newPosition.y = Mathf.Clamp(newPosition.y, minGlobalLimit, maxGlobalLimit);
        else // Z_Axis
            newPosition.z = Mathf.Clamp(newPosition.z, minGlobalLimit, maxGlobalLimit);

        // 7. Apply the clamped position
        rb.MovePosition(newPosition);
    }
}