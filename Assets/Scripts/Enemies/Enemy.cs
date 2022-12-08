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
        //print("enemy start");
    }

    private void Update()
    {
        checkForDespawn();
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
