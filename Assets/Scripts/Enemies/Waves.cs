using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Waves
{

    private List<Enemy> enemyPrefabs = new List<Enemy>();

    public List<WaveElement> wave_1 = new List<WaveElement>();
    public List<WaveElement> wave_2 = new List<WaveElement>();
    public List<WaveElement> wave_3 = new List<WaveElement>();
    public List<WaveElement> wave_4 = new List<WaveElement>();
    public List<WaveElement> wave_5 = new List<WaveElement>();
    public List<WaveElement> wave_6 = new List<WaveElement>();
    public List<WaveElement> wave_7 = new List<WaveElement>();
    public List<WaveElement> wave_8 = new List<WaveElement>();


    private void initializeWaves()
    {
        // wave 1
        wave_1.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Turtle"), enemyCount: 3, spawnDelay: 1000));
        wave_1.Add(new WaveElement(WaveElementType.Delay, delay: 2000));
        wave_1.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Turtle"), enemyCount: 3, spawnDelay: 1000));
        wave_1.Add(new WaveElement(WaveElementType.Delay, delay: 2000));
        wave_1.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Turtle"), enemyCount: 3, spawnDelay: 1000));

        // wave 2
        wave_2.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Turtle"), enemyCount: 1, spawnDelay: 1000));
        wave_2.Add(new WaveElement(WaveElementType.Delay, delay: 2000));
        wave_2.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Turtle"), enemyCount: 2, spawnDelay: 1000));
        wave_2.Add(new WaveElement(WaveElementType.Delay, delay: 2000));
        wave_2.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Turtle"), enemyCount: 3, spawnDelay: 1000));
        wave_2.Add(new WaveElement(WaveElementType.Delay, delay: 2000));
        wave_2.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Turtle"), enemyCount: 4, spawnDelay: 1000));
        wave_2.Add(new WaveElement(WaveElementType.Delay, delay: 2000));
        wave_2.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Turtle"), enemyCount: 5, spawnDelay: 1000));
        wave_2.Add(new WaveElement(WaveElementType.Delay, delay: 2000));

        // wave 3
        wave_3.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Turtle"), enemyCount: 5, spawnDelay: 1000));
        wave_3.Add(new WaveElement(WaveElementType.Delay, delay: 2000));
        wave_3.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Turtle"), enemyCount: 5, spawnDelay: 1000));
        wave_3.Add(new WaveElement(WaveElementType.Delay, delay: 2000));
        wave_3.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Skeleton"), enemyCount: 2, spawnDelay: 500));

        // wave 4
        wave_4.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Skeleton"), enemyCount: 1, spawnDelay: 500));
        wave_4.Add(new WaveElement(WaveElementType.Delay, delay: 2000));
        wave_4.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Skeleton"), enemyCount: 2, spawnDelay: 500));
        wave_4.Add(new WaveElement(WaveElementType.Delay, delay: 2000));
        wave_4.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Skeleton"), enemyCount: 3, spawnDelay: 500));
        wave_4.Add(new WaveElement(WaveElementType.Delay, delay: 2000));

        // wave 5
        wave_5.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Skeleton"), enemyCount: 15, spawnDelay: 750));

        // wave 6
        // Enemy 3 - TODO
        wave_6.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Turtle"), enemyCount: 1, spawnDelay: 1000));

        // wave 7
        // Enemy 3 - TODO
        wave_7.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Turtle"), enemyCount: 1, spawnDelay: 1000));

        // wave 8
        // Enemy 3 - TODO
        wave_8.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Turtle"), enemyCount: 1, spawnDelay: 1000));
    }

    public Waves(List<Enemy> enemyPrefabs)
    {
        this.enemyPrefabs = enemyPrefabs;
    }

    public List<List<WaveElement>> map_0()
    {
        initializeWaves();

        List<List<WaveElement>> waves_return = new List<List<WaveElement>>
        {
            wave_1,
            wave_2,
            wave_3,
            wave_4,
            wave_5,
            wave_6,
            wave_7,
            wave_8
        };
        return waves_return;
    }
}
