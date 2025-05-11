using System.Collections;
using UnityEngine;

public class EquationMonsterSpawner : MonoBehaviour
{
    [Header("Monster Settings")]
    public GameObject monsterPrefab;
    public float spawnInterval = 5f;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    void Start()
    {
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
        if (spawnPoints.Length == 0 || monsterPrefab == null) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(monsterPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
