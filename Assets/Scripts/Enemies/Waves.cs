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
        wave_1.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Turtle"), enemyCount: 1, spawnDelay: 2000));
        wave_1.Add(new WaveElement(WaveElementType.Delay, delay: 3000));
        wave_1.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Turtle"), enemyCount: 2, spawnDelay: 2000));
        wave_1.Add(new WaveElement(WaveElementType.Delay, delay: 3000));
        wave_1.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Turtle"), enemyCount: 3, spawnDelay: 2000));

        // wave 2
        wave_2.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Turtle"), enemyCount: 2, spawnDelay: 1500));
        wave_2.Add(new WaveElement(WaveElementType.Delay, delay: 2500));
        wave_2.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Turtle"), enemyCount: 3, spawnDelay: 1500));
        wave_2.Add(new WaveElement(WaveElementType.Delay, delay: 2500));
        wave_2.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Turtle"), enemyCount: 4, spawnDelay: 1500));
        wave_2.Add(new WaveElement(WaveElementType.Delay, delay: 2500));
        wave_2.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Turtle"), enemyCount: 5, spawnDelay: 1500));
        wave_2.Add(new WaveElement(WaveElementType.Delay, delay: 2500));
        wave_2.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Turtle"), enemyCount: 10, spawnDelay: 1500));
        wave_2.Add(new WaveElement(WaveElementType.Delay, delay: 2500));

        // wave 3
        wave_3.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Turtle"), enemyCount: 12, spawnDelay: 1250));
        wave_3.Add(new WaveElement(WaveElementType.Delay, delay: 3000));
        wave_3.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Turtle"), enemyCount: 12, spawnDelay: 1250));
        wave_3.Add(new WaveElement(WaveElementType.Delay, delay: 2500));
        wave_3.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Skeleton"), enemyCount: 4, spawnDelay: 500));

        // wave 4
        wave_4.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Skeleton"), enemyCount: 12, spawnDelay: 250));
        wave_4.Add(new WaveElement(WaveElementType.Delay, delay: 5000));
        wave_4.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Skeleton"), enemyCount: 12, spawnDelay: 250));
        wave_4.Add(new WaveElement(WaveElementType.Delay, delay: 5000));
        wave_4.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Skeleton"), enemyCount: 16, spawnDelay: 250));


        // wave 5
        for (int i = 0; i < 10; i++)
        {
            wave_5.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Turtle"), enemyCount: 2, spawnDelay: 800));
            wave_5.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Skeleton"), enemyCount: 2, spawnDelay: 800));
        }

        // wave 6
        wave_6.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "OrcWolfrider"), enemyCount: 1, spawnDelay: 2000));
        wave_6.Add(new WaveElement(WaveElementType.Delay, delay: 4000));
        wave_6.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "OrcWolfrider"), enemyCount: 2, spawnDelay: 3000));
        wave_6.Add(new WaveElement(WaveElementType.Delay, delay: 4000));
        wave_6.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "OrcWolfrider"), enemyCount: 2, spawnDelay: 2500));
        wave_6.Add(new WaveElement(WaveElementType.Delay, delay: 4000));
        wave_6.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "OrcWolfrider"), enemyCount: 2, spawnDelay: 2500));

        // wave 7
        wave_7.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Skeleton"), enemyCount: 10, spawnDelay: 250));
        wave_7.Add(new WaveElement(WaveElementType.Delay, delay: 4000));
        wave_7.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "OrcWolfrider"), enemyCount: 2, spawnDelay: 500));
        wave_7.Add(new WaveElement(WaveElementType.Delay, delay: 4000));
        wave_7.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Skeleton"), enemyCount: 10, spawnDelay: 250));
        wave_7.Add(new WaveElement(WaveElementType.Delay, delay: 4000));
        wave_7.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "OrcWolfrider"), enemyCount: 2, spawnDelay: 500));


        // wave 8
        for (int i = 0; i < 10; i++)
        {
            wave_8.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Turtle"), enemyCount: 2, spawnDelay: 500));
            wave_8.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Skeleton"), enemyCount: 2, spawnDelay: 500));
            wave_8.Add(new WaveElement(WaveElementType.Delay, delay: 500));
        }
        wave_8.Add(new WaveElement(WaveElementType.Delay, delay: 7000));
        wave_8.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "OrcWolfrider"), enemyCount: 2, spawnDelay: 500));
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
