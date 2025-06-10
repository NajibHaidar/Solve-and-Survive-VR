using UnityEngine;
using TMPro;

public class GameStatsUI : MonoBehaviour
{
    public TextMeshProUGUI defeatedText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI aliveText;
    public WaveManager waveManager;
    public EquationMonsterSpawner spawner;


    void Update()
    {
        // Only update if the menu is currently active
        if (!gameObject.activeInHierarchy) return;
        UpdateUI();
    }

    void UpdateUI()
    {
        var stats = GameStatsManager.Instance;
        defeatedText.text = $"Equation Monsters Defeated: {stats.MonstersDefeated}";
        int score = Mathf.RoundToInt(stats.TimeAlive) + (10 * stats.MonstersDefeated);
        scoreText.text = $"Total Score: {score}";

        if (waveManager != null)
            waveText.text = $"Wave: {waveManager.CurrentWave}";

        if (spawner != null)
        {
            int aliveCount = GameObject.FindGameObjectsWithTag("EquationMonster").Length;
            aliveText.text = $"Monsters Alive: {aliveCount}";
        }
    }
}
