using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves
{

    private List<Enemy> enemyPrefabs = new List<Enemy>();

    public List<WaveElement> wave_1 = new List<WaveElement>();
    public List<WaveElement> wave_2 = new List<WaveElement>();

    private void initializeWave_1()
    {
        wave_1.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Turtle"), enemyCount: 15, spawnDelay: 500));
        wave_1.Add(new WaveElement(WaveElementType.Delay, delay: 3000));
        wave_1.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Skeleton"), enemyCount: 5, spawnDelay: 250));
    }

    private void initializeWave_2()
    {
        wave_2.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Skeleton"), enemyCount: 10, spawnDelay: 500));
        wave_2.Add(new WaveElement(WaveElementType.Delay, delay: 3000));
        wave_2.Add(new WaveElement(WaveElementType.EnemySpawn, enemyPrefabs.Find(e => e.Name == "Skeleton"), enemyCount: 10, spawnDelay: 250));
    }

    public Waves(List<Enemy> enemyPrefabs)
    {
        this.enemyPrefabs = enemyPrefabs;
    }

    public List<List<WaveElement>> map_0()
    {
        initializeWave_1();
        initializeWave_2();

        List<List<WaveElement>> waves_return = new List<List<WaveElement>>();
        waves_return.Add(wave_1);
        waves_return.Add(wave_2);
        return waves_return;
    }
}
