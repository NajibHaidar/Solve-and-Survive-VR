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

    void Start()
    {
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
        if (cameraTransform != null)
        {
            Vector3 currentPosition = transform.position; // keep belt position fixed

            // Only rotate Y to match the camera's Y-axis rotation
            Quaternion cameraRotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);
            transform.rotation = cameraRotation;

            transform.position = currentPosition; // reapply fixed position to ensure no drift
        }
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
