using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverPanel;

    public void ShowGameOver()
    {
        // Debug.Log("Prefab instance? " + gameOverPanel.scene.IsValid()); // → False

        // Debug.Log(gameOverPanel.activeInHierarchy);
        // Debug.Log("GameOver panel ref: " + gameOverPanel);

        gameOverPanel.SetActive(true);
        // Debug.Log("After SetActive: " + gameOverPanel.activeInHierarchy);

        // Transform current = gameOverPanel.transform;
        // while (current != null)
        // {
        //     Debug.Log($"Parent: {current.name} | ActiveSelf: {current.gameObject.activeSelf} | ActiveInHierarchy: {current.gameObject.activeInHierarchy}");
        //     current = current.parent;
        // }

    }

    public void PlayAgain()
    {
        Time.timeScale = 1f; // Resume normal time
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
