using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public EquationMonsterSpawner spawner;        // Reference to spawner script
    public TextMeshProUGUI waveText;              // Optional: UI to show wave number
    public float timeBetweenWaves = 5f;

    private int waveNumber = 0;

    void Start()
    {
        StartCoroutine(StartNextWave());
    }

    private IEnumerator StartNextWave()
    {
        while (true)
        {
            waveNumber++;
            if (waveText != null)
                waveText.text = "Wave " + waveNumber;

            spawner.SpawnWave(waveNumber);

            yield return new WaitUntil(() => !spawner.AreMonstersAlive()); // Wait until all monsters are defeated before starting the next wave
            yield return new WaitForSeconds(timeBetweenWaves); // Wait before starting the next wave

        }
    }
}
