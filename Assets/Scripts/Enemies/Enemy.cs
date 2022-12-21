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
    // blocker that slowness only trigger one times 
    Boolean inifnitySlowBlocker;

    // duration of slow / burn. Set from ProjectileController
    public float burnTimer;
    float slowTimer;

    // time per burn dmg (0.2s)
    float burningTickTimer;
    // burn damage per tick
    float burningDamage;
    // slowness strength in percent
    float slownessStrength;

    [SerializeField]
    private float lootForPlayer = 1;

    public GameObject healthBar;

    public void Start()
    {
        Health = 100;

        // inizilize enemy effects
        enemyIsBurning = false;
        enemyIsSlowed = false;
        inifnitySlowBlocker = false;
        burningTickTimer = 0.2f;
    }

    private void Update()
    {
        checkForDespawn();

        // stop slow / burn effect after duration
        if (this.getBurnTimer() < 0.0f)
        {
            this.setEnemyIsBurning(false);
        }

        if (this.getSlowTimer() < 0.0f)
        {
            this.setEnemyIsSlowed(false);
        }

        // trigger burn dmg after each tick
        if (enemyIsBurning)
        {
            if (burningTickTimer < 0)
            {
                Hit(burningDamage);
                burningTickTimer = 0.2f;
            }
            burningTickTimer -= Time.deltaTime;
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

    // despawn enemys at end of map
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

    public float getBurnTimer()
    {
        return this.burnTimer;
    }

    public void setBurnTimer(float time)
    {
        this.burnTimer = time;
    }

    // effects on Enemy (slow) -------------------------
    public void setEnemyIsSlowed(bool enemyIsSlowed)
    {
        if (enemyIsSlowed)
        {
            // set slow
            this.GetComponent<NavMeshAgent>().speed = this.GetComponent<NavMeshAgent>().speed * slownessStrength;
            inifnitySlowBlocker = false;
        } else
        {
            // called from update, so here the blocker, that slow reset only trigger one time
            if (!inifnitySlowBlocker)
            {
                // reset slow
                this.GetComponent<NavMeshAgent>().speed = this.GetComponent<NavMeshAgent>().speed / slownessStrength;
                inifnitySlowBlocker = true;
            }
        }
        this.enemyIsSlowed = enemyIsSlowed;
    }

    public bool getEnemyIsSlowed()
    {
        return this.enemyIsSlowed;
    }

    public void setSlownessStrength(float slownessStrength)
    {
        this.slownessStrength = slownessStrength;
    }

    public float getSlowTimer()
    {
        return this.slowTimer;
    }
    public void setSlowTimer(float time)
    {
        this.slowTimer = time;
    }

    // -------------------------------------------------------------

    // hit an enemy with an projectile
    // decrease health from enemy
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
