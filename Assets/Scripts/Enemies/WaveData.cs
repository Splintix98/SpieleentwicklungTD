using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveData
{
    // The type of enemy to spawn
    public GameObject enemyPrefab;

    // The number of enemies to spawn
    public int enemyCount;

    // The spawn delay between each enemy
    public float spawnDelay;

    // Constructor for the WaveData class
    public WaveData(GameObject enemy, int count, float delay)
    {
        enemyPrefab = enemy;
        enemyCount = count;
        spawnDelay = delay;
    }
}