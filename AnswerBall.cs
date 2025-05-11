using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class AnswerBall : MonoBehaviour
{
    public int ballValue;
    public int slotIndex;
    public AnswerBallBeltManager beltManager;

    private XRGrabInteractable interactable;
    private Rigidbody rb;

    void Awake()
    {
        interactable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.useGravity = false;

        interactable.selectEntered.AddListener(OnGrabbed);
        interactable.selectExited.AddListener(OnReleased);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if we hit the correct object
        EquationMonster monster = collision.gameObject.GetComponent<EquationMonster>();
        if (monster != null)
        {
            monster.CheckAnswer(ballValue);
            Destroy(gameObject); // Remove the ball after it hits
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        // Detach and enable physics
        transform.parent = null;
        rb.isKinematic = false;
        rb.useGravity = true;

        // Immediately schedule respawn for the slot
        beltManager.RespawnBall(slotIndex, 1.0f);

        // Optional: destroy this ball after 5 seconds
        Destroy(gameObject, 5f);
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    public void SetBallValue(int value)
    {
        ballValue = value;

        Canvas canvas = GetComponentInChildren<Canvas>();
        TextMeshProUGUI tmp = canvas.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp != null)
            tmp.text = value.ToString();
    }
    
}

