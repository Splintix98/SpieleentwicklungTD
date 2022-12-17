using UnityEngine;
using UnityEngine.AI;
using System;

public class Enemy : PoolableObject
{
    public EnemyMovement Movement;
    public NavMeshAgent Agent;
    public Transform DestroyBlock;

    public String Name;
    public float Health;
    public int enemyID;

    [SerializeField]
    private float lootForPlayer = 1;

    public GameObject healthBar;

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
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            Physics.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider>());
    }
    */

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

    void OnMouseDown()
    {
        EnemyMenu.Instance.ShowEnemyInformation(gameObject, healthBar);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        Agent.enabled = false;
        if (gameObject == EnemyMenu.Instance.SelectedEnemy)
        {
            EnemyMenu.Instance.CloseMenu();
        }
    }

    public void setEnemyID(int id)
    {
        this.enemyID = id;
    }

    public int getEnemyID()
    {
        return this.enemyID;
    }

    public float getHealth()
    {
        return Health;
    }

    public void Hit(float damage)
    {
        Health -= damage;
        Debug.Log(Health);
        if (Health <= 0 && gameObject.activeSelf)
        {
            if (gameObject == EnemyMenu.Instance.SelectedEnemy)
            {
                EnemyMenu.Instance.CloseMenu();
            }
            PlayerStats.Instance.CollectLoot(lootForPlayer);
            gameObject.SetActive(false);
            Destroy(gameObject);
            Destroy(this);
        }
        else
        {
            EnemyHealthBar enemyHealthBar = healthBar.GetComponent<EnemyHealthBar>();
            enemyHealthBar.updateEnemyHealthBar(Health);
        }
    }


}
