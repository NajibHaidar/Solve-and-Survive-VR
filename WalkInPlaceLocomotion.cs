using UnityEngine;
using TMPro;

public class WalkInPlaceLocomotion : MonoBehaviour
{
    [SerializeField] CharacterController characterController; // Reference to the CharacterController component
    
    [SerializeField] GameObject leftHand, rightHand; // References to the left and right hand GameObjects

    Vector3 previousPosLeft, previousPosRight; // Previous positions of the left and right hands
    Vector3 direction; // Direction we want to move in
    Vector3 gravity = new Vector3(0, -9.8f, 0); // Gravity vector

    [SerializeField] float velocityThreshold = 0.5f; // Threshold for hand movement velocity to trigger walking
    [SerializeField] float individualYThreshold = 0.2f;    // Per-hand Y threshold
    [SerializeField] bool applyGravity = false; // Flag to apply gravity to the character controller

    [SerializeField] float speed = 4.0f; // Speed of the character

    // [SerializeField] TextMeshProUGUI debugText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetPreviousPos(); // Initialize the previous positions of the hands
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the velocity of the player hand movement ( we want to know if the player is swinging their arms )
        Vector3 leftHandVelocity = (leftHand.transform.position - previousPosLeft) / Time.deltaTime;
        Vector3 rightHandVelocity = (rightHand.transform.position - previousPosRight) / Time.deltaTime;
        // float totalVelocity = leftHandVelocity.magnitude * 0.8f + rightHandVelocity.magnitude * 0.8f; // Combine the velocities of both hands
        
        float leftY = Mathf.Abs(leftHandVelocity.y);
        float rightY = Mathf.Abs(rightHandVelocity.y);
        float totalYVelocity = leftY * 0.8f + rightY * 0.8f;

        // Debug.Log("Total Velocity: " + totalVelocity);
        // Debug.Log("Move Direction: " + direction);

        if (totalYVelocity >= velocityThreshold && leftY >= individualYThreshold && rightY >= individualYThreshold) // If the total velocity is above a threshold then player has swung their hand
        {
            // Get the direction that the player is facing
            direction = Camera.main.transform.forward; // Get the forward direction of the camera

            // Move the player using character controller
            characterController.Move(speed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up)); // Move the character controller in the direction the player is facing
        }

        // Apply gravity to the character controller
        if (applyGravity == true) 
        {
            characterController.Move(gravity * Time.deltaTime); // Move the character controller downwards due to gravity
        }
       
        SetPreviousPos(); // Update the previous positions of the hands
        // Debug.Log("Delta Time: " + Time.deltaTime);
        // Debug.Log("Left Hand Position: " + leftHand.transform.position); // Log the position of the left hand
        // Debug.Log("Right Hand Position: " + rightHand.transform.position); // Log the position of the right hand
        // Debug.Log("Player Position: " + characterController.transform.position); // Log the position of the player

        // debugText.text = 
        //     "L Vel: " + leftHandVelocity.ToString("F2") + "\n" +
        //     "R Vel: " + rightHandVelocity.ToString("F2") + "\n" +
        //     "Total Vel: " + totalVelocity.ToString("F2") + "\n";

    }

    void SetPreviousPos() 
    {
        previousPosLeft = leftHand.transform.position; // Set the previous position of the left hand to its current position
        previousPosRight = rightHand.transform.position; // Set the previous position of the right hand to its current position
    }
}