using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealthManager : MonoBehaviour
{
    [SerializeField] private int maxLives = 3;
    [SerializeField] private TextMeshProUGUI heartText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject mirror;

    private int currentLives;

    void Start()
    {
        currentLives = maxLives;
        UpdateHeartDisplay();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void TakeDamage()
    {
        currentLives--;
        UpdateHeartDisplay();
        Debug.Log("Player took damage! Lives left: " + currentLives);

        if (currentLives <= 0)
        {
            TriggerGameOver();
        }
    }

    void UpdateHeartDisplay()
    {
        if (heartText != null)
            heartText.text = "♥ " + currentLives;
    }

    void TriggerGameOver()
    {
        Debug.Log("Game Over!");

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
            gameOverPanel.GetComponent<GameOverUI>().ShowGameOver();

        // Deactivate mirror
        if (mirror != null)
            mirror.SetActive(false);

        // Deactivate AnswerBalls by tag

        foreach (GameObject ball in GameObject.FindGameObjectsWithTag("AnswerBall"))
            ball.SetActive(false);

        Time.timeScale = 0f;
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
