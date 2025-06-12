using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System.Collections;

public class AnswerBall : MonoBehaviour
{
    public int ballValue;
    public int slotIndex;
    public AnswerBallBeltManager beltManager;

    private XRGrabInteractable interactable;
    private Rigidbody rb;
    private bool hasMerged = false;
    private bool hasBeenGrabbed = false;
    private Coroutine destroyCoroutine;

    [SerializeField] private float selfDestructDelay = 10f; // Time before the ball self-destructs if not grabbed


    void Awake()
    {
        interactable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.useGravity = false;

        interactable.selectEntered.AddListener(OnGrabbed);
        interactable.selectExited.AddListener(OnReleased);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only the ball being held should initiate the merge
        if (!hasBeenGrabbed) return;

        if (other.CompareTag("AnswerBall"))
        {
            AnswerBall otherBall = other.GetComponent<AnswerBall>();

            // Only merge if the other ball has been grabbed
            if (otherBall != null && !otherBall.hasMerged && otherBall.hasBeenGrabbed && otherBall != this)
            {
                // Merge the other ball into THIS one
                hasMerged = true;

                SetBallValue(ballValue + otherBall.ballValue);
                Destroy(otherBall.gameObject);

                Invoke(nameof(ResetMergeFlag), 0.2f);
            }
        }
    }


    private void OnGrabbed(SelectEnterEventArgs args)
    {
        hasBeenGrabbed = true;

        // Cancel any scheduled destruction
        if (destroyCoroutine != null)
        {
            StopCoroutine(destroyCoroutine);
            destroyCoroutine = null;
        }

        // Detach and enable physics
        transform.parent = null;
        rb.isKinematic = false;
        rb.useGravity = true;

        // Immediately schedule respawn for the slot
        beltManager.RespawnBall(slotIndex, 1.0f);
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        rb.isKinematic = false;
        rb.useGravity = true;

        // Start destroy timer now
        destroyCoroutine = StartCoroutine(DestroyAfterSeconds(selfDestructDelay));
    }

    private IEnumerator DestroyAfterSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    public void SetBallValue(int value)
    {
        ballValue = value;

        Canvas canvas = GetComponentInChildren<Canvas>();
        TextMeshProUGUI tmp = canvas.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp != null)
            tmp.text = value.ToString();
    }

    public bool HasBeenGrabbed()
    {
        return hasBeenGrabbed;
    }

    private void ResetMergeFlag()
    {
        hasMerged = false;
    }


}

