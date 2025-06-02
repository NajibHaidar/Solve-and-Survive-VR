using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquationMonsterSpawner : MonoBehaviour
{
    [Header("Monster Types")]
    public GameObject[] monsterPrefabs;

    [Header("Spawn Manager")]
    public Transform spawnManager;  // Assign your SpawnManager parent in the Inspector

    private List<Transform> spawnPoints = new List<Transform>();

    void Awake()
    {
        foreach (Transform child in spawnManager)
        {
            spawnPoints.Add(child);
        }
    }

    public void SpawnWave(int waveNumber)
    {
        int baseEnemies = 10;
        float growthFactor = 7f;  // faster/slower growth
        int maxEnemies = 60;

        int monstersToSpawn = Mathf.Min(
            baseEnemies + Mathf.FloorToInt(Mathf.Log(waveNumber + 1) * growthFactor),
            maxEnemies
        );

        StartCoroutine(SpawnWaveCoroutine(monstersToSpawn, waveNumber));
    }

    private IEnumerator SpawnWaveCoroutine(int count, int waveNumber)
    {
        if (monsterPrefabs.Length == 0)
        {
            Debug.LogError("No monster prefabs assigned to EquationMonsterSpawner!");
            yield break;
        }

        float delayBetweenSpawns = Mathf.Max(0.2f, 2.0f - (waveNumber * 0.1f)); // faster spawns in later waves

        for (int i = 0; i < count; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            GameObject randomPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];
            GameObject monster = Instantiate(randomPrefab, spawnPoint.position, spawnPoint.rotation);

            if (waveNumber % 2 == 0)
            {
                var agent = monster.GetComponent<UnityEngine.AI.NavMeshAgent>();
                if (agent != null)
                {
                    agent.speed += waveNumber * 0.1f;
                }
            }

            yield return new WaitForSeconds(delayBetweenSpawns);
        }
    }


    public bool AreMonstersAlive()
    {
        return GameObject.FindGameObjectsWithTag("EquationMonster").Length > 0;
    }
}

