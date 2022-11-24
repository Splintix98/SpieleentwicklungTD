using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public GameObject enemy;
    public Transform tower;

    public float projectileSpeed;
    public float projectileDamage;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.Find("TurtleShell");
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
            EnemyHealthController enemyHealthController = otherObject.GetComponent<EnemyHealthController>();
            if (enemyHealthController == null) return;
            enemyHealthController.hit(projectileDamage);
        }
        Destroy(this.gameObject);
    }

    public void setProjectileSpeed(float projectileSpeed)
    {
        this.projectileSpeed = projectileSpeed;
    }

    public void setprojectileDamage(float projectileDamage)
    {
        this.projectileDamage = projectileDamage;
    }
}
