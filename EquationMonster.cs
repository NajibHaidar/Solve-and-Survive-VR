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

        agent = GetComponentInParent<NavMeshAgent>();
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

        int blankIndex = Random.Range(0, 2); // 0 = left blank, 1 = right blank
        string equationStr = "";

        // Choose operator (for now it's always addition)
        string op = "+";

        // Uncomment below to randomly pick an operator once ball smashing logic is implemented
        /*
        string[] ops = { "+", "-", "*", "/" };
        op = ops[Random.Range(0, ops.Length)];

        // Recalculate c based on operator
        switch (op)
        {
            case "+": c = a + b; break;
            case "-": c = a - b; break;
            case "*": c = a * b; break;
            case "/": 
                // Ensure no division by zero and clean division
                b = Random.Range(1, 10);
                c = a;
                a = b * Random.Range(1, 10); // force a divisible number
                break;
        }
        */

        if (blankIndex == 0)
        {
            correctAnswer = a;
            equationStr = "_ " + op + " " + b + " = " + c;
        }
        else
        {
            correctAnswer = b;
            equationStr = a + " " + op + " _ = " + c;
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

            Destroy(transform.root.gameObject); // Destroy monster on hit
        }
    }

}
