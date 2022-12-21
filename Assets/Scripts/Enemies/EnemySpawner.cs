using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public delegate void UpdateGUIDelegate();
    public UpdateGUIDelegate updateGUICallback;

    #region Singleton
    private static EnemySpawner instance;
    public static EnemySpawner Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<EnemySpawner>();
            return instance;
        }
    }
    #endregion

    public Transform Target;
    public Transform SpawnPosition;
    public Transform DestroyBlock;
    public int NumberOfEnemiesToSpawn = 5;
    public int WaveTimer;
    public List<Enemy> EnemyPrefabs = new List<Enemy>();
    
    private List<List<WaveElement>> Waves;
    private int currentWave = 0;
    private int maxWaves = 0;

    public float CurrentWave { get { return currentWave; } }
    public float MaxWaves { get { return maxWaves; } }
    [SerializeField] TextMeshProUGUI textMeshProWaveTimer;


    // enemies are pregenerated on gamestart so that the performance in game is better
    private Dictionary<int, ObjectPool> EnemyObjectPools = new Dictionary<int, ObjectPool>();

    // counter to give each enemy a unique ID
    private int enemyID = 0;

    private void Awake()
    {
        Waves WavesBuilder = new Waves(EnemyPrefabs);
        Waves = WavesBuilder.map_0();
        maxWaves = Waves.Count;

        if (Waves == null)
        {
            throw new System.Exception("Waves is null.");
        }

        List<int> enemyAmounts = getEnemyAmounts(Waves);

        // fill the pool with the according amount of enemies
        for (int i = 0; i < EnemyPrefabs.Count; i++)
        {
            // TODO: throws warning that it couldn't generate agent because it is not close enough to the NavMesh
            //      maybe because Awake happens before the NavMesh Generation/Intialization?
            EnemyObjectPools.Add(i, ObjectPool.CreateInstance(EnemyPrefabs[i], enemyAmounts[i]));
        }
    }

    private void Start()
    {
        StartCoroutine(ProcessWaves(Waves));
    }

    private IEnumerator ProcessWaves(List<List<WaveElement>> waves)
    {
        WaitForSeconds Wait;

        if (waves == null)
        {
            throw new System.Exception("waves is null.");
        }

        
        foreach (var wave in waves)
        {
            currentWave += 1;
            updateGUICallback();
            foreach (var waveElem in wave)
            {
                // either type is delay --> simply wait for specified seconds
                if (waveElem.waveElementType == WaveElementType.Delay)
                {
                    Wait = new WaitForSeconds(waveElem.delay / 1000);
                    yield return Wait;
                }
                // or type is spawn enemy --> spawn the required enemy
                else if (waveElem.waveElementType == WaveElementType.EnemySpawn)
                {
                    Wait = new WaitForSeconds(waveElem.spawnDelay / 1000);

                    for (int i = 0; i < waveElem.enemyCount; i++)
                    {
                        SpawnEnemy(getIndexOfEnemyPool(waveElem.enemyPrefab));                            

                        yield return Wait;
                    }
                }
            }

            // if there are still enemies left, wait until they are all destroyed
            if (GameObject.FindWithTag("Enemy") != null)
            {
                yield return new WaitUntil(() => GameObject.FindWithTag("Enemy") == null);
            }

            // if the last wave was the last wave --> end game
            if (currentWave == maxWaves)
            {
                break;
            }

            // after all enemies are destroyed, wait an additional few seconds and start the counter
            textMeshProWaveTimer.gameObject.SetActive(true);
            for (int i = WaveTimer; i > 0; i--)
            {
                textMeshProWaveTimer.text = "Next Wave: " + i.ToString() + "s";
                Wait = new WaitForSeconds(1);
                yield return Wait;
            }
            textMeshProWaveTimer.gameObject.SetActive(false);
        }
    }

    // spawns an enemy from the given pool
    private void SpawnEnemy(int PoolIndex)
    {
        PoolableObject poolableObject = EnemyObjectPools[PoolIndex].GetObject();

        if (poolableObject != null)
        {
            Enemy enemy = poolableObject.GetComponent<Enemy>();

            enemy.setEnemyID(enemyID);
            enemyID = enemyID + 1;

            // TODO: Gegner spawnen aktuell am Rand des SpawnBlocks, sollen aber auf ihm spawnen
            Vector3 spawnPosition = SpawnPosition.position;

            NavMeshHit Hit;
            if (NavMesh.SamplePosition(spawnPosition, out Hit, 5f, 1)) // -1 for all areas, 1 for walkable
            {
                enemy.Agent.Warp(Hit.position);
                enemy.transform.Rotate(0, -90F, 0); // TODO: make rotation independent from map --> not a fixed value

                enemy.Movement.Target = Target;
                enemy.Agent.enabled = true;

                // increase of agents speed makes agent navigate worse (runs into walls, cannot turn fast enough)
                // --> other parameters have to be adjusted as well
                // 120 and 8 are the standard values for a speed of 1
                enemy.Agent.angularSpeed = enemy.Agent.speed * 120;
                enemy.Agent.acceleration = enemy.Agent.speed * 8;
                enemy.DestroyBlock = DestroyBlock;
            }
        }
        else
        {
            Debug.LogError($"Unable to fetch enemy of type {PoolIndex} from object pool. Out of objects?");
        }
    }

    // returns how many of each enemy can spawn at most so that the pools can be created
    private List<int> getEnemyAmounts(List<List<WaveElement>> waves)
    {
        // for each different Enemy in EnemyPrefabs it holds the maximum amount the enemy will spawn in a wave
        // amount per wave because in a wave multiple batches of the same enemy can be active at once
        //      between waves there won't survive any enemies
        List<int> enemyAmounts = new List<int>();

        foreach (var enemyType in EnemyPrefabs)
        {
            int waveAmounts = 0;
            foreach (var wave in waves)
            {
                foreach(var waveElem in wave)
                {
                    if (waveElem.waveElementType == WaveElementType.EnemySpawn && waveElem.enemyPrefab.Name == enemyType.Name)
                    {
                        waveAmounts += waveElem.enemyCount;
                    }
                }
            }
            enemyAmounts.Add(waveAmounts);
        }

        return enemyAmounts;
    }

    // returns the index of the pool that holds the enemies of the given type
    private int getIndexOfEnemyPool(Enemy enemy)
    {
        // find the pool to spawn enemies from
        int poolIndex = 0;
        for (int j = 0; j < EnemyPrefabs.Count; j++)
        {
            if (EnemyPrefabs[j].Name == enemy.Name)
            {
                return j;
            }
        }

        return poolIndex;
    }
    
}
