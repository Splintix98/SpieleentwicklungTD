using TMPro;
using UnityEngine;

public class TowerMenu : MonoBehaviour
{
    public GameObject towerPointer;
    public GameObject constructionMenu;
    private GameObject selectedTower;
    public GameObject towerMenu;

    public TextMeshProUGUI towerName;
    public TextMeshProUGUI towerHealth;
    public TextMeshProUGUI towerDamage;
    public TextMeshProUGUI towerRange;

    public TextMeshProUGUI tomwerUpgradecosts;

    public TextMeshProUGUI infoTextForTowerFocus;

    // Start is called before the first frame update
    void Start()
    {

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

    public void setTowerFocusNext()
    {
        Debug.Log(selectedTower);
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
        constructionMenu.SetActive(true);
        towerController.EnableLineRender = false;
        selectedTower = null;

    }


}
