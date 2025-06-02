using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System.Collections;

public abstract class EquationMonster : MonoBehaviour
{
    protected int correctAnswer;
    private TextMeshProUGUI equationText;

    [Header("Flash Settings")]
    public Renderer visualRenderer;
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;
    public int flashCount = 3;

    private Color originalColor;
    private NavMeshAgent agent;

    void Awake()
    {
        equationText = GetComponentInChildren<TextMeshProUGUI>();
        if (equationText == null)
        {
            Debug.LogError("EquationMonster: No TextMeshProUGUI found!");
            return;
        }

        agent = GetComponentInParent<NavMeshAgent>();
        if (visualRenderer == null)
        {
            visualRenderer = GetComponentInChildren<Renderer>();
        }
        originalColor = visualRenderer.material.color;

        // Let the subclass handle equation logic
        GenerateEquation(out string equationStr, out correctAnswer);
        equationText.text = equationStr;
    }

    // ⬇️ ABSTRACT METHOD FOR SUBCLASSES TO IMPLEMENT
    protected abstract void GenerateEquation(out string equationStr, out int answer);

    public virtual void CheckAnswer(int answer)
    {
        if (answer == correctAnswer)
        {
            GameStatsUI statsUI = FindFirstObjectByType<GameStatsUI>();
            if (statsUI != null)
            {
                statsUI.IncrementDefeated();
            }
            Destroy(transform.root.gameObject);
        }
        else
        {
            StartCoroutine(FlashRed());
            agent.speed += 0.5f;
        }
    }

    private IEnumerator FlashRed()
    {
        for (int i = 0; i < flashCount; i++)
        {
            visualRenderer.material.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            visualRenderer.material.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Monster Trigger Entered: " + other.name);
        if (other.CompareTag("Player"))
        {
            PlayerHealthManager playerHealth = other.GetComponent<PlayerHealthManager>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage();
            }

            Destroy(transform.root.gameObject);
        }
    }
}
