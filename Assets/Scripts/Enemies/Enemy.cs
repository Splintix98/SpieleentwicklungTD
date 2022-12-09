using UnityEngine;
using UnityEngine.AI;
using System;

public class Enemy : PoolableObject
{
    public EnemyMovement Movement;
    public NavMeshAgent Agent;
    public Transform DestroyBlock;

    private int Health;
    public int enemyID;

    [SerializeField]
    private float lootForPlayer = 1;



    public void Start()
    {
        Health = 100;
    }

    private void Update()
    {
        checkForDespawn();
    }

    // this function is supposed to disable collision between agents but it doesn't work
    // on NavMeshAgents there is currently only the option to select Quality "None" for Obstacle Avoidance if you want them
    //      to not collide with each other. However that also results in no other collisions being registered anymore.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            Physics.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider>());
    }

    private void checkForDespawn()
    {
        float xDistance = Math.Abs(Agent.transform.position.x - DestroyBlock.position.x);
        float zDistance = Math.Abs(Agent.transform.position.z - DestroyBlock.position.z);

        if ((xDistance + zDistance) < 0.2)
        {
            gameObject.SetActive(false);
            //or: Destroy(gameObject);
            PlayerStats.Instance.TakeDamage((float)0.25);
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();

        Agent.enabled = false;
    }

    public void setEnemyID(int id)
    {
        this.enemyID = id;
    }

    public int getEnemyID()
    {
        return this.enemyID;
    }

    public int getHealth()
    {
        return Health;
    }

    public void Hit(int damage)
    {
        Health -= damage;
        if (Health < 0 && gameObject.activeSelf)
        {
            PlayerStats.Instance.CollectLoot(lootForPlayer);
            gameObject.SetActive(false);
            Destroy(gameObject);
            Destroy(this);
        }
    }


}
