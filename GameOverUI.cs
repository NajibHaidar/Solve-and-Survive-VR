using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GameOverUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI title;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI defeatedText;
    public TextMeshProUGUI waveText;
    public Button restartButton;
    public Button mainMenuButton;

    public void ShowGameOver()
    {
        var stats = GameStatsManager.Instance;
        int score = stats.Score;
        int defeated = stats.MonstersDefeated;
        int wave = FindFirstObjectByType<WaveManager>()?.CurrentWave ?? 0;

        scoreText.text = $"Final Score: {score}";
        defeatedText.text = $"Monsters Defeated: {defeated}";
        waveText.text = $"Wave Reached: {wave}";
    }
}
