using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public Transform Target;
    public Transform SpawnPosition;
    public Transform DestroyBlock;
    public int NumberOfEnemiesToSpawn = 5;
    public float SpawnDelay = 1f;
    public List<Enemy> EnemyPrefabs = new List<Enemy>();

    // enemies are pregenerated on gamestart so that the performance in game is better
    private Dictionary<int, ObjectPool> EnemyObjectPools = new Dictionary<int, ObjectPool>();

    // counter to give each enemy a unique ID
    private int enemyID = 0;

    private void Awake()
    {
        // fill the pool with the according amount of enemies
        for (int i = 0; i < EnemyPrefabs.Count; i++)
        {
            // TODO: throws warning that it couldn't generate agent because it is not close enough to the NavMesh
            EnemyObjectPools.Add(i, ObjectPool.CreateInstance(EnemyPrefabs[i], NumberOfEnemiesToSpawn/EnemyPrefabs.Count));
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        WaitForSeconds Wait = new WaitForSeconds(SpawnDelay);

        int spawnedEnemies = 0;

        while (spawnedEnemies < NumberOfEnemiesToSpawn)
        {
            // cycles through the pools so that enemies from all pools are spawned
            int PoolIndex = spawnedEnemies % EnemyPrefabs.Count;
            
            SpawnEnemy(PoolIndex);
            spawnedEnemies++;
            yield return Wait;
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
}
