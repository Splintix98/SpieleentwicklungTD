using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.TextCore.Text;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionMenu : MonoBehaviour
{

    public GameObject constructionMenu;
    public Transform buildArea;
    public Transform buildAreaHalf;
    public GameObject towerMenu;
    public GameObject towerPointer;
    public bool AllowNewTowerConstruction { get; set; } = true;

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
        public void createTower1(GameObject tower) {
        if (AllowNewTowerConstruction)
        {
            GameObject newTower = Instantiate(tower, new Vector3(0, 0, 0), Quaternion.identity);
            newTower.SetActive(true);
            Draggable draggable = newTower.AddComponent<Draggable>();
            draggable.buildArea = buildArea;
            draggable.buildAreaHalf = buildAreaHalf;
            draggable.constructionMenu = this;
            Clickable clickable = newTower.AddComponent<Clickable>();
            clickable.towerMenu = towerMenu.GetComponent<TowerMenu>();
            AllowNewTowerConstruction = false;
        }


    }


}
