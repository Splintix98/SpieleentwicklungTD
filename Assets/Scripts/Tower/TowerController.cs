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

    

    // effects for projectiles (fire)
    private float burningDamage;
    // effects for projectiles (water/ice)
    private float slownessStrength;
    // effects for projectiles (earth)
    private float rangeCluster;
    private float clusterDamagePercent;
    private float scalefactorEcplosion;
    // effects for projectiles (air)
    private float enemyReturnDuration;

    // variable for tower "focus" state
    // -1   = last enemy
    // 0    = nearest
    // 1    = first enemy
    // 99   = strongest (most Health) enemy
    public int towerModi;

    // cost for next upgrades 
    int next_upgrade_path_1_costs;
    int next_upgrade_path_2_costs;
    int next_upgrade_path_3_costs;

    // upgrade lvl counter
    private int level_upgrade_1;
    private int level_upgrade_2;
    private int level_upgrade_3;

    // blocker for path
    // 1    = half blocked
    // 0    = no limitation
    // -1   = full blocked
    private int upgrade_path_1_blocked;
    private int upgrade_path_2_blocked;
    private int upgrade_path_3_blocked;

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
    private AudioSource shotSound;

    public static bool Clickable { get; set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        // init variables
        towerRange = 4;
        towerDamage = 20.0f;
        fireRate = 1f;
        towerHealth = 100;
        projectileSpeed = 5.0f;
        lastShotCooldown = 0;
        towerModi = 1;

        // effects for projectiles (fire)
        burningDamage = 1;
        // effects for projectiles (water/ice)
        slownessStrength = 0.8f;
        // effects for projectiles (earth)
        scalefactorEcplosion = 0.2f;
        rangeCluster = 0.5f;
        clusterDamagePercent = 0.1f;
        // effects for projectiles (air)
        enemyReturnDuration = 0.5f;

        // disable linerenderer on start
        towerLineIndicator = this.gameObject.GetComponent<LineRenderer>();
        towerLineIndicator.enabled = false;

        // init variables for upgrade state
        // script "towermenu" will overwrite this to save and load the update state
        level_upgrade_1 = 0;
        level_upgrade_2 = 0;
        level_upgrade_3 = 0;

        next_upgrade_path_1_costs = 1;
        next_upgrade_path_2_costs = 1;
        next_upgrade_path_3_costs = 1;

        upgrade_path_1_blocked = 0;
        upgrade_path_2_blocked = 0;
        upgrade_path_3_blocked = 0;

        shotSound = GetComponent<AudioSource>();
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
                // create bullet and set properties
                GameObject b = Instantiate(bulletPrefab) as GameObject;
                ProjectileController projectileController = b.GetComponent<ProjectileController>();
                projectileController.setEnemy(enemy);
                projectileController.setProjectileSpeed(projectileSpeed);
                projectileController.setprojectileDamage(towerDamage);
                projectileController.setProjectileType(this.name);
                projectileController.setBurningDamage(burningDamage);
                projectileController.setSlownessStrength(slownessStrength);
                projectileController.setRangeClusterDamage(rangeCluster);
                projectileController.setClusterDamagePercent(clusterDamagePercent);
                projectileController.setReturnDuration(enemyReturnDuration);
                projectileController.setScalefactorEcplosion(scalefactorEcplosion);
                b.transform.position = towerRotationPoint.transform.GetChild(2).transform.position;
                if (shotSound && SoundManager.AllowNextShotSound())
                {
                    shotSound.volume = SoundManager.GetRandomVolume(10, 25);
                    shotSound.Play();
                }
                
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
                    if (enemy.getCurrentHealth() > focusedEnemy.getCurrentHealth())
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


    void OnMouseDown()
    {
        if (Clickable)
        {
            TowerMenu.Instance.ShowTowerInformation(gameObject);
        }

    }



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

    //-----

    public float getBurningDamage()
    {
        return this.burningDamage;
    }

    public void setBurningDamage(float burningDamage)
    {
        this.burningDamage = burningDamage;
    }

    //-----

    public float getSlownessStrength()
    {
        return this.slownessStrength;
    }

    public void setSlownessStrength(float slownessStrength)
    {
        this.slownessStrength = slownessStrength;
    }

    //-----

    public float getRangeCluster()
    {
        return this.rangeCluster;
    }

    public void setRangeCluster(float rangeCluster)
    {
        this.rangeCluster = rangeCluster;
    }

    //-----

    public float getClusterDamagePercent()
    {
        return this.clusterDamagePercent;
    }

    public void setClusterDamagePercent(float clusterDamagePercent)
    {
        this.clusterDamagePercent = clusterDamagePercent;
    }

    //-----
    public float getEnemyReturnDuration()
    {
        return this.enemyReturnDuration;
    }

    public void setEnemyReturnDuration(float enemyReturnDuration)
    {
        this.enemyReturnDuration = enemyReturnDuration;
    }

    //-----
    public float getScalefactorEcplosion()
    {
        return this.scalefactorEcplosion;
    }

    public void setScalefactorEcplosion(float scalefactorEcplosion)
    {
        this.scalefactorEcplosion = scalefactorEcplosion;
    }
    

    // ----- upgrades (getter, setter) ------

    public int getNext_upgrade_path_1_costs()
    {
        return this.next_upgrade_path_1_costs;
    }

    public int getNext_upgrade_path_2_costs()
    {
        return this.next_upgrade_path_2_costs;
    }

    public int getNext_upgrade_path_3_costs()
    {
        return this.next_upgrade_path_3_costs;
    }

    public void setNext_upgrade_path_1_costs(int next_upgrade_path_1_costs)
    {
        this.next_upgrade_path_1_costs = next_upgrade_path_1_costs;
    }

    public void setNext_upgrade_path_2_costs(int next_upgrade_path_2_costs)
    {
        this.next_upgrade_path_2_costs = next_upgrade_path_2_costs;
    }

    public void setNext_upgrade_path_3_costs(int next_upgrade_path_3_costs)
    {
        this.next_upgrade_path_3_costs = next_upgrade_path_3_costs;
    }


    // ----------------

    public int getLevel_upgrade_1()
    {
        return this.level_upgrade_1;
    }

    public int getLevel_upgrade_2()
    {
        return this.level_upgrade_2;
    }

    public int getLevel_upgrade_3()
    {
        return this.level_upgrade_3;
    }

    public void setLevel_upgrade_1(int Level_upgrade)
    {
        this.level_upgrade_1 = Level_upgrade;
    }

    public void setLevel_upgrade_2(int Level_upgrade)
    {
        this.level_upgrade_2 = Level_upgrade;
    }

    public void setLevel_upgrade_3(int Level_upgrade)
    {
        this.level_upgrade_3 = Level_upgrade;
    }

    // ----------------
    public int getUpgrade_path_1_blocked()
    {
        return this.upgrade_path_1_blocked;
    }

    public int getUpgrade_path_2_blocked()
    {
        return this.upgrade_path_2_blocked;
    }

    public int getUpgrade_path_3_blocked()
    {
        return this.upgrade_path_3_blocked;
    }

    public void setUpgrade_path_1_blocked(int upgrade_path_1_blocked)
    {
        this.upgrade_path_1_blocked = upgrade_path_1_blocked;
    }

    public void setUpgrade_path_2_blocked(int upgrade_path_2_blocked)
    {
        this.upgrade_path_2_blocked = upgrade_path_2_blocked;
    }

    public void setUpgrade_path_3_blocked(int upgrade_path_3_blocked)
    {
        this.upgrade_path_3_blocked = upgrade_path_3_blocked;
    }
}
