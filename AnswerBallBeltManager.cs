using UnityEngine;
using System.Collections;

public class AnswerBallBeltManager : MonoBehaviour
{
    [Header("Ball Settings")]
    public GameObject numberBallPrefab;
    public int numberOfBalls = 9;
    public float radius = 0.5f;
    public float z_offset = 0.25f; // z offset for the balls
    private Transform[] ballSlots;

    [Header("Belt Position Settings")]
    public Transform cameraTransform;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationThresholdAngle = 45f;
    [SerializeField] private float rotationSmoothSpeed = 3f;
    private Quaternion targetRotation;

    void Start()
    {

        Vector3 flatForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        targetRotation = Quaternion.LookRotation(flatForward, Vector3.up);
        transform.rotation = targetRotation;

        ballSlots = new Transform[numberOfBalls];

        for (int i = 0; i < numberOfBalls; i++)
        {
            GameObject slot = new GameObject("BallSlot_" + i);
            slot.transform.parent = transform;
            ballSlots[i] = slot.transform;
        }

        PositionSlotsAlongArc();

        for (int i = 0; i < numberOfBalls; i++)
        {
            SpawnBall(i);
        }
    }

    void Update()
    {
        if (cameraTransform == null) return;

        // Set waist position below camera
        Vector3 headPos = cameraTransform.position;
        transform.position = new Vector3(headPos.x, headPos.y - 0.6f, headPos.z);

        // Get flat direction from camera
        Vector3 flatForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;

        // Check angle from current belt rotation
        float angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(flatForward, Vector3.up));

        if (angle > rotationThresholdAngle)
        {
            targetRotation = Quaternion.LookRotation(flatForward, Vector3.up);
        }

        // Always ease toward targetRotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmoothSpeed);
    }
    

    private void PositionSlotsAlongArc()
    {
        for (int i = 0; i < numberOfBalls; i++)
        {
            float angle = Mathf.Lerp(135f, 45f, (float)i / (numberOfBalls - 1));
            float radians = angle * Mathf.Deg2Rad;

            Vector3 pos = new Vector3(
                radius * Mathf.Cos(radians),
                0f,
                radius * Mathf.Sin(radians) + z_offset
            );

            ballSlots[i].localPosition = pos;
        }
    }

    private void SpawnBall(int index)
    {
        Transform slot = ballSlots[index];

        GameObject ball = Instantiate(numberBallPrefab, slot.position, slot.rotation, slot);
        AnswerBall answerBallScript = ball.GetComponent<AnswerBall>();
        int value = index + 1;
        answerBallScript.SetBallValue(value);
        answerBallScript.slotIndex = index; // assign which slot it's from
        answerBallScript.beltManager = this; // pass ref back

        // Optional: update TMP manually, though SetBallValue handles it
    }

    public void RespawnBall(int slotIndex, float delay = 1.0f)
    {
        StartCoroutine(RespawnAfterDelay(slotIndex, delay));
    }

    private IEnumerator RespawnAfterDelay(int index, float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnBall(index);
    }
}
