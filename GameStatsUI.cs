using UnityEngine;
using TMPro;

public class GameStatsUI : MonoBehaviour
{
    public TextMeshProUGUI defeatedText;
    public TextMeshProUGUI scoreText;

    private int monstersDefeated = 0;
    private float timeAlive = 0f;

    void Update()
    {
        timeAlive += Time.deltaTime;
        UpdateUI();
    }

    public void IncrementDefeated()
    {
        monstersDefeated++;
    }

    void UpdateUI()
    {
        defeatedText.text = $"Equation Monsters Defeated: {monstersDefeated}";
        int score = Mathf.RoundToInt(timeAlive) + (10 * monstersDefeated);
        scoreText.text = $"Total Score: {score}";
    }
}
