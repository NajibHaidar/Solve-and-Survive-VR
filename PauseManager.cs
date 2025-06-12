using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    [SerializeField] private GameObject mirror;

    // New UI fields
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI defeatedText;
    public TextMeshProUGUI waveText;

    private bool isPaused = false;
    private bool wasMenuButtonPressedLastFrame = false;
    private List<GameObject> pausedBalls = new List<GameObject>();

    void Update()
    {
        bool isMenuButtonPressed = IsPauseButtonPressed(XRNode.LeftHand) || IsPauseButtonPressed(XRNode.RightHand);

        if (isMenuButtonPressed && !wasMenuButtonPressedLastFrame)
        {
            if (!isPaused) Pause();
            else Resume();
        }

        wasMenuButtonPressedLastFrame = isMenuButtonPressed;
    }

    private bool IsPauseButtonPressed(XRNode node)
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(node);
        return device.TryGetFeatureValue(CommonUsages.menuButton, out bool pressed) && pressed;
    }

    public void Pause()
    {
        if (mirror != null)
            mirror.SetActive(false);

        pausedBalls.Clear();

        foreach (GameObject ball in GameObject.FindGameObjectsWithTag("AnswerBall"))
        {
            pausedBalls.Add(ball);
            ball.SetActive(false);
        }

        UpdatePauseStats();

        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        isPaused = true;
    }

    public void Resume()
    {
        if (mirror != null)
            mirror.SetActive(true);

        foreach (GameObject ball in pausedBalls)
        {
            if (ball != null)
                ball.SetActive(true);
        }

        pausedBalls.Clear();

        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        isPaused = false;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    private void UpdatePauseStats()
    {
        var stats = GameStatsManager.Instance;
        int score = stats.Score;
        int defeated = stats.MonstersDefeated;
        int wave = FindFirstObjectByType<WaveManager>()?.CurrentWave ?? 0;

        if (scoreText != null) scoreText.text = $"CURRENT SCORE: {score}";
        if (defeatedText != null) defeatedText.text = $"TOTAL EQUATIONS SOVLED: {defeated}";
        if (waveText != null) waveText.text = $"CURRENT WAVE: {wave}";
    }
}

