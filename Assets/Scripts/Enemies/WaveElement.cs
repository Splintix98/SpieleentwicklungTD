using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WaveElementType
{
    Delay,
    EnemySpawn,
}


public class WaveElement
{
    public WaveElementType waveElementType;

    public Enemy enemyPrefab;

    public int enemyCount;

    // delay between enemy spawns
    public float spawnDelay;

    // if WaveElementType == Delay --> Delay that is simply waited
    public float delay;

    public WaveElement(WaveElementType waveElemType, Enemy enemy = null, int enemyCount = 0, float delay = 10000, float spawnDelay = 500)
    {
        this.waveElementType = waveElemType;
        this.enemyPrefab = enemy;
        this.enemyCount = enemyCount;
        this.spawnDelay = spawnDelay;
        this.delay = delay;
    }
}
