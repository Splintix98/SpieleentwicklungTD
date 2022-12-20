using System;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class TowerMenu : MonoBehaviour
{


    #region Sigleton
    private static TowerMenu instance;
    public static TowerMenu Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<TowerMenu>();
            return instance;
        }
    }
    #endregion



    public GameObject towerPointer;
    public GameObject constructionMenu;
    public GameObject towerMainMenu;
    public GameObject towerOptionsMenu;
    public GameObject towerUpgradeMenu;
    public GameObject selectedTower;


    public TextMeshProUGUI towerName;
    public TextMeshProUGUI towerHealth;
    public TextMeshProUGUI towerDamage;
    public TextMeshProUGUI towerRange;

    public TextMeshProUGUI upgrade_1_label_text;
    public TextMeshProUGUI upgrade_2_label_text;
    public TextMeshProUGUI upgrade_3_label_text;

    public TextMeshProUGUI upgrade_1_costs_label;
    public TextMeshProUGUI upgrade_2_costs_label;
    public TextMeshProUGUI upgrade_3_costs_label;

    public TextMeshProUGUI playermoney;

    public Object[] progressbar_upgrade_path_1;
    public Object[] progressbar_upgrade_path_2;
    public Object[] progressbar_upgrade_path_3;

    private int level_upgrade_1;
    private int level_upgrade_2;
    private int level_upgrade_3;

    int upgrade_1_costs;
    int upgrade_2_costs;
    int upgrade_3_costs;

    bool allowUpdate_1_moneycheck;
    bool allowUpdate_2_moneycheck;
    bool allowUpdate_3_moneycheck;

    public TextMeshProUGUI infoTextForTowerFocus;

    // Start is called before the first frame update
    void Start()
    {
        constructionMenu.SetActive(true);
        towerOptionsMenu.SetActive(true);
        towerUpgradeMenu.SetActive(false);
        towerMainMenu.SetActive(false);

        level_upgrade_1 = 0;
        level_upgrade_2 = 0;
        level_upgrade_3 = 0;

        upgrade_1_costs = 1;
        upgrade_2_costs = 1;
        upgrade_3_costs = 1;

        upgrade_1_costs_label.text = "1   <sprite=1>";
        upgrade_2_costs_label.text = "1   <sprite=1>";
        upgrade_3_costs_label.text = "1   <sprite=1>";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && selectedTower)
        {
            CloseMenu();
            return;
        }
        if (Input.GetMouseButtonDown(0) && selectedTower && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject != selectedTower)
                {
                    CloseMenu();
                }
            }
            else
            {
                CloseMenu();
            }
        }


        // check if player has enough money for upgradePath 1
        if (upgrade_1_costs <= PlayerStats.Instance.Coins && !String.Equals(upgrade_1_costs_label.text, "max level"))
        {
            allowUpdate_1_moneycheck = true;
            upgrade_1_costs_label.color = new Color(255, 255, 255, 1);
        } else
        {
            allowUpdate_1_moneycheck = false;
            upgrade_1_costs_label.color = new Color(255, 0, 0, 0.5f);
        }

        // check if player has enough money for upgradePath 2
        if (upgrade_2_costs <= PlayerStats.Instance.Coins && !String.Equals(upgrade_2_costs_label.text,"max level"))
        {
            allowUpdate_2_moneycheck = true;
            upgrade_2_costs_label.color = new Color(255, 255, 255, 1);
        }
        else
        {
            allowUpdate_2_moneycheck = false;
            upgrade_2_costs_label.color = new Color(255, 0, 0, 0.5f);
        }

        // check if player has enough money for upgradePath 3
        if (upgrade_3_costs <= PlayerStats.Instance.Coins && !String.Equals(upgrade_3_costs_label.text, "max level"))
        {
            allowUpdate_3_moneycheck = true;
            upgrade_3_costs_label.color = new Color(255, 255, 255, 1);
        }
        else
        {
            allowUpdate_3_moneycheck = false;
            upgrade_3_costs_label.color = new Color(255, 0, 0, 0.5f);
        }
    }



    public void ShowTowerInformation(GameObject tower) {
        if (selectedTower != null) {
            TowerController oldTowerController = selectedTower.GetComponent<TowerController>();
            oldTowerController.EnableLineRender = false;
        }
        towerPointer.transform.position = new Vector3(tower.transform.position.x, tower.transform.position.y + 1.5f, tower.transform.position.z);
        towerPointer.transform.rotation = Camera.main.transform.rotation;
        towerPointer.transform.Rotate(0, 0, 180f);
        constructionMenu.SetActive(false);
        towerOptionsMenu.SetActive(true);
        towerUpgradeMenu.SetActive(false);
        towerMainMenu.SetActive(true);
        towerPointer.SetActive(true);
        selectedTower = tower;
        
        string name = selectedTower.name;
        TowerController towerController = selectedTower.GetComponent<TowerController>();
        towerController.EnableLineRender = true;
        name = name.Replace("_", " ");
        name = name.Replace("(", " ");
        name = name.Replace(")", " ");
        name = name.Replace("Clone", " ");

        towerName.text = "Name: " + name;
        towerHealth.text = "Health: " + towerController.towerHealth;
        towerDamage.text = "Damage: " + towerController.towerDamage.ToString();
        towerRange.text = "Range: " + towerController.towerRange;
    }

    // -------------------------------------------------

    // set focus from tower to next focustype
    public void setTowerFocusNext()
    {
        switch (selectedTower.GetComponent<TowerController>().getTowerModi())
        {
            case -1:
                selectedTower.GetComponent<TowerController>().setTowerModi(0);
                infoTextForTowerFocus.text = "nearest\nEnemy";
                break;

            case 0:
                selectedTower.GetComponent<TowerController>().setTowerModi(1);
                infoTextForTowerFocus.text = "first\nEnemy";
                break;

            case 1:
                selectedTower.GetComponent<TowerController>().setTowerModi(99);
                infoTextForTowerFocus.text = "strongest\nEnemy";
                break;

            case 99:
                selectedTower.GetComponent<TowerController>().setTowerModi(-1);
                infoTextForTowerFocus.text = "last\nEnemy";
                break;
        }


    }

    // set focus from tower to last focustype
    public void setTowerFocusLast()
    {
        switch (selectedTower.GetComponent<TowerController>().getTowerModi())
        {
            case -1:
                selectedTower.GetComponent<TowerController>().setTowerModi(99);
                infoTextForTowerFocus.text = "strongest\nEnemy";
                break;

            case 0:
                selectedTower.GetComponent<TowerController>().setTowerModi(-1);
                infoTextForTowerFocus.text = "last\nEnemy";
                break;

            case 1:
                selectedTower.GetComponent<TowerController>().setTowerModi(0);
                infoTextForTowerFocus.text = "nearest\nEnemy";
                break;

            case 99:
                selectedTower.GetComponent<TowerController>().setTowerModi(1);
                infoTextForTowerFocus.text = "first\nEnemy";
                break;
        }
    }

    // ---- Upgrades -------------------------------

    public void ShowUpgradeMenu()
    {
        Debug.Log(selectedTower.name);
        // rename different "upgrade" labels.
        if (selectedTower.name == "Tower_Fire(Clone)")
        {
            upgrade_1_label_text.text = "Towerrange";
            upgrade_2_label_text.text = "Damage";
            upgrade_3_label_text.text = "burndamage";
        }
        if (selectedTower.name == "Tower_Water(Clone)")
        {
            upgrade_1_label_text.text = "Towerrange";
            upgrade_2_label_text.text = "Damage";
            upgrade_3_label_text.text = "slowness";
        }
        if (selectedTower.name == "Tower_Earth(Clone)")
        {
            upgrade_1_label_text.text = "Clusterdamage";
            upgrade_2_label_text.text = "Damage";
            upgrade_3_label_text.text = "Clusterrange";
        }
        if (selectedTower.name == "Tower_Air(Clone)")
        {
            upgrade_1_label_text.text = "Towerrange";
            upgrade_2_label_text.text = "Damage";
            upgrade_3_label_text.text = "Attackspeed";
        }

        towerOptionsMenu.SetActive(false);
        towerUpgradeMenu.SetActive(true);
    }

    // ---- Upgrade path 1 ---------------------------

    public void do_upgrade_1()
    {
        if (level_upgrade_1 == 0 && (level_upgrade_2 == 0 || level_upgrade_3 == 0) && allowUpdate_1_moneycheck)
        {
            PlayerStats.Instance.SpendCoins(upgrade_1_costs);
            if (selectedTower.name == "Tower_Earth(Clone)")
            {
                selectedTower.GetComponent<TowerController>().setClusterDamagePercent(selectedTower.GetComponent<TowerController>().getClusterDamagePercent() + 0.2f);
            }
            else
            {
                selectedTower.GetComponent<TowerController>().setTowerRange(selectedTower.GetComponent<TowerController>().getTowerRange() * 1.05f);
            }
            level_upgrade_1 += 1;
            upgrade_1_costs += 1;
            upgrade_1_costs_label.text = upgrade_1_costs + "   <sprite=1>";
            progressbar_upgrade_path_1[0].GetComponent<RawImage>().color = new Color(255, 0, 0, 0.5f);

            if (level_upgrade_2 > 0)
            {
                level_upgrade_3 = -1;
                upgrade_3_costs_label.text = "max level";
                upgrade_3_costs_label.color = new Color(255, 0, 0, 0.5f);
                progressbar_upgrade_path_3[0].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
                progressbar_upgrade_path_3[1].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
                progressbar_upgrade_path_3[2].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
                progressbar_upgrade_path_3[3].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            }
            if (level_upgrade_3 > 0)
            {
                level_upgrade_2 = -1;
                upgrade_2_costs_label.text = "max level";
                upgrade_2_costs_label.color = new Color(255, 0, 0, 0.5f);
                progressbar_upgrade_path_2[0].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
                progressbar_upgrade_path_2[1].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
                progressbar_upgrade_path_2[2].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
                progressbar_upgrade_path_2[3].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            }
        }
        else if (level_upgrade_1 == 1 && allowUpdate_1_moneycheck)
        {
            PlayerStats.Instance.SpendCoins(upgrade_1_costs);
            if (selectedTower.name == "Tower_Earth(Clone)")
            {
                selectedTower.GetComponent<TowerController>().setClusterDamagePercent(selectedTower.GetComponent<TowerController>().getClusterDamagePercent() + 0.2f);
            }
            else
            {
                selectedTower.GetComponent<TowerController>().setTowerRange(selectedTower.GetComponent<TowerController>().getTowerRange() * 1.05f);
            }
            level_upgrade_1 += 1;
            upgrade_1_costs += 1;
            upgrade_1_costs_label.text = upgrade_1_costs + "   <sprite=1>";
            progressbar_upgrade_path_1[1].GetComponent<RawImage>().color = new Color(255, 0, 0, 0.5f);

            if (level_upgrade_2 > 2 || level_upgrade_3 > 2)
            {
                upgrade_1_costs_label.text = "max level";
                upgrade_1_costs_label.color = new Color(255, 0, 0, 0.5f);
                level_upgrade_1 = -1;
            }
        }
        else if (level_upgrade_1 == 2 && level_upgrade_2 <= 2 && level_upgrade_3 <= 2 && allowUpdate_1_moneycheck)
        {
            PlayerStats.Instance.SpendCoins(upgrade_1_costs);
            if (selectedTower.name == "Tower_Earth(Clone)")
            {
                selectedTower.GetComponent<TowerController>().setClusterDamagePercent(selectedTower.GetComponent<TowerController>().getClusterDamagePercent() + 0.2f);
            }
            else
            {
                selectedTower.GetComponent<TowerController>().setTowerRange(selectedTower.GetComponent<TowerController>().getTowerRange() * 1.05f);
            }
            level_upgrade_1 += 1;
            upgrade_1_costs += 1;
            upgrade_1_costs_label.text = upgrade_1_costs + "   <sprite=1>";
            progressbar_upgrade_path_1[2].GetComponent<RawImage>().color = new Color(255, 0, 0, 0.5f);

            progressbar_upgrade_path_2[2].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            progressbar_upgrade_path_2[3].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            progressbar_upgrade_path_3[2].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            progressbar_upgrade_path_3[3].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
        }
        else if (level_upgrade_1 == 3 && allowUpdate_1_moneycheck)
        {
            PlayerStats.Instance.SpendCoins(upgrade_1_costs);
            if (selectedTower.name == "Tower_Earth(Clone)")
            {
                selectedTower.GetComponent<TowerController>().setClusterDamagePercent(selectedTower.GetComponent<TowerController>().getClusterDamagePercent() + 0.2f);
            }
            else
            {
                selectedTower.GetComponent<TowerController>().setTowerRange(selectedTower.GetComponent<TowerController>().getTowerRange() * 1.05f);
            }
            upgrade_1_costs_label.text = "max level";
            upgrade_1_costs_label.color = new Color(255, 0, 0, 0.5f);
            level_upgrade_1 += 1;
            progressbar_upgrade_path_1[3].GetComponent<RawImage>().color = new Color(255, 0, 0, 0.5f);
        }
    }

    // ---- Upgrade path 2 ---------------------------

    public void do_upgrade_2()
    {
        if (level_upgrade_2 == 0 && (level_upgrade_1 == 0 || level_upgrade_3 == 0) && allowUpdate_2_moneycheck)
        {
            PlayerStats.Instance.SpendCoins(upgrade_2_costs);
            selectedTower.GetComponent<TowerController>().setTowerDamage(selectedTower.GetComponent<TowerController>().getTowerDamage() * 1.05f);
            level_upgrade_2 += 1;
            upgrade_2_costs += 1;
            upgrade_2_costs_label.text = upgrade_2_costs + "   <sprite=1>";
            progressbar_upgrade_path_2[0].GetComponent<RawImage>().color = new Color(255, 0, 0, 0.5f);

            if (level_upgrade_1 > 0)
            {
                level_upgrade_3 = -1;
                upgrade_3_costs_label.text = "max level";
                upgrade_3_costs_label.color = new Color(255, 0, 0, 0.5f);
                progressbar_upgrade_path_3[0].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
                progressbar_upgrade_path_3[1].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
                progressbar_upgrade_path_3[2].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
                progressbar_upgrade_path_3[3].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            }
            if (level_upgrade_3 > 0)
            {
                level_upgrade_1 = -1;
                upgrade_1_costs_label.text = "max level";
                upgrade_1_costs_label.color = new Color(255, 0, 0, 0.5f);
                progressbar_upgrade_path_1[0].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
                progressbar_upgrade_path_1[1].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
                progressbar_upgrade_path_1[2].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
                progressbar_upgrade_path_1[3].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            }
        }
        else if (level_upgrade_2 == 1 && allowUpdate_2_moneycheck)
        {
            PlayerStats.Instance.SpendCoins(upgrade_2_costs);
            selectedTower.GetComponent<TowerController>().setTowerDamage(selectedTower.GetComponent<TowerController>().getTowerDamage() * 1.05f);
            level_upgrade_2 += 1;
            upgrade_2_costs += 1;
            upgrade_2_costs_label.text = upgrade_2_costs + "   <sprite=1>";
            progressbar_upgrade_path_2[1].GetComponent<RawImage>().color = new Color(255, 0, 0, 0.5f);

            if (level_upgrade_1 > 2 || level_upgrade_3 > 2)
            {
                upgrade_2_costs_label.text = "max level";
                upgrade_2_costs_label.color = new Color(255, 0, 0, 0.5f);
                level_upgrade_2 = -1;
            }
        }
        else if (level_upgrade_2 == 2 && level_upgrade_1 <= 2 && level_upgrade_3 <= 2 && allowUpdate_2_moneycheck)
        {
            PlayerStats.Instance.SpendCoins(upgrade_2_costs);
            selectedTower.GetComponent<TowerController>().setTowerDamage(selectedTower.GetComponent<TowerController>().getTowerDamage() * 1.05f);
            level_upgrade_2 += 1;
            upgrade_2_costs += 1;
            upgrade_2_costs_label.text = upgrade_2_costs + "   <sprite=1>";
            progressbar_upgrade_path_2[2].GetComponent<RawImage>().color = new Color(255, 0, 0, 0.5f);

            progressbar_upgrade_path_1[2].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            progressbar_upgrade_path_1[3].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            progressbar_upgrade_path_3[2].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            progressbar_upgrade_path_3[3].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
        }
        else if (level_upgrade_2 == 3 && allowUpdate_2_moneycheck)
        {
            PlayerStats.Instance.SpendCoins(upgrade_2_costs);
            selectedTower.GetComponent<TowerController>().setTowerDamage(selectedTower.GetComponent<TowerController>().getTowerDamage() * 1.05f);
            upgrade_2_costs_label.text = "max level";
            upgrade_2_costs_label.color = new Color(255, 0, 0, 0.5f);
            level_upgrade_2 += 1;
            progressbar_upgrade_path_2[3].GetComponent<RawImage>().color = new Color(255, 0, 0, 0.5f);
        }
    }

    // ---- Upgrade path 3 ---------------------------

    public void do_upgrade_3()
    {

        if (level_upgrade_3 == 0 && (level_upgrade_2 == 0 || level_upgrade_3 == 0) && allowUpdate_3_moneycheck)
        {
            PlayerStats.Instance.SpendCoins(upgrade_3_costs);
            if (selectedTower.name == "Tower_Fire(Clone)")
            {
                selectedTower.GetComponent<TowerController>().setBurningDamage(selectedTower.GetComponent<TowerController>().getBurningDamage() + 1.0f);
            }
            if (selectedTower.name == "Tower_Water(Clone)")
            {
                selectedTower.GetComponent<TowerController>().setSlownessStrength(selectedTower.GetComponent<TowerController>().getSlownessStrength() + 0.1f);
            }
            if (selectedTower.name == "Tower_Earth(Clone)")
            {
                selectedTower.GetComponent<TowerController>().setRangeCluster(selectedTower.GetComponent<TowerController>().getRangeCluster() + 0.5f);
            }
            if (selectedTower.name == "Tower_Air(Clone)")
            {
                selectedTower.GetComponent<TowerController>().setFireRate(selectedTower.GetComponent<TowerController>().getFireRate() * 1.05f);
            }
            level_upgrade_3 += 1;
            upgrade_3_costs += 1;
            upgrade_3_costs_label.text = upgrade_3_costs + "   <sprite=1>";
            progressbar_upgrade_path_3[0].GetComponent<RawImage>().color = new Color(255, 0, 0, 0.5f);

            if (level_upgrade_1 > 0)
            {
                level_upgrade_2 = -1;
                upgrade_2_costs_label.text = "max level";
                upgrade_2_costs_label.color = new Color(255, 0, 0, 0.5f);
                progressbar_upgrade_path_2[0].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
                progressbar_upgrade_path_2[1].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
                progressbar_upgrade_path_2[2].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
                progressbar_upgrade_path_2[3].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            }
            if (level_upgrade_2 > 0)
            {
                level_upgrade_1 = -1;
                upgrade_1_costs_label.text = "max level";
                upgrade_1_costs_label.color = new Color(255, 0, 0, 0.5f);
                progressbar_upgrade_path_1[0].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
                progressbar_upgrade_path_1[1].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
                progressbar_upgrade_path_1[2].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
                progressbar_upgrade_path_1[3].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            }
        }
        else if (level_upgrade_3 == 1 && allowUpdate_3_moneycheck)
        {
            PlayerStats.Instance.SpendCoins(upgrade_3_costs);
            if (selectedTower.name == "Tower_Fire(Clone)")
            {
                selectedTower.GetComponent<TowerController>().setBurningDamage(selectedTower.GetComponent<TowerController>().getBurningDamage() + 1.0f);
            }
            if (selectedTower.name == "Tower_Water(Clone)")
            {
                selectedTower.GetComponent<TowerController>().setSlownessStrength(selectedTower.GetComponent<TowerController>().getSlownessStrength() + 0.1f);
            }
            if (selectedTower.name == "Tower_Earth(Clone)")
            {
                selectedTower.GetComponent<TowerController>().setRangeCluster(selectedTower.GetComponent<TowerController>().getRangeCluster() + 0.5f);
            }
            if (selectedTower.name == "Tower_Air(Clone)")
            {
                selectedTower.GetComponent<TowerController>().setFireRate(selectedTower.GetComponent<TowerController>().getFireRate() * 1.05f);
            }
            level_upgrade_3 += 1;
            upgrade_3_costs += 1;
            upgrade_3_costs_label.text = upgrade_3_costs + "   <sprite=1>";
            progressbar_upgrade_path_3[1].GetComponent<RawImage>().color = new Color(255, 0, 0, 0.5f);

            if (level_upgrade_1 > 2 || level_upgrade_2 > 2)
            {
                upgrade_3_costs_label.text = "max level";
                upgrade_3_costs_label.color = new Color(255, 0, 0, 0.5f);
                level_upgrade_3 = -1;
            }
        }
        else if (level_upgrade_3 == 2 && level_upgrade_1 <= 2 && level_upgrade_2 <= 2 && allowUpdate_3_moneycheck)
        {
            PlayerStats.Instance.SpendCoins(upgrade_3_costs);
            if (selectedTower.name == "Tower_Fire(Clone)")
            {
                selectedTower.GetComponent<TowerController>().setBurningDamage(selectedTower.GetComponent<TowerController>().getBurningDamage() + 1.0f);
            }
            if (selectedTower.name == "Tower_Water(Clone)")
            {
                selectedTower.GetComponent<TowerController>().setSlownessStrength(selectedTower.GetComponent<TowerController>().getSlownessStrength() + 0.1f);
            }
            if (selectedTower.name == "Tower_Earth(Clone)")
            {
                selectedTower.GetComponent<TowerController>().setRangeCluster(selectedTower.GetComponent<TowerController>().getRangeCluster() + 0.5f);
            }
            if (selectedTower.name == "Tower_Air(Clone)")
            {
                selectedTower.GetComponent<TowerController>().setFireRate(selectedTower.GetComponent<TowerController>().getFireRate() * 1.05f);
            }
            level_upgrade_3 += 1;
            upgrade_3_costs += 1;
            upgrade_3_costs_label.text = upgrade_3_costs + "   <sprite=1>";
            progressbar_upgrade_path_3[2].GetComponent<RawImage>().color = new Color(255, 0, 0, 0.5f);

            progressbar_upgrade_path_1[2].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            progressbar_upgrade_path_1[3].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            progressbar_upgrade_path_2[2].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            progressbar_upgrade_path_2[3].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
        }
        else if (level_upgrade_3 == 3 && allowUpdate_3_moneycheck)
        {
            PlayerStats.Instance.SpendCoins(upgrade_3_costs);
            if (selectedTower.name == "Tower_Fire(Clone)")
            {
                selectedTower.GetComponent<TowerController>().setBurningDamage(selectedTower.GetComponent<TowerController>().getBurningDamage() + 1.0f);
            }
            if (selectedTower.name == "Tower_Water(Clone)")
            {
                selectedTower.GetComponent<TowerController>().setSlownessStrength(selectedTower.GetComponent<TowerController>().getSlownessStrength() + 0.1f);
            }
            if (selectedTower.name == "Tower_Earth(Clone)")
            {
                selectedTower.GetComponent<TowerController>().setRangeCluster(selectedTower.GetComponent<TowerController>().getRangeCluster() + 0.5f);
            }
            if (selectedTower.name == "Tower_Air(Clone)")
            {
                selectedTower.GetComponent<TowerController>().setFireRate(selectedTower.GetComponent<TowerController>().getFireRate() * 1.05f);
            }
            upgrade_3_costs_label.text = "max level";
            upgrade_3_costs_label.color = new Color(255, 0, 0, 0.5f);
            level_upgrade_3 += 1;
            progressbar_upgrade_path_3[3].GetComponent<RawImage>().color = new Color(255, 0, 0, 0.5f);
        }
    }

    // ----------------------------------------------

    public void DestroyTower()
    {
        /*
        towerPointer.SetActive(false);
        towerMenu.SetActive(false);
        constructionMenu.SetActive(true);

        Destroy(selectedTower);
        selectedTower = null;
        */
        towerPointer.SetActive(false);
        towerMainMenu.SetActive(false);

        constructionMenu.SetActive(true);
        Destroy(selectedTower);
        selectedTower = null;
    }


    public void CloseMenu() {
        TowerController towerController = selectedTower.GetComponent<TowerController>();
        towerPointer.SetActive(false);
        towerOptionsMenu.SetActive(false);
        towerOptionsMenu.SetActive(true);
        towerMainMenu.SetActive(false);
        constructionMenu.SetActive(true);
        towerController.EnableLineRender = false;
        selectedTower = null;

    }


}
