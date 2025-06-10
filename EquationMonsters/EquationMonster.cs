using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public abstract class EquationMonster : MonoBehaviour
{
    protected int correctAnswer;
    private TextMeshProUGUI equationText;

    [Header("Flash Settings")]
    private Renderer[] allRenderers;
    private Dictionary<Renderer, Color> originalColors = new Dictionary<Renderer, Color>();
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;
    public int flashCount = 3;
    private NavMeshAgent agent;
    private bool defeated = false;

    void Awake()
    {
        equationText = GetComponentInChildren<TextMeshProUGUI>();
        if (equationText == null)
        {
            Debug.LogError("EquationMonster: No TextMeshProUGUI found!");
            return;
        }

        agent = GetComponent<NavMeshAgent>();
        allRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in allRenderers)
        {
            if (r != null && r.material != null)
            {
                originalColors[r] = r.material.color;
            }
        }
        // Let the subclass handle equation logic
        GenerateEquation(out string equationStr, out correctAnswer);
        equationText.text = equationStr;
    }


    // ABSTRACT METHOD FOR SUBCLASSES TO IMPLEMENT
    protected abstract void GenerateEquation(out string equationStr, out int answer);

    public virtual void CheckAnswer(int answer)
    {
        if (defeated) return;

        if (answer == correctAnswer)
        {
            defeated = true;
            Debug.Log("IncrementDefeated called");
            GameStatsManager.Instance.IncrementDefeated();
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
            foreach (Renderer r in allRenderers)
            {
                r.material.color = flashColor;
            }
            yield return new WaitForSeconds(flashDuration);

            foreach (Renderer r in allRenderers)
            {
                if (originalColors.ContainsKey(r))
                    r.material.color = originalColors[r];
            }
            yield return new WaitForSeconds(flashDuration);
        }
    }

    public virtual void OnAnswerHit(Collider other)
    {
        if (defeated) return;

        AnswerBall ball = other.GetComponent<AnswerBall>();
        if (ball != null)
        {
            CheckAnswer(ball.ballValue);
            Destroy(ball.gameObject); // destroy ball after use
        }
    }


    public virtual void OnPlayerCollision(Collider player)
    {
        if (defeated) return;

        defeated = true;
        PlayerHealthManager playerHealth = player.GetComponent<PlayerHealthManager>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage();
        }

        Destroy(transform.root.gameObject);
    }
}

