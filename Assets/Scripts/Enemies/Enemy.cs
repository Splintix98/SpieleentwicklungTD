using System.Collections;
using System.Collections.Generic;
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

    public void Start()
    {
        Health = 100;
        print("enemy start");
    }

    private void Update()
    {
        print("Agent: " + Agent.transform.position);
        print("Block: " + DestroyBlock.position);
        float xDistance = Math.Abs(Agent.transform.position.x - DestroyBlock.position.x);
        float zDistance = Math.Abs(Agent.transform.position.z - DestroyBlock.position.z);
        print("Distances: (" + xDistance + ", " + zDistance + ")");

        if ((xDistance + zDistance) < 0.2)
        {
            print("GameObject Destroyed");
            gameObject.SetActive(false);
        }
        
        /*
        print(Agent.transform.position);
        //DestroyBlock.
        NavMeshHit navMeshHit;
        if (NavMesh.SamplePosition(Agent.transform.position, out navMeshHit, 0.1f, NavMesh.GetAreaFromName("Walkable")))
        {
            print(navMeshHit.mask);
            // Destroy(gameObject);
            //or gameObject.SetActive(false);
        }
        */
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("Collision detected");
        GameObject otherObject = collision.gameObject;
        if (!otherObject.name.Contains("foresttower") || otherObject.tag == "ForestTower")
        {
            // or Destroy(gameObject);
            gameObject.SetActive(false);
            return;
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
        if (Health < 0)
        {
            Destroy(gameObject);
        }
    }


}
