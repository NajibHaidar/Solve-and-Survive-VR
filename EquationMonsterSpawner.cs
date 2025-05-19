using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquationMonsterSpawner : MonoBehaviour
{
    [Header("Monster Settings")]
    public GameObject monsterPrefab;
    public float spawnInterval = 5f;

    [Header("Spawn Manager")]
    public Transform spawnManager;  // Assign your SpawnManager parent in the Inspector

    private List<Transform> spawnPoints = new List<Transform>();

    void Start()
    {
        // Get all child spawn points of the SpawnManager
        foreach (Transform child in spawnManager)
        {
            spawnPoints.Add(child);
        }

        StartCoroutine(SpawnMonstersLoop());
    }

    IEnumerator SpawnMonstersLoop()
    {
        while (true)
        {
            SpawnMonsterAtRandomPoint();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnMonsterAtRandomPoint()
    {
        if (spawnPoints.Count == 0 || monsterPrefab == null) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Instantiate(monsterPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
