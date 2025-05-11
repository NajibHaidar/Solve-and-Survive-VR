using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System.Collections;

public class EquationMonster : MonoBehaviour
{
    public int correctAnswer;
    private TextMeshProUGUI equationText;

    [Header("Flash Settings")]
    public Renderer visualRenderer; // Drag your monster's visual mesh here
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

        agent = GetComponent<NavMeshAgent>();
        if (visualRenderer == null)
        {
            visualRenderer = GetComponentInChildren<Renderer>();
        }
        originalColor = visualRenderer.material.color;

        GenerateRandomEquation();
    }

    void GenerateRandomEquation()
    {
        int a = Random.Range(1, 10);
        int b = Random.Range(1, 10);
        int c = a + b;

        int blankIndex = Random.Range(0, 2);
        string equationStr = "";

        if (blankIndex == 0)
        {
            correctAnswer = a;
            equationStr = "_ + " + b + " = " + c;
        }
        else
        {
            correctAnswer = b;
            equationStr = a + " + _ = " + c;
        }

        equationText.text = equationStr;
    }

    public void CheckAnswer(int answer)
    {
        if (answer == correctAnswer)
        {
            // Increment monster defeated count
            GameStatsUI statsUI = FindFirstObjectByType<GameStatsUI>();
            if (statsUI != null)
            {
                statsUI.IncrementDefeated();
            }
            Destroy(transform.root.gameObject); // Correct → kill monster #TODO later make it into a death animation
        }
        else
        {
            StartCoroutine(FlashRed());
            agent.speed += 0.5f; // Make it faster
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
}
