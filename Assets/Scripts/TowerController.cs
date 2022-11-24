using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public float towerHealth = 100;
    public float towerRange;
    public float projectileSpeed;
    public float towerDamage;
    public float fireRate;
    public GameObject tower;

    Transform enemy;
    Transform towerRotationPoint;
    LineRenderer towerLineIndicator;
    private float lastShotCooldown = 0;

    

    public static GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //towerRange = 30;
        //projectileSpeed = 5.0f;

        enemy = GameObject.Find("TurtleShell").transform.GetChild(0);
        //towerRotationPoint = GameObject.Find("Tower").transform.GetChild(1).transform.GetChild(0);
        //towerLineIndicator = GameObject.Find("Tower").gameObject.GetComponent<LineRenderer>();
        towerRotationPoint = tower.transform.GetChild(1).transform.GetChild(0);
        towerLineIndicator = tower.gameObject.GetComponent<LineRenderer>();
        bulletPrefab = Resources.Load("Prefabs/Projectile") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy == null) return;  
        float diffX = enemy.transform.position.x - transform.GetChild(1).transform.position.x;
        float diffZ = enemy.transform.position.z - transform.GetChild(1).transform.position.z;
        float hypothenuse = Mathf.Sqrt((Mathf.Pow(diffZ, 2) + Mathf.Pow(diffX, 2)));

        rotateTowerToEnemy(diffX, diffZ, hypothenuse);
        RenderTowerIndicator();

        if (lastShotCooldown <= 0)
        {
            lastShotCooldown = fireRate;
            if (hypothenuse < (towerRange / 10))
            {
                GameObject b = Instantiate(bulletPrefab) as GameObject;
                ProjectileController projectileController = b.GetComponent<ProjectileController>();
                projectileController.setProjectileSpeed(projectileSpeed);
                projectileController.setprojectileDamage(towerDamage);
                b.transform.position = towerRotationPoint.transform.GetChild(2).transform.position;
            }
        }
        else {
            lastShotCooldown -= Time.deltaTime;
        }
    }

    // ---------------------------------------------------------------------------------

    public void rotateTowerToEnemy(float diffX, float diffZ, float hypothenuse)
    {
        float rotation;

        if (diffX != 0 && diffZ != 0 && hypothenuse < (towerRange / 10))
        {
            rotation = Mathf.Asin(diffX / hypothenuse) * 180 / Mathf.PI;

            if (((enemy.transform.position.x <= transform.GetChild(1).transform.position.x) && (enemy.transform.position.z <= transform.GetChild(1).transform.position.z)) ||
                ((enemy.transform.position.x >= transform.GetChild(1).transform.position.x) && (enemy.transform.position.z <= transform.GetChild(1).transform.position.z)))
            {
                rotation = 360 - rotation;
            }
            if (((enemy.transform.position.x <= transform.GetChild(1).transform.position.x) && (enemy.transform.position.z > transform.GetChild(1).transform.position.z)) ||
                ((enemy.transform.position.x >= transform.GetChild(1).transform.position.x) && (enemy.transform.position.z >= transform.GetChild(1).transform.position.z)))
            {
                rotation += 180;
            }
            towerRotationPoint.transform.SetPositionAndRotation(towerRotationPoint.transform.position, Quaternion.Euler(new Vector3(-90, 180 + rotation, 0)));
        }
    }

    // ---------------------------------------------------------------------------------

    public void RenderTowerIndicator()
    {
        towerLineIndicator.positionCount = (50+1);

        for (int i = 0; i < (50+1); i++)
        {
            float angle = i * (360f / 50);
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * towerRange;
            float z = Mathf.Cos(Mathf.Deg2Rad * angle) * towerRange;
            towerLineIndicator.SetPosition(i, new Vector3(x, 0, z));
        }
    }
}
