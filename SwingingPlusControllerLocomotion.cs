using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Allows locomotion in VR by swinging arms, with optional reverse direction via A/X buttons.
/// </summary>
public class SwingingPlusControllerLocomotion : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;

    [Header("Movement Settings")]
    [SerializeField] private float speed = 6.0f;
    [SerializeField] private float velocityThreshold = 0.5f;
    [SerializeField] private float individualYThreshold = 0.2f;
    [SerializeField] private bool applyGravity = false;

    private Vector3 previousPosLeft;
    private Vector3 previousPosRight;
    private readonly Vector3 gravity = new Vector3(0, -9.8f, 0);

    private void Start()
    {
        SetPreviousHandPositions();
    }

    private void Update()
    {
        Vector3 leftHandVelocity = (leftHand.transform.position - previousPosLeft) / Time.deltaTime;
        Vector3 rightHandVelocity = (rightHand.transform.position - previousPosRight) / Time.deltaTime;

        float leftY = Mathf.Abs(leftHandVelocity.y);
        float rightY = Mathf.Abs(rightHandVelocity.y);
        float totalYVelocity = leftY * 0.8f + rightY * 0.8f;

        if (totalYVelocity >= velocityThreshold && leftY >= individualYThreshold && rightY >= individualYThreshold)
        {
            Vector3 rawDir = (rightHand.transform.forward + leftHand.transform.forward) * 0.5f;
            Vector3 planarDir = new Vector3(rawDir.x, 0f, rawDir.z).normalized;

            bool reverse = IsPrimaryButtonPressed(XRNode.LeftHand) || IsPrimaryButtonPressed(XRNode.RightHand);
            if (reverse)
                planarDir = -planarDir;

            characterController.Move(planarDir * speed * Time.deltaTime);

            Debug.DrawRay(transform.position, planarDir, Color.green);
        }

        if (applyGravity)
        {
            characterController.Move(gravity * Time.deltaTime);
        }

        SetPreviousHandPositions();
    }

    private void SetPreviousHandPositions()
    {
        previousPosLeft = leftHand.transform.position;
        previousPosRight = rightHand.transform.position;
    }

    private bool IsPrimaryButtonPressed(XRNode node)
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(node);
        return device.TryGetFeatureValue(CommonUsages.primaryButton, out bool pressed) && pressed;
    }
}
