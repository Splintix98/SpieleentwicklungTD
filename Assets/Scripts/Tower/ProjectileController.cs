using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public Enemy enemy;
    public Transform tower;

    public float projectileSpeed;
    public int projectileDamage;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy)
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
        GameObject otherObject = collision.gameObject;
        if (!otherObject.name.Contains("tower"))
        {
            Enemy enemyHealthController = otherObject.GetComponent<Enemy>();
            if (enemyHealthController == null) return;
            enemyHealthController.Hit(projectileDamage);
        }
        Destroy(this.gameObject);
    }
    public void setEnemy(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void setProjectileSpeed(float projectileSpeed)
    {
        this.projectileSpeed = projectileSpeed;
    }

    public void setprojectileDamage(int projectileDamage)
    {
        this.projectileDamage = projectileDamage;
    }
}
