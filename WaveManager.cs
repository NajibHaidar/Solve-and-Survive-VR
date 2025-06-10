using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public EquationMonsterSpawner spawner;        // Reference to spawner script
    public float timeBetweenWaves = 5f;
    private int waveNumber = 0;
    public int CurrentWave => waveNumber;
    public TextMeshProUGUI waveOverlayText;
    public CanvasGroup waveCanvasGroup;  // drag the CanvasGroup here
    public float overlayFadeDuration = 1f;
    public float overlayDisplayTime = 3f;


    void Start()
    {
        StartCoroutine(StartNextWave());
    }

    private IEnumerator StartNextWave()
    {
        while (true)
        {
            waveNumber++;

            yield return StartCoroutine(ShowWaveOverlay($"WAVE {waveNumber}"));

            spawner.SpawnWave(waveNumber);
            yield return new WaitUntil(() => !spawner.AreMonstersAlive());
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    private IEnumerator ShowWaveOverlay(string message)
    {
        waveOverlayText.text = message;
        waveOverlayText.gameObject.SetActive(true);
        waveCanvasGroup.alpha = 0;

        // Fade in
        float t = 0;
        while (t < overlayFadeDuration)
        {
            waveCanvasGroup.alpha = Mathf.Lerp(0, 1, t / overlayFadeDuration);
            t += Time.deltaTime;
            yield return null;
        }
        waveCanvasGroup.alpha = 1;

        // Wait
        yield return new WaitForSeconds(overlayDisplayTime);

        // Fade out
        t = 0;
        while (t < overlayFadeDuration)
        {
            waveCanvasGroup.alpha = Mathf.Lerp(1, 0, t / overlayFadeDuration);
            t += Time.deltaTime;
            yield return null;
        }
        waveCanvasGroup.alpha = 0;
        waveOverlayText.gameObject.SetActive(false);
    }

}
