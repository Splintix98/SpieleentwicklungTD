using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class bulletshot : MonoBehaviour
{
    public GameObject enemy;
    public Transform tower;

    float bulletSpeed = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.Find("TurtleShell");
        tower = GameObject.Find("Tower").transform.GetChild(1).transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
       transform.position = Vector3.MoveTowards(transform.position, enemy.transform.position, bulletSpeed * Time.deltaTime);
    }

    private void OnCollisionStay(Collision collision)
    {
        Destroy(this.gameObject);
        Debug.Log("Hit!");
    }
}
