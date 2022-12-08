using Mono.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public float towerHealth;
    public float towerRange;
    public float projectileSpeed;
    public int towerDamage;
    public float fireRate;
    [SerializeField]
    private int constructionCosts;

    // -1   = last enemy
    // 0    = nearest
    // 1    = first enemy
    public int towerModi;

    Transform towerRotationPoint;
    LineRenderer towerLineIndicator;

    public int ConstructionCosts { get { return constructionCosts; } }

    public bool EnableLineRender
    {
        get { return towerLineIndicator.enabled; }
        set { towerLineIndicator.enabled = value; }
    }

    public bool EnableShoot { get; set; }


    private GameObject bulletPrefab;

    private float lastShotCooldown;

    // Start is called before the first frame update
    void Start()
    {
        towerHealth = 100;
        //towerRange = 3;
        projectileSpeed = 5.0f;
        lastShotCooldown = 0;
        //towerDamage = 5;
        //fireRate = 1;
        towerModi = 0;

        
        towerLineIndicator = this.gameObject.GetComponent<LineRenderer>();
        towerLineIndicator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        towerRotationPoint = this.transform.GetChild(1).transform.GetChild(0);
        RenderTowerIndicator();
        if (!EnableShoot) return;
        Enemy enemy = findEnemy(towerModi);

        if (enemy == null) return;
        float diffX = enemy.transform.position.x - towerRotationPoint.transform.position.x;
        float diffZ = enemy.transform.position.z - towerRotationPoint.transform.position.z;
        float hypothenuse = Mathf.Sqrt((Mathf.Pow(diffZ, 2) + Mathf.Pow(diffX, 2)));

        rotateTowerToEnemy(enemy, diffX, diffZ, hypothenuse);

        if (lastShotCooldown <= 0)
        {
            lastShotCooldown = 1 / fireRate;
            if (hypothenuse < towerRange && bulletPrefab != null)
            {
                GameObject b = Instantiate(bulletPrefab) as GameObject;
                ProjectileController projectileController = b.GetComponent<ProjectileController>();
                projectileController.setEnemy(enemy);
                projectileController.setProjectileSpeed(projectileSpeed);
                projectileController.setprojectileDamage(towerDamage);
                b.transform.position = towerRotationPoint.transform.GetChild(2).transform.position;
            }
        }
        else
        {
            lastShotCooldown -= Time.deltaTime;
        }
    }

    // ---------------------------------------------------------------------------------

    public Enemy findEnemy(int modi)
    {
        Enemy focusedEnemy = null;
        Enemy[] allAktiveEnemys = FindObjectsOfType(typeof(Enemy)) as Enemy[];

        foreach (Enemy enemy in allAktiveEnemys)
        {
            float diffX = enemy.transform.position.x - transform.GetChild(1).transform.position.x;
            float diffZ = enemy.transform.position.z - transform.GetChild(1).transform.position.z;
            float hypothenuse = Mathf.Sqrt((Mathf.Pow(diffZ, 2) + Mathf.Pow(diffX, 2)));

            // skip enemys who are not in towerrange
            if (hypothenuse > towerRange)
            {
                continue;
            }

            //inizilize enemy
            if (focusedEnemy == null)
            {
                focusedEnemy = enemy;
            }


            switch (modi)
            {
                // focus last Enemy in Towerrange
                case -1:
                    if (enemy.getEnemyID() > focusedEnemy.getEnemyID())
                    {
                        focusedEnemy = enemy;
                    }
                    break;

                // focus nearest Enemy in Towerrange
                case 0:
                    float diffX_focusedEnemy = focusedEnemy.transform.position.x - towerRotationPoint.transform.position.x;
                    float diffZ_focusedEnemy = focusedEnemy.transform.position.z - towerRotationPoint.transform.position.z;
                    float hypothenuse_focusedEnemy = Mathf.Sqrt((Mathf.Pow(diffZ_focusedEnemy, 2) + Mathf.Pow(diffX_focusedEnemy, 2)));
                    if (hypothenuse < hypothenuse_focusedEnemy)
                    {
                        focusedEnemy = enemy;
                    }
                    break;

                // focus first Enemy in Towerrange
                case 1:
                    if (enemy.getEnemyID() < focusedEnemy.getEnemyID())
                    {
                        focusedEnemy = enemy;
                    }
                    break;
            }
        }
        return focusedEnemy;
    }

    // ---------------------------------------------------------------------------------

    public void rotateTowerToEnemy(Enemy enemy, float diffX, float diffZ, float hypothenuse)
    {
        float rotation;

        if (diffX != 0 && diffZ != 0 && hypothenuse < towerRange)
        {
            rotation = Mathf.Asin(diffX / hypothenuse) * 180 / Mathf.PI;

            if (((enemy.transform.position.x <= towerRotationPoint.transform.position.x) && (enemy.transform.position.z <= towerRotationPoint.transform.position.z)) ||
                ((enemy.transform.position.x >= towerRotationPoint.transform.position.x) && (enemy.transform.position.z <= towerRotationPoint.transform.position.z)))
            {
                rotation = 360 - rotation;
            }
            if (((enemy.transform.position.x <= towerRotationPoint.transform.position.x) && (enemy.transform.position.z > towerRotationPoint.transform.position.z)) ||
                ((enemy.transform.position.x >= towerRotationPoint.transform.position.x) && (enemy.transform.position.z >= towerRotationPoint.transform.position.z)))
            {
                rotation += 180;
            }
            towerRotationPoint.transform.SetPositionAndRotation(towerRotationPoint.transform.position, Quaternion.Euler(new Vector3(-90, 180 + rotation, 0)));
        }
    }

    // ---------------------------------------------------------------------------------

    public void RenderTowerIndicator()
    {
        int ringElements = 50;
        towerLineIndicator.startWidth = 0.05f;
        towerLineIndicator.positionCount = (ringElements + 1);

        for (int i = 0; i < (ringElements + 1); i++)
        {
            float angle = i * (360f / ringElements);
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * towerRange;
            float z = Mathf.Cos(Mathf.Deg2Rad * angle) * towerRange;
            towerLineIndicator.SetPosition(i, new Vector3(towerRotationPoint.transform.position.x + x, 2, towerRotationPoint.transform.position.z + z));
        }
    }

    // ---------------------------------------------------------------------------------

    public void setProjectilePreset(GameObject bulletPrefab)
    {
        this.bulletPrefab = bulletPrefab;
    }


    


}
