using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BallMergerManager : MonoBehaviour
{
    public float mergeDistance = 0.15f;  // how close is "smashing"
    public Transform leftHand;
    public Transform rightHand;

    void Update()
    {
        // Find held balls in both hands
        AnswerBall leftBall = GetHeldBall(leftHand);
        AnswerBall rightBall = GetHeldBall(rightHand);

        if (leftBall != null && rightBall != null)
        {
            float dist = Vector3.Distance(leftBall.transform.position, rightBall.transform.position);
            if (dist <= mergeDistance)
            {
                // Merge them into the right ball
                int newValue = leftBall.ballValue + rightBall.ballValue;
                rightBall.SetBallValue(newValue);

                Destroy(leftBall.gameObject); // remove left ball
            }
        }
    }

    AnswerBall GetHeldBall(Transform hand)
    {
        XRGrabInteractable[] interactables = hand.GetComponentsInChildren<XRGrabInteractable>();
        foreach (var interactable in interactables)
        {
            AnswerBall ab = interactable.GetComponent<AnswerBall>();
            if (ab != null)
                return ab;
        }
        return null;
    }
}
