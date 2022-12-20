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
    private float burningDuration;
    private float slowedDuration;

    private float burningDamage;
    private float slownessStrength;

    // Start is called before the first frame update
    void Start()
    {
            burningDuration = 2.0f;
            slowedDuration = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemy.IsDestroyed() && enemy.isActiveAndEnabled)
        {
            transform.position = Vector3.MoveTowards(transform.position, enemy.transform.position, projectileSpeed * Time.deltaTime);
            enemy.SetBurningDamage(burningDamage);
            enemy.setSlownessStrength(slownessStrength);
        }
        else
        {
            Destroy(gameObject);
            Destroy(this);
        }

        // timer fire
        if (burningDuration > 0 && enemy.isActiveAndEnabled && enemy.getEnemyIsBurning())
        {
            burningDuration -= Time.deltaTime;
        } else if (burningDuration < 0 && enemy.isActiveAndEnabled && enemy.getEnemyIsBurning())
        {
            enemy.setEnemyIsBurning(false);
            Destroy(gameObject);
            Destroy(this);
        }

        // timer slow
        if (slowedDuration > 0 && enemy.isActiveAndEnabled && enemy.getEnemyIsSlowed())
        {
            slowedDuration -= Time.deltaTime;
        }
        else if (slowedDuration < 0 && enemy.isActiveAndEnabled && enemy.getEnemyIsSlowed())
        {
            enemy.setEnemyIsSlowed(false);
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

                if (projectileType == "Tower_Fire(Clone)")
                {
                    this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    this.gameObject.transform.GetChild(2).gameObject.SetActive(false);
                    this.gameObject.transform.GetChild(3).gameObject.SetActive(false);
                    this.gameObject.transform.GetChild(4).gameObject.SetActive(false);

                    enemy.setEnemyIsBurning(true);
                    burningDuration = 2.0f;

                }
                else if (projectileType == "Tower_Water(Clone)")
                {
                    this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    enemy.setEnemyIsSlowed(true);
                    slowedDuration = 2.0f;

                } else
                {
                    Destroy(this.gameObject);
                }
                enemyHealthController.Hit(projectileDamage);
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
}
