using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ProjectileController : MonoBehaviour
{
    public Enemy enemy;
    public Transform tower;

    public float projectileSpeed;
    public float projectileDamage;

    // projectileTyp = "Tower_Fire(Clone)", "Tower_Earth(Clone)", "Tower_Air(Clone)", "Tower_Water(Clone)"  -> (for effects)
    private string projectileType;

    private bool projectileCollidated;

    // effects for projectiles (fire)
    private float burningDamage;
    // effects for projectiles (water/ice)
    private float slownessStrength;
    // effects for projectiles (earth)
    private float clusterDamagePercent;
    private float rangeClusterDamage;
    // effects for projectiles (air)
    private float EnemyReturnDuration;
    public Transform TargetPosition;
    public Transform SpawnPosition;

    // Start is called before the first frame update
    void Start()
    {
            projectileCollidated = false;
    }

    // Update is called once per frame
    void Update()
    {
        // check if enemy is avaible
        if (!enemy.IsDestroyed() && enemy.isActiveAndEnabled)
        {
            transform.position = Vector3.MoveTowards(transform.position, enemy.transform.position, projectileSpeed * Time.deltaTime);
            enemy.SetBurningDamage(burningDamage);
            enemy.setSlownessStrength(slownessStrength);

            // timer fire
            if (enemy.getBurnTimer() >= 0.0f && enemy.getEnemyIsBurning())
            {
                enemy.setBurnTimer(enemy.getBurnTimer() - Time.deltaTime);
            }
            // remove projectile if projectile already hit enemy
            else if (enemy.getBurnTimer() < 0.0f && projectileCollidated)
            {
                Destroy(gameObject);
                Destroy(this);
            } 
          

            // timer slow
            if (enemy.getSlowTimer() >= 0.0f && enemy.getEnemyIsSlowed())
            {
                enemy.setSlowTimer(enemy.getSlowTimer() - Time.deltaTime);
            }
            // remove projectile if projectile already hit enemy
            else if (enemy.getSlowTimer() < 0.0f && projectileCollidated)
            {
                Destroy(gameObject);
                Destroy(this);
            }

            // timer return to spawn
            if (enemy.getGoToSpawnTimer() >= 0.0f && enemy.getEnemyGoToSpawn())
            {
                enemy.setGoToSpawnTimer(enemy.getGoToSpawnTimer() - Time.deltaTime);
            }
            else if (enemy.getGoToSpawnTimer() < 0.0f && projectileCollidated)
            {
                Destroy(gameObject);
                Destroy(this);
            }
        }
        else
        {
            Destroy(gameObject);
            Destroy(this);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // check collision from projectile and enemy
        GameObject otherObject = collision.gameObject;
        // ignore tower collision (projectil spawn there) and check for focused Enemy
        if (otherObject.CompareTag("Enemy"))
        {
            if (enemy.getEnemyID() == otherObject.GetComponent<Enemy>().getEnemyID())
            {
                // hit enemy if exist
                Enemy enemyHealthController = otherObject.GetComponent<Enemy>();
                if (enemyHealthController == null)
                {
                    return;
                    //Destroy(gameObject);
                    //Destroy(this);
                }
                enemyHealthController.Hit(projectileDamage);

                // set effect from fire projectile
                if (projectileType == "Tower_Fire(Clone)")
                {
                    // disable all elements from fire projectile, expect fire animation ("burn" animation)
                    this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    this.gameObject.transform.GetChild(2).gameObject.SetActive(false);
                    this.gameObject.transform.GetChild(3).gameObject.SetActive(false);
                    this.gameObject.transform.GetChild(4).gameObject.SetActive(false);

                    // game mechanic: removes slow if burn
                    if (enemy.getEnemyIsSlowed())
                    {
                        enemy.setEnemyIsSlowed(false);
                    }

                    // set / reset burn timer
                    enemy.setBurnTimer(2.0f);

                    // destroy projectile, if enemy is already burning (bugfix to prevent stacking from fireanimation at enemy)
                    if (enemy.getEnemyIsBurning())
                    {
                        Destroy(gameObject);
                        Destroy(this);
                    }

                    // set burn to active
                    enemy.setEnemyIsBurning(true);

                }
                // set effect from water/ice projectile
                else if (projectileType == "Tower_Water(Clone)")
                {
                    // dsiable projectile with collider
                    this.gameObject.transform.GetChild(0).gameObject.SetActive(false);

                    // game mechanic: removes burn if slow
                    if (enemy.getEnemyIsBurning())
                    {
                        enemy.setEnemyIsBurning(false);
                    }

                    // set / reset burn timer
                    enemy.setSlowTimer(2.0f);

                    // destroy projectile, if enemy is already slowed (bugfix to prevent stacking from slowprojectiles at enemy)
                    if (enemy.getEnemyIsSlowed())
                    {
                        Destroy(gameObject);
                        Destroy(this);
                    }

                    // set slow to active
                    enemy.setEnemyIsSlowed(true);

                }
                // set effect from earth projectile
                else if (projectileType == "Tower_Earth(Clone)")
                {
                    // scan for all enemys
                    Enemy[] allAktiveEnemys = FindObjectsOfType(typeof(Enemy)) as Enemy[];

                    // iterate over all
                    foreach (Enemy enemy in allAktiveEnemys)
                    {
                        // get distance of each
                        float diffX = this.enemy.transform.position.x - enemy.transform.position.x;
                        float diffZ = this.enemy.transform.position.z - enemy.transform.position.z;
                        float hypothenuse = Mathf.Sqrt((Mathf.Pow(diffZ, 2) + Mathf.Pow(diffX, 2)));

                        // skip enemys who are not in towerrange
                        if (hypothenuse < rangeClusterDamage && this.enemy.getEnemyID() != enemy.getEnemyID())
                        {
                            // hit enemys in range with damage * factor
                            enemy.Hit(projectileDamage * clusterDamagePercent);
                        }
                    }
                    Destroy(this.gameObject);
                }
                // set effect from air projectile
                else if (projectileType == "Tower_Air(Clone)")
                {
                    this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    enemy.setGoToSpawnTimer(EnemyReturnDuration);
                    if (enemy.getEnemyGoToSpawn())
                    {
                        Destroy(this.gameObject);
                    }
                    enemy.GetComponent<Enemy>().setEnemyGoToSpawn(true);
                }

                projectileCollidated = true;
            }
        } 
        else
        {
            // ignore collision of each object, expect enemys
            Physics.IgnoreCollision(otherObject.GetComponent<Collider>(), this.transform.GetChild(0).GetComponent<Collider>());
        }
    }

    // getter & setter -------------------------------------------------------------------------
    public void setEnemy(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void setProjectileSpeed(float projectileSpeed)
    {
        this.projectileSpeed = projectileSpeed;
    }

    public void setprojectileDamage(float projectileDamage)
    {
        this.projectileDamage = projectileDamage;
    }

    public void setProjectileType(string projectileType)
    {
        this.projectileType = projectileType;
    }

    public void setBurningDamage(float burningDamage)
    {
        this.burningDamage = burningDamage;
    }

    public void setSlownessStrength(float slownessStrength)
    {
        this.slownessStrength = slownessStrength;
    }

    public void setRangeClusterDamage(float range)
    {
        this.rangeClusterDamage = range;
    }

    public void setClusterDamagePercent(float clusterDamagePercent)
    {
        this.clusterDamagePercent = clusterDamagePercent;
    }

    public void setReturnDuration(float EnemyReturnDuration)
    {
        this.EnemyReturnDuration = EnemyReturnDuration;
    }
}
