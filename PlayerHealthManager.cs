// using UnityEngine;
// using TMPro;

// public class PlayerHealthManager : MonoBehaviour
// {
//     [SerializeField] private int maxLives = 3;
//     [SerializeField] private TextMeshProUGUI heartText;
//     [SerializeField] private GameObject gameOverUI;
//     [SerializeField] private GameOverMenu gameOverMenu;

//     private int currentLives;

//     void Start()
//     {
//         currentLives = maxLives;
//         UpdateHeartDisplay();
//         if (gameOverUI != null) gameOverUI.SetActive(false);
//     }

//     public void TakeDamage()
//     {
//         currentLives--;
//         UpdateHeartDisplay();
//         Debug.Log("Player took damage! Lives left: " + currentLives);

//         if (currentLives <= 0)
//         {
//             GameOver();
//         }
//     }

//     void UpdateHeartDisplay()
//     {
//         heartText.text = "♥ " + currentLives;
//     }

//     void GameOver()
//     {
//         Debug.Log("Game Over!");
//         // if (gameOverUI != null) gameOverUI.SetActive(true);
//         gameOverMenu.ShowGameOver();
//         // Freeze gameplay
//         Time.timeScale = 0f;
//     }
// }


using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealthManager : MonoBehaviour
{
    [SerializeField] private int maxLives = 3;
    [SerializeField] private TextMeshProUGUI heartText;
    [SerializeField] private GameObject gameOverPanel;

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

        Time.timeScale = 0f;
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
