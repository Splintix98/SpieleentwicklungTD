using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TowerMenu : MonoBehaviour
{
    public GameObject towerPointer;
    public GameObject constructionMenu;
    public GameObject upgradeMenu;
    private GameObject selectedTower;
    public GameObject towerMenu;

    public TextMeshProUGUI towerName;
    public TextMeshProUGUI towerHealth;
    public TextMeshProUGUI towerDamage;
    public TextMeshProUGUI towerRange;

    public TextMeshProUGUI upgrade_1_label;
    public TextMeshProUGUI upgrade_2_label;
    public TextMeshProUGUI upgrade_3_label;
    public TextMeshProUGUI upgrade_1_costs_label;
    public TextMeshProUGUI upgrade_2_costs_label;
    public TextMeshProUGUI upgrade_3_costs_label;

    public Object[] banner_upgrade_path_1;
    public Object[] banner_upgrade_path_2;
    public Object[] banner_upgrade_path_3;

    private int level_upgrade_1;
    private int level_upgrade_2;
    private int level_upgrade_3;

    public TextMeshProUGUI infoTextForTowerFocus;

    // Start is called before the first frame update
    void Start()
    {
        constructionMenu.SetActive(true);
        towerMenu.SetActive(false);
        upgradeMenu.SetActive(false);

        level_upgrade_1 = 0;
        level_upgrade_2 = 0;
        level_upgrade_3 = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && selectedTower)
        {
            CloseMenu();
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
        towerMenu.SetActive(true);
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
        towerMenu.SetActive(false);
        upgradeMenu.SetActive(true);

        // rename different "upgrade" labels.
        upgrade_1_label.text = "more\nTowerrange";
        upgrade_2_label.text = "more\nProjectile-\ndamage";
        upgrade_3_label.text = "more\nAttackspeed";

        upgrade_1_costs_label.text = "costs: 1";
        upgrade_2_costs_label.text = "costs: 1";
        upgrade_3_costs_label.text = "costs: 1";
    }

    // ---- Upgrade path 1 ---------------------------

    public void do_upgrade_1()
    {
        if (level_upgrade_1 == 0 && (level_upgrade_2 == 0 || level_upgrade_3 == 0))
        {
            selectedTower.GetComponent<TowerController>().setTowerRange(selectedTower.GetComponent<TowerController>().getTowerRange() * 1.05f);
            upgrade_1_costs_label.text = "costs: 1";
            level_upgrade_1 += 1;
            banner_upgrade_path_1[0].GetComponent<RawImage>().color = new Color(255, 0, 0, 0.5f);

            if (level_upgrade_2 > 0)
            {
                level_upgrade_3 = -1;
                upgrade_3_costs_label.text = "max level";
                banner_upgrade_path_3[0].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
                banner_upgrade_path_3[1].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            }
            if (level_upgrade_3 > 0)
            {
                level_upgrade_2 = -1;
                upgrade_2_costs_label.text = "max level";
                banner_upgrade_path_2[0].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
                banner_upgrade_path_2[1].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            }
        }
        else if (level_upgrade_1 == 1)
        {
            selectedTower.GetComponent<TowerController>().setTowerRange(selectedTower.GetComponent<TowerController>().getTowerRange() * 1.05f);
            upgrade_1_costs_label.text = "costs: 2";
            level_upgrade_1 += 1;
            banner_upgrade_path_1[1].GetComponent<RawImage>().color = new Color(255, 0, 0, 0.5f);

            if (level_upgrade_2 >= 2 || level_upgrade_3 >= 2)
            {
                upgrade_1_costs_label.text = "max level";
            }
        }
        else if (level_upgrade_1 == 2 && level_upgrade_2 <= 2 && level_upgrade_3 <= 2)
        {
            selectedTower.GetComponent<TowerController>().setTowerRange(selectedTower.GetComponent<TowerController>().getTowerRange() * 1.05f);
            upgrade_1_costs_label.text = "costs: 3";
            level_upgrade_1 += 1;
            banner_upgrade_path_1[2].GetComponent<RawImage>().color = new Color(255, 0, 0, 0.5f);

            banner_upgrade_path_2[2].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            banner_upgrade_path_2[3].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            banner_upgrade_path_3[2].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            banner_upgrade_path_3[3].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
        }
        else if (level_upgrade_1 == 3)
        {
            selectedTower.GetComponent<TowerController>().setTowerRange(selectedTower.GetComponent<TowerController>().getTowerRange() * 1.05f);
            upgrade_1_costs_label.text = "max level";
            level_upgrade_1 += 1;
            banner_upgrade_path_1[3].GetComponent<RawImage>().color = new Color(255, 0, 0, 0.5f);
        } else
        {
            upgrade_1_costs_label.text = "max level";
        }
    }

    // ---- Upgrade path 2 ---------------------------

    public void do_upgrade_2()
    {
        if (level_upgrade_2 == 0 && (level_upgrade_1 == 0 || level_upgrade_3 == 0))
        {
            selectedTower.GetComponent<TowerController>().setTowerDamage(selectedTower.GetComponent<TowerController>().getTowerDamage() * 1.05f);
            upgrade_2_costs_label.text = "costs: 1";
            level_upgrade_2 += 1;
            banner_upgrade_path_2[0].GetComponent<RawImage>().color = new Color(255, 0, 0, 0.5f);

            if (level_upgrade_1 > 0)
            {
                level_upgrade_3 = -1;
                upgrade_3_costs_label.text = "max level";
                banner_upgrade_path_3[0].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
                banner_upgrade_path_3[1].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            }
            if (level_upgrade_3 > 0)
            {
                level_upgrade_1 = -1;
                upgrade_1_costs_label.text = "max level";
                banner_upgrade_path_1[0].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
                banner_upgrade_path_1[1].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            }
        }
        else if (level_upgrade_2 == 1)
        {
            selectedTower.GetComponent<TowerController>().setTowerDamage(selectedTower.GetComponent<TowerController>().getTowerDamage() * 1.05f);
            upgrade_2_costs_label.text = "costs: 2";
            level_upgrade_2 += 1;
            banner_upgrade_path_2[1].GetComponent<RawImage>().color = new Color(255, 0, 0, 0.5f);

            if (level_upgrade_1 >= 2 || level_upgrade_3 >= 2)
            {
                upgrade_2_costs_label.text = "max level";
            }
        }
        else if (level_upgrade_2 == 2 && level_upgrade_1 <= 2 && level_upgrade_3 <= 2)
        {
            selectedTower.GetComponent<TowerController>().setTowerDamage(selectedTower.GetComponent<TowerController>().getTowerDamage() * 1.05f);
            upgrade_2_costs_label.text = "costs: 3";
            level_upgrade_2 += 1;
            banner_upgrade_path_2[2].GetComponent<RawImage>().color = new Color(255, 0, 0, 0.5f);

            banner_upgrade_path_1[2].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            banner_upgrade_path_1[3].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            banner_upgrade_path_3[2].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            banner_upgrade_path_3[3].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
        }
        else if (level_upgrade_2 == 3)
        {
            selectedTower.GetComponent<TowerController>().setTowerDamage(selectedTower.GetComponent<TowerController>().getTowerDamage() * 1.05f);
            upgrade_2_costs_label.text = "max level";
            level_upgrade_2 += 1;
            banner_upgrade_path_2[3].GetComponent<RawImage>().color = new Color(255, 0, 0, 0.5f);
        }
        else
        {
            upgrade_1_costs_label.text = "max level";
        }
    }

    // ---- Upgrade path 3 ---------------------------

    public void do_upgrade_3()
    {
        if (level_upgrade_3 == 0 && (level_upgrade_2 == 0 || level_upgrade_3 == 0))
        {
            selectedTower.GetComponent<TowerController>().setFireRate(selectedTower.GetComponent<TowerController>().getFireRate() * 1.05f);
            upgrade_3_costs_label.text = "costs: 1";
            level_upgrade_3 += 1;
            banner_upgrade_path_3[0].GetComponent<RawImage>().color = new Color(255, 0, 0, 0.5f);

            if (level_upgrade_1 > 0)
            {
                level_upgrade_2 = -1;
                upgrade_2_costs_label.text = "max level";
                banner_upgrade_path_2[0].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
                banner_upgrade_path_2[1].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            }
            if (level_upgrade_2 > 0)
            {
                level_upgrade_1 = -1;
                upgrade_1_costs_label.text = "max level";
                banner_upgrade_path_1[0].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
                banner_upgrade_path_1[1].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            }
        }
        else if (level_upgrade_3 == 1)
        {
            selectedTower.GetComponent<TowerController>().setFireRate(selectedTower.GetComponent<TowerController>().getFireRate() * 1.05f);
            upgrade_3_costs_label.text = "costs: 2";
            level_upgrade_3 += 1;
            banner_upgrade_path_3[1].GetComponent<RawImage>().color = new Color(255, 0, 0, 0.5f);

            if (level_upgrade_1 >= 2 || level_upgrade_2 >= 2)
            {
                upgrade_3_costs_label.text = "max level";
            }
        }
        else if (level_upgrade_3 == 2 && level_upgrade_2 <= 1 && level_upgrade_3 <= 2)
        {
            selectedTower.GetComponent<TowerController>().setFireRate(selectedTower.GetComponent<TowerController>().getFireRate() * 1.05f);
            upgrade_3_costs_label.text = "costs: 3";
            level_upgrade_3 += 1;
            banner_upgrade_path_3[2].GetComponent<RawImage>().color = new Color(255, 0, 0, 0.5f);

            banner_upgrade_path_1[2].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            banner_upgrade_path_1[3].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            banner_upgrade_path_2[2].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            banner_upgrade_path_2[3].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
        }
        else if (level_upgrade_3 == 3)
        {
            selectedTower.GetComponent<TowerController>().setFireRate(selectedTower.GetComponent<TowerController>().getFireRate() * 1.05f);
            upgrade_3_costs_label.text = "max level";
            level_upgrade_3 += 1;
            banner_upgrade_path_3[3].GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
        }
        else
        {
            upgrade_3_costs_label.text = "max level";
        }
    }

    // ----------------------------------------------

    public void DestroyTower()
    {
        towerPointer.SetActive(false);
        towerMenu.SetActive(false);
        constructionMenu.SetActive(true);

        Destroy(selectedTower);
        selectedTower = null;
    }


    public void CloseMenu() {
        TowerController towerController = selectedTower.GetComponent<TowerController>();
        towerPointer.SetActive(false);
        towerMenu.SetActive(false);
        upgradeMenu.SetActive(false);
        constructionMenu.SetActive(true);
        towerController.EnableLineRender = false;
        selectedTower = null;

    }


}
