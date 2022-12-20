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

    private float burningDamage;
    private float slownessStrength;
    private float clusterDamagePercent;
    private float rangeClusterDamage;

    // Start is called before the first frame update
    void Start()
    {
            projectileCollidated = false;
    }

    // Update is called once per frame
    void Update()
    {
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
            else if (enemy.getSlowTimer() < 0.0f && projectileCollidated)
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

                if (projectileType == "Tower_Fire(Clone)")
                {
                    this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    this.gameObject.transform.GetChild(2).gameObject.SetActive(false);
                    this.gameObject.transform.GetChild(3).gameObject.SetActive(false);
                    this.gameObject.transform.GetChild(4).gameObject.SetActive(false);

                    if (enemy.getEnemyIsSlowed())
                    {
                        enemy.setEnemyIsSlowed(false);
                    }

                    enemy.setBurnTimer(2.0f);

                    if (enemy.getEnemyIsBurning())
                    {
                        Destroy(gameObject);
                        Destroy(this);
                    }

                    enemy.setEnemyIsBurning(true);

                }
                else if (projectileType == "Tower_Water(Clone)")
                {
                    this.gameObject.transform.GetChild(0).gameObject.SetActive(false);

                    if (enemy.getEnemyIsBurning())
                    {
                        enemy.setEnemyIsBurning(false);
                    }

                    enemy.setSlowTimer(2.0f);

                    if (enemy.getEnemyIsSlowed())
                    {
                        enemy.setEnemyIsSlowed(false);
                    }

                    enemy.setEnemyIsSlowed(true);

                }
                else if (projectileType == "Tower_Earth(Clone)")
                {
                    Enemy[] allAktiveEnemys = FindObjectsOfType(typeof(Enemy)) as Enemy[];

                    foreach (Enemy enemy in allAktiveEnemys)
                    {
                        // get distance of each
                        float diffX = this.enemy.transform.position.x - enemy.transform.position.x;
                        float diffZ = this.enemy.transform.position.z - enemy.transform.position.z;
                        float hypothenuse = Mathf.Sqrt((Mathf.Pow(diffZ, 2) + Mathf.Pow(diffX, 2)));

                        // skip enemys who are not in towerrange
                        if (hypothenuse < rangeClusterDamage && this.enemy.getEnemyID() != enemy.getEnemyID())
                        {
                            enemy.Hit(projectileDamage * clusterDamagePercent);
                        }
                    }
                    Destroy(this.gameObject);
                }
                else if (projectileType == "Tower_Air(Clone)")
                {
                    Destroy(this.gameObject);
                }

                projectileCollidated = true;
            }
        } 
        else
        {          
            Physics.IgnoreCollision(otherObject.GetComponent<Collider>(), this.transform.GetChild(0).GetComponent<Collider>());
        }
    }

    // set Enemy by tower
    public void setEnemy(Enemy enemy)
    {
        this.enemy = enemy;
    }

    // set projectile speed by tower
    public void setProjectileSpeed(float projectileSpeed)
    {
        this.projectileSpeed = projectileSpeed;
    }

    // set projectile damage by tower
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
}
