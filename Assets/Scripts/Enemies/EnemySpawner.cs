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
    public float MovementSpeed = 1f;
    public List<Enemy> EnemyPrefabs = new List<Enemy>();

    private NavMeshTriangulation Triangulation;
    private Dictionary<int, ObjectPool> EnemyObjectPools = new Dictionary<int, ObjectPool>();

    private int enemyID = 0;

    private void Awake()
    {
        for (int i = 0; i < EnemyPrefabs.Count; i++)
        {
            EnemyObjectPools.Add(i, ObjectPool.CreateInstance(EnemyPrefabs[i], NumberOfEnemiesToSpawn));
        }
    }

    private void Start()
    {
        Triangulation = NavMesh.CalculateTriangulation();
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        WaitForSeconds Wait = new WaitForSeconds(SpawnDelay);

        int spawnedEnemies = 0;

        while (spawnedEnemies < NumberOfEnemiesToSpawn)
        {
            int SpawnIndex = spawnedEnemies % EnemyPrefabs.Count;
            SpawnEnemy(SpawnIndex);
            spawnedEnemies++;
            yield return Wait;
        }
    }

    private void SpawnEnemy(int SpawnIndex)
    {
        PoolableObject poolableObject = EnemyObjectPools[SpawnIndex].GetObject();

        if (poolableObject != null)
        {
            Enemy enemy = poolableObject.GetComponent<Enemy>();

            enemy.setEnemyID(enemyID);
            enemyID = enemyID + 1;

            int VertexIndex = Random.Range(0, Triangulation.vertices.Length);

            // Vector3 sourcePosition = Triangulation.vertices[VertexIndex];
            // sourcePosition = new Vector3(0.5f, 0.0f, -3.5f);
            Vector3 sourcePosition = SpawnPosition.position;
            // TODO: Gegner spawnen aktuell am Rand des SpawnBlocks, sollen aber auf ihm spawnen
            // TODO: Gegner gucken aktuell beim spawnen nicht in Richtung des Pfads

            NavMeshHit Hit;
            if (NavMesh.SamplePosition(sourcePosition, out Hit, 2f, 1)) // -1 for all areas, 1 for walkable
            {
                enemy.Agent.Warp(Hit.position);
                
                // attempt to let the agent look in the right direction when spawned
                /*
                var turnTorwardNavSteeringTarget = enemy.Agent.steeringTarget;
                Vector3 direction = (turnTorwardNavSteeringTarget - enemy.transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, lookRotation, Time.deltaTime * 5);
                */

                enemy.Movement.Target = Target;
                enemy.Agent.enabled = true;

                // MovementSpeed increase makes agent navigate worse (runs into walls, cannot turn fast enough)
                // --> other parameters have to be adjusted as well
                enemy.Agent.speed = MovementSpeed;
                enemy.Agent.angularSpeed = MovementSpeed * 120;
                enemy.Agent.acceleration = MovementSpeed * 8;
                enemy.DestroyBlock = DestroyBlock;
            }
        }
        else
        {
            Debug.LogError($"Unable to fetch enemy of type {SpawnIndex} from object pool. Out of objects?");
        }
    }
}