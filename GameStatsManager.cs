using UnityEngine;

public class GameStatsManager : MonoBehaviour
{
    public static GameStatsManager Instance;

    public int MonstersDefeated { get; private set; } = 0;
    public float TimeAlive { get; private set; } = 0f;

    public int Score => Mathf.RoundToInt(TimeAlive) + (10 * MonstersDefeated);

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        TimeAlive += Time.deltaTime;
    }

    public void IncrementDefeated()
    {
        MonstersDefeated++;
    }
}

