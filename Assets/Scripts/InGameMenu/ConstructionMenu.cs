using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionMenu : MonoBehaviour
{

    public GameObject constructionMenu;
    public Transform buildArea;
    public Transform buildAreaHalf;
    public GameObject forestTower;

    private GameObject newTower;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        int layerObject = 1;
        Vector3 ray = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        RaycastHit2D hit = Physics2D.Raycast(ray, ray, layerObject);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.GetComponent<GameObject>().name);
        }

    }
        public void createTower1() {

        GameObject newTower = Instantiate(forestTower, new Vector3(0, 0, 0), Quaternion.identity);
        Draggable sc = newTower.AddComponent<Draggable>();
        sc.buildArea = buildArea;
        sc.buildAreaHalf = buildAreaHalf;


    }


}
