using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : PoolableObject
{
    public EnemyMovement Movement;
    public NavMeshAgent Agent;
    public int Health;

    public int enemyID;

    public void Start()
    {
        Health = 100;
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
}
