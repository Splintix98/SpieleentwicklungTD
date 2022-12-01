using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public GameObject towerAttributesContainer;
    public TextMeshProUGUI towerFireRate;
    public TextMeshProUGUI towerDamage;
    public TextMeshProUGUI towerRange;

    public bool allowNewTowerConstruction { get; set; } = true;

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

    public void CreateTower(GameObject tower)
    {
        if (allowNewTowerConstruction)
        {
            GameObject newTower = Instantiate(tower, new Vector3(0, 0, 0), Quaternion.identity);
            newTower.SetActive(true);
            Draggable draggable = newTower.AddComponent<Draggable>();
            draggable.buildArea = buildArea;
            draggable.buildAreaHalf = buildAreaHalf;
            draggable.constructionMenu = this;
            Clickable clickable = newTower.AddComponent<Clickable>();
            clickable.towerMenu = towerMenu.GetComponent<TowerMenu>();
            allowNewTowerConstruction = false;

            if (newTower.name == "Tower_Fire(Clone)")
            {
                TowerController towercontroller = newTower.GetComponent<TowerController>();
                towercontroller.setProjectilePreset(Resources.Load("Prefabs/fireProjectile/fireProjectile") as GameObject);
            }
            if (newTower.name == "Tower_Earth(Clone)")
            {
                TowerController towercontroller = newTower.GetComponent<TowerController>();
                towercontroller.setProjectilePreset(Resources.Load("Prefabs/stoneProjectile/stoneProjectile") as GameObject);
            }
            if (newTower.name == "Tower_Air(Clone)")
            {
                TowerController towercontroller = newTower.GetComponent<TowerController>();
                towercontroller.setProjectilePreset(Resources.Load("Prefabs/stoneProjectile/stoneProjectile") as GameObject);

            }
            if (newTower.name == "Tower_Water(Clone)")
            {
                TowerController towercontroller = newTower.GetComponent<TowerController>();
                towercontroller.setProjectilePreset(Resources.Load("Prefabs/stoneProjectile/stoneProjectile") as GameObject);
            }
        }
    }

    public void ShowTowerAttributes(GameObject tower)
    {
        TowerController towerController = tower.GetComponent<TowerController>();
        towerDamage.text = "Damage: " + towerController.towerDamage;
        towerFireRate.text = "Fire rate: " + towerController.fireRate ;
        towerRange.text = "Range: " + towerController.towerRange;

        towerAttributesContainer.SetActive(true);
    }

    public void HideTowerAttributes()
    {
        towerAttributesContainer.SetActive(false);
    }
}
