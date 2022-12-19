using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public Enemy enemy;
    public Transform tower;

    public float projectileSpeed;
    public float projectileDamage;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.isActiveAndEnabled)
        {
            transform.position = Vector3.MoveTowards(transform.position, enemy.transform.position, projectileSpeed * Time.deltaTime);
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
                Destroy(this.gameObject);
            }
        } 
        else
        {
            //Physics.IgnoreCollision(otherObject.collider, collider);
            
            Physics.IgnoreCollision(otherObject.GetComponent<Collider>(), GetComponent<Collider>());
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
}
