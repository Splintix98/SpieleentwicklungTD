using UnityEngine;
using UnityEngine.AI;
using System;
using static UnityEngine.EventSystems.EventTrigger;

public class Enemy : PoolableObject
{
    public EnemyMovement Movement;
    public NavMeshAgent Agent;
    public Transform DestroyBlock;

    public String Name;
    public float Health;
    public int enemyID;

    // effects on Enemy
    Boolean enemyIsBurning;
    Boolean enemyIsSlowed;
    float burningTickTimer;
    float burningDamage;
    float slownessStrength;
    bool blockInfinitySlow;

    [SerializeField]
    private float lootForPlayer = 1;

    public GameObject healthBar;

    public void Start()
    {
        Health = 100;

        // inizilize enemy effects
        enemyIsBurning = false;
        enemyIsSlowed = false;
        blockInfinitySlow = false;
        burningTickTimer = 0.2f;
    }

    private void Update()
    {
        checkForDespawn();

        if (enemyIsBurning)
        {
            if (burningTickTimer < 0)
            {
                Hit(burningDamage);
                burningTickTimer = 0.2f;
            }
            burningTickTimer -= Time.deltaTime;
        }

        if (enemyIsSlowed && !blockInfinitySlow)
        {
            this.GetComponent<NavMeshAgent>().speed = this.GetComponent<NavMeshAgent>().speed * slownessStrength;
            blockInfinitySlow = true;
        }
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

        if ((xDistance + zDistance) < 0.5)
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

    // -------------------------------------------------------------

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

    // effects on Enemy (burn) ------------------------
    public void setEnemyIsBurning(bool enemyIsBurning)
    {
        this.enemyIsBurning = enemyIsBurning;
    }
    public bool getEnemyIsBurning()
    {
        return this.enemyIsBurning;
    }
    public void SetBurningDamage(float damage)
    {
        this.burningDamage = damage;
    }

    // effects on Enemy (slow) -------------------------
    public void setEnemyIsSlowed(bool enemyIsSlowed)
    {
        this.enemyIsSlowed = enemyIsSlowed;
        blockInfinitySlow = false;
    }

    public bool getEnemyIsSlowed()
    {
        return this.enemyIsSlowed;
    }

    public void setSlownessStrength(float slownessStrength)
    {
        this.slownessStrength = slownessStrength;
    }

    // -------------------------------------------------------------

    public void Hit(float damage)
    {
        Health -= damage;
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
