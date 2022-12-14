using Mono.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    // Tower and projectil properties
    public float towerHealth;
    public float towerRange;
    public float projectileSpeed;
    public float towerDamage;
    public float fireRate;
    [SerializeField]
    private int constructionCosts;

    // variable for tower "focus" state
    // -1   = last enemy
    // 0    = nearest
    // 1    = first enemy
    // 99   = strongest (most Health) enemy
    public int towerModi;

    // TODO: Kommentar
    Transform towerRotationPoint;
    LineRenderer towerLineIndicator;

    public int ConstructionCosts { get { return constructionCosts; } }

    public bool EnableLineRender
    {
        get { return towerLineIndicator.enabled; }
        set { towerLineIndicator.enabled = value; }
    }

    public bool EnableShoot { get; set; }
    public GameObject bulletPrefab;

    private float lastShotCooldown;

    // Start is called before the first frame update
    void Start()
    {
        // init variables
        towerHealth = 100;
        towerRange = 5;
        projectileSpeed = 5.0f;
        lastShotCooldown = 0;
        towerDamage = 1;
        fireRate = 1f;
        towerModi = 1;

        // disable linerenderer on start
        towerLineIndicator = this.gameObject.GetComponent<LineRenderer>();
        towerLineIndicator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // save rorationobject from tower
        towerRotationPoint = this.transform.GetChild(1).transform.GetChild(0);
        // render towerrange -> only if linerenderer is enabled
        RenderTowerIndicator();
        // catch bug -> tower shot only "placement"
        if (!EnableShoot) return;

        // get enemy to focus
        Enemy enemy = findEnemy(towerModi);

        // if enemy found, get positions and distance
        if (enemy == null) return;
        float diffX = enemy.transform.position.x - towerRotationPoint.transform.position.x;
        float diffZ = enemy.transform.position.z - towerRotationPoint.transform.position.z;
        float hypothenuse = Mathf.Sqrt((Mathf.Pow(diffZ, 2) + Mathf.Pow(diffX, 2)));

        // rotate the tower in enemy direction
        rotateTowerToEnemy(enemy, diffX, diffZ, hypothenuse);

        // check cooldown from shot
        if (lastShotCooldown <= 0)
        {

            lastShotCooldown = 1 / fireRate;

            if (hypothenuse < towerRange && bulletPrefab != null)
            {
                // create bullet and set position, enemy, speed and damage to bullet
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
            // decrease cooldown for shot
            lastShotCooldown -= Time.deltaTime;
        }
    }

    // ---------------------------------------------------------------------------------

    // function to find a enemy for the tower 
    // Multiple focus modes can be set
    public Enemy findEnemy(int modi)
    {
        // variable for final Enemy and alls Enemys in the Scene
        Enemy focusedEnemy = null;
        Enemy[] allAktiveEnemys = FindObjectsOfType(typeof(Enemy)) as Enemy[];

        // iterate over every enemy in the scene
        foreach (Enemy enemy in allAktiveEnemys)
        {
            // get distance of each
            float diffX = enemy.transform.position.x - towerRotationPoint.transform.position.x;
            float diffZ = enemy.transform.position.z - towerRotationPoint.transform.position.z;
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
                continue;
            }

            // replace focused enemy by new one if one fits better
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

                // focus strongest Enemy in Towerrange

                case 99:
                    if (enemy.getHealth() > focusedEnemy.getHealth())
                    {
                        focusedEnemy = enemy;
                    }
                    break;
            }
        }
        // return enemy to tower
        return focusedEnemy;
    }

    // ---------------------------------------------------------------------------------

    // rotate tower to enemy
    // diffX = enemy.x - tower.X (coordinates)
    // diffZ = enemy.Z - tower.Z (coordinates)
    public void rotateTowerToEnemy(Enemy enemy, float diffX, float diffZ, float hypothenuse)
    {
        float rotation;

        // check if enemy is in tower range
        if (diffX != 0 && diffZ != 0 && hypothenuse < towerRange)
        {
            // calculate base rotation
            rotation = Mathf.Asin(diffX / hypothenuse) * 180 / Mathf.PI;

            // calculate "circle"-rotation
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
            // rotate tower by calculated degrees
            towerRotationPoint.transform.SetPositionAndRotation(towerRotationPoint.transform.position, Quaternion.Euler(new Vector3(-90, 180 + rotation, 0)));
        }
    }

    // ---------------------------------------------------------------------------------

    // calculate and set towerrangeindicator
    public void RenderTowerIndicator()
    {
        // line-properties
        int ringElements = 360;
        towerLineIndicator.startWidth = 0.05f;
        towerLineIndicator.endWidth = 0.05f;
        towerLineIndicator.positionCount = ringElements + 1;

        var points = new Vector3[ringElements + 1];

        // draw each segment of the cicle
        for (int i = 0; i < (ringElements + 1); i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / ringElements);
            points[i] = new Vector3(Mathf.Sin(rad) * towerRange + towerRotationPoint.transform.position.x, 1.5f, Mathf.Cos(rad) * towerRange + towerRotationPoint.transform.position.z);
        }

        towerLineIndicator.SetPositions(points);
    }

    // ---------------------------------------------------------------------------------

    // set prefab for projectile
    public void setProjectilePreset(GameObject bulletPrefab)
    {
        this.bulletPrefab = bulletPrefab;
    }

    // ------- Getter and Setter -------------------------------------------------------

    public float getTowerRange()
    {
        return this.towerRange;
    }

    public void setTowerRange(float towerRange)
    {
        this.towerRange = towerRange;
    }

    //-----
    public float getFireRate()
    {
        return this.fireRate;
    }

    public void setFireRate(float fireRate)
    {
        this.fireRate = fireRate;
    }

    //-----
    public float getProjectileSpeed()
    {
        return this.projectileSpeed;
    }

    public void setProjectileSpeed(float projectileSpeed)
    {
        this.projectileSpeed = projectileSpeed;
    }

    //-----

    public float getLastShotCooldown()
    {
        return this.lastShotCooldown;
    }

    public void setLastShotCooldown(float lastShotCooldown)
    {
        this.lastShotCooldown = lastShotCooldown;
    }

    //-----

    public float getTowerDamage()
    {
        return this.towerDamage;
    }

    public void setTowerDamage(float towerDamage)
    {
        this.towerDamage = towerDamage;
    }

    //-----

    public int getTowerModi()
    {
        return this.towerModi;
    }

    public void setTowerModi(int towerModi)
    {
        this.towerModi = towerModi;
    }

}
