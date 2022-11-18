using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public GameObject enemy;
    public Transform tower;

    public float projectileSpeed;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.Find("TurtleShell");
        tower = GameObject.Find("Tower").transform.GetChild(1).transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
       transform.position = Vector3.MoveTowards(transform.position, enemy.transform.position, projectileSpeed * Time.deltaTime);
    }

    private void OnCollisionStay(Collision collision)
    {
        Destroy(this.gameObject);
        Debug.Log("Hit!");
    }

    public void setProjectileSpeed(float projectileSpeed)
    {
        this.projectileSpeed = projectileSpeed;
    }
}
