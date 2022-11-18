using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public float towerRange;
    public float projectileSpeed;

    Transform enemy;
    Transform towerRotationPoint;
    LineRenderer towerLineIndicator;

    public static GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        towerRange = 30;
        projectileSpeed = 5.0f;

        enemy = GameObject.Find("TurtleShell").transform.GetChild(0);
        towerRotationPoint = GameObject.Find("Tower").transform.GetChild(1).transform.GetChild(0);
        towerLineIndicator = GameObject.Find("Tower").gameObject.GetComponent<LineRenderer>();
        bulletPrefab = Resources.Load("Prefabs/Projectile") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        float diffX = enemy.transform.position.x - transform.GetChild(1).transform.position.x;
        float diffZ = enemy.transform.position.z - transform.GetChild(1).transform.position.z;
        float hypothenuse = Mathf.Sqrt((Mathf.Pow(diffZ, 2) + Mathf.Pow(diffX, 2)));

        rotateTowerToEnemy(diffX, diffZ, hypothenuse);
        RenderTowerIndicator();

        if (Input.GetButtonDown("Fire1"))
        {
            if (hypothenuse < (towerRange / 10))
            {
                GameObject b = Instantiate(bulletPrefab) as GameObject;
                b.GetComponent<ProjectileController>().setProjectileSpeed(projectileSpeed);
                b.transform.position = towerRotationPoint.transform.GetChild(2).transform.position;
            }
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
